using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string accountname, string password)
        {
            Session accountsession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));
            A2C_LoginAccount a2C_LoginAccount = (A2C_LoginAccount)await accountsession.Call(new C2A_LoginAccount() { AccountName = accountname, Password = password });

            Log.Debug(a2C_LoginAccount.Message);
            return ErrorCode.ERR_Success;

        }
    }
}