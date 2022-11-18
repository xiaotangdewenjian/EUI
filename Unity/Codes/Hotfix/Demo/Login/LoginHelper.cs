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

        public static async ETTask<int> GetServerInfo(Scene zoneScene)
        {
            A2C_GetServerInfos a2cgetserverinfos = (A2C_GetServerInfos)await zoneScene.GetComponent<SessionComponent>().Session.Call(new C2A_GetServerInfos()
            {
                AccountID = zoneScene.GetComponent<AccountInfoComponent>().AccountID,
                Token = zoneScene.GetComponent<AccountInfoComponent>().Token
            });

            #region �յ���������������������
            if (a2cgetserverinfos.Error != ErrorCode.ERR_Success)
            {
                return a2cgetserverinfos.Error;
                Log.Debug("û�л�ȡ��������");
            }
            #endregion

            //serverinfo װ�������ֶΣ�serverinfocomponentװ��serverinfo,zonescene����serverinfocomponent
            foreach (var serverstruct in a2cgetserverinfos.ServerInfoProtoList)
            {
                ServerInfo serverInfo = zoneScene.GetComponent<ServerInfoComponent>().AddChild<ServerInfo>();
                serverInfo.FromMessage(serverstruct);
                zoneScene.GetComponent<ServerInfoComponent>().Add(serverInfo);
            }

            await ETTask.CompletedTask;
            return ErrorCode.ERR_Success;
        }
    }
}