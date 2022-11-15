

using System;

namespace ET
{
    public class C2A_LoginAccountHandler:AMRpcHandler<C2A_LoginAccount,A2C_LoginAccount>
    {
        //端口是谁，这个session是谁的子节点
        protected override async ETTask Run(Session session, C2A_LoginAccount request, A2C_LoginAccount response, Action reply)
        {
            session.RemoveComponent<SessionAcceptTimeoutComponent>();//这个session可以用了，防止被断开
            var accountinfolist = await DBManagerComponent.Instance.GetZoneDB(session.DomainZone()).Query<Account>(d => d.AccountName.Equals(request.AccountName.Trim()));
            if(accountinfolist.Count > 0)//查寻到了
            {
                
            }



            await ETTask.CompletedTask;
        }
    }
}
