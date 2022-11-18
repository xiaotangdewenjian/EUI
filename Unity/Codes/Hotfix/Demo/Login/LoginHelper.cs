using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string accountname, string password)
        {
            zoneScene.AddComponent<AccountInfoComponent>();//记录这个客户端账号获得的token以及accountid
            Session accountsession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));//这session是随机的.
            A2C_LoginAccount a2C_LoginAccount = (A2C_LoginAccount)await accountsession.Call(new C2A_LoginAccount() { AccountName = accountname, Password = password });//收到Account信息
            
            #region 登陆不成功
            if(a2C_LoginAccount.Error != ErrorCode.ERR_Success)
            {
                return a2C_LoginAccount.Error;
            }
            #endregion

            #region 记录返回的信息，存在AccountInfoComponent里面
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2C_LoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountID = a2C_LoginAccount.AccountID;
            #endregion

            zoneScene.AddComponent<SessionComponent>().Session = accountsession;//不知道这是要干啥，可能就是记录仪一下session
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();

            return ErrorCode.ERR_Success;

        }
    }
}