using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string accountname, string password)
        {
            Session accountsession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));//��session�������.
            A2C_LoginAccount a2C_LoginAccount = (A2C_LoginAccount)await accountsession.Call(new C2A_LoginAccount() { AccountName = accountname, Password = password });//�յ�Account��Ϣ
            
            zoneScene.AddComponent<AccountInfoComponent>();//��¼����ͻ����˺Ż�õ�token�Լ�accountid
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2C_LoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountID = a2C_LoginAccount.AccountID;


            zoneScene.AddComponent<SessionComponent>().Session = accountsession;//��֪������Ҫ��ɶ�����ܾ��Ǽ�¼��i����session
            return ErrorCode.ERR_Success;

        }
    }
}