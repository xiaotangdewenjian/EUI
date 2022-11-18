using System;


namespace ET
{
    public static class LoginHelper
    {
        public static async ETTask<int> Login(Scene zoneScene, string address, string accountname, string password)
        {
            zoneScene.AddComponent<AccountInfoComponent>();//��¼����ͻ����˺Ż�õ�token�Լ�accountid
            Session accountsession = zoneScene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(address));//��session�������.
            A2C_LoginAccount a2C_LoginAccount = (A2C_LoginAccount)await accountsession.Call(new C2A_LoginAccount() { AccountName = accountname, Password = password });//�յ�Account��Ϣ
            
            #region ��½���ɹ�
            if(a2C_LoginAccount.Error != ErrorCode.ERR_Success)
            {
                return a2C_LoginAccount.Error;
            }
            #endregion

            #region ��¼���ص���Ϣ������AccountInfoComponent����
            zoneScene.GetComponent<AccountInfoComponent>().Token = a2C_LoginAccount.Token;
            zoneScene.GetComponent<AccountInfoComponent>().AccountID = a2C_LoginAccount.AccountID;
            #endregion

            zoneScene.AddComponent<SessionComponent>().Session = accountsession;//��֪������Ҫ��ɶ�����ܾ��Ǽ�¼��һ��session
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();

            return ErrorCode.ERR_Success;

        }
    }
}