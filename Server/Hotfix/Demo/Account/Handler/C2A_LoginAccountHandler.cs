

using System;

namespace ET
{
    //持续监听,收到消息，端口是谁，就为谁创建session,在onaccept里面创建
    public class C2A_LoginAccountHandler:AMRpcHandler<C2A_LoginAccount,A2C_LoginAccount>
    {
        //request包含了账号名称和密码
        //response包含了错误码，令牌，账户身份证.
        //account包含了，账号名和密码
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            session.RemoveComponent<SessionAcceptTimeoutComponent>();//这个session可以用了，防止被断开
            session.DomainScene().AddComponent<TokenComponent>();//让通过的账户拿到对的令牌,accountid和token
            session.DomainScene().AddComponent<AccountSessionComponent>();//session的accountid和sessionid
            await ETTask.CompletedTask;
            //using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.LoginAccount,request.AccountName.Trim().GetHashCode()))
            //{

            //}//携程锁不会用

            var accountinfolist = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<Account>(d => d.AccountName.Equals(request.AccountName.Trim()));
            Account account;//让account做session的孩子，保持同步
            #region 查询数据库
            if (accountinfolist.Count > 0)//查寻到了就把这个account存成sesssion的child,没查到则创建，并且添加child
            {
                account = accountinfolist[0];
                session.AddChild(account);
                #region 名字对了密码不对,返回一个错误码
                if (account.Password != request.Password)
                {
                    response.Error = ErrorCode.ERR_PasswordWrong;
                    reply();
                    await TimerComponent.Instance.WaitAsync(1000);
                    session.Dispose();
                    return;
                }
                #endregion
            }
            else //没有查询到,为数据库添加这个账号，当然也可以做成创建
            {
                account = session.AddChild<Account>();
                account.AccountName = request.AccountName;
                account.Password = request.Password;
                await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Save<Account>(account);
            }
            #endregion
            //这里通过验证了

            #region 登录中心服查询
            StartSceneConfig startSceneConfig = StartSceneConfigCategory.Instance.GetBySceneName(1,"LoginCenter");//1服的LoginCenter
            long LoginCenterInstanceID = startSceneConfig.InstanceId;//这个instance就是actor通讯的地址
            var loginaccountResponse = (L2A_LoginAccountResponse)await ActorMessageSenderComponent.Instance.Call(LoginCenterInstanceID,new A2L_LoginAccountRequest() { AccountID = account.Id});//账号服务器生成的accountID发送给登陆中心服，从而让他记录一下,LoginCenterInstanceID是提前配置好的，就像constvalue中的地址一样
            if (loginaccountResponse.Error != ErrorCode.ERR_Success)
            {
                response.Error = loginaccountResponse.Error;
                reply();
                await TimerComponent.Instance.WaitAsync(1000);
                session.Dispose();
                account.Dispose();
                return;
            }
            #endregion

            #region 顶号操作
            long sessioninstanceidfromaccount = session.DomainScene().GetComponent<AccountSessionComponent>().Get(account.Id);//别人也会顶掉好像
            Session secondsession = Game.EventSystem.Get(sessioninstanceidfromaccount) as Session;
            if(secondsession != null)
            {
                secondsession.Send(new A2C_Disconnect() { Error = 0 });
                await TimerComponent.Instance.WaitAsync(1000);
                secondsession.Dispose();
                session.DomainScene().GetComponent<AccountSessionComponent>().Add(account.Id, session.InstanceId);
            }
            #endregion

            string Token = TimeHelper.ServerNow().ToString();
            session.DomainScene().GetComponent<TokenComponent>().Add(account.Id, Token);//让通过的账户拿到对的令牌

            response.AccountID = account.Id;
            response.Token = Token;
            response.Error = ErrorCode.ERR_Success;
            reply();
            account?.Dispose();
        }
    }
}
