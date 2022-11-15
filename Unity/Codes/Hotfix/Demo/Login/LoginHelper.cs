using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string accountname, string password)
        {
            Session accountsession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));//这session是随机的.
            A2C_LoginAccount a2C_LoginAccount = (A2C_LoginAccount)await accountsession.Call(new C2A_LoginAccount() { AccountName = accountname, Password = password });//收到Account信息
            
            zoneScene.AddComponent<AccountInfoComponent>();//记录这个客户端账号获得的token以及accountid
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2C_LoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountID = a2C_LoginAccount.AccountID;


            zoneScene.AddComponent<SessionComponent>().Session = accountsession;//不知道这是要干啥，可能就是记录仪i西安session
            return ErrorCode.ERR_Success;

        }
    }
}