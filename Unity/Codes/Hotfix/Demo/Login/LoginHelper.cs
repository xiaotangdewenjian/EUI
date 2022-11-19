using System;


namespace ET
{
    //��νfeommessage���ǰѽṹ��ת��Ϊserverinfo�Լ�roleinfo
    //��νTommessage���ǰ�serverinfo�Լ�roleinfoת��Ϊ�ṹ��
    [FriendClass(typeof(AccountInfoComponent))]
    [FriendClass(typeof(ServerInfoComponent))]
    [FriendClass(typeof(RoleInfoComponent))]
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

            #region ��¼�ɹ���session�洢��SessionComponent�У��Ա�������������
            zoneScene.AddComponent<SessionComponent>().Session = accountsession;
            zoneScene.GetComponent<SessionComponent>().Session.AddComponent<PingComponent>();
            #endregion



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
            foreach (var serverstruct in a2cgetserverinfos.ServerInfoProtoList)//�����涼�ǽṹ��
            {
                #region ���������ķ��������ṹ��ת��Ϊ�ֶδ洢
                ServerInfo serverInfo = zoneScene.GetComponent<ServerInfoComponent>().AddChild<ServerInfo>();
                serverInfo.FromMessage(serverstruct);
                zoneScene.GetComponent<ServerInfoComponent>().serverinfolist.Add(serverInfo);
                #endregion
            }
            return ErrorCode.ERR_Success;
        }

        //������ɫҪ�õ����ƣ�accountid,��ɫ���֣�Ҫ������������
        public static async ETTask<int> CreateRole(Scene zonescene, string name)
        {
            //ͨ��client�����ƣ�accountid,�Լ��Լ����õĵ����ֺ�����������ɫ
            A2C_CreateRole a2ccreaterole = (A2C_CreateRole)await zonescene.GetComponent<SessionComponent>().Session.Call(new C2A_CreateRole()
            {
                AccountID = zonescene.GetComponent<AccountInfoComponent>().AccountID,
                Token = zonescene.GetComponent<AccountInfoComponent>().Token,
                Name = name,
                ServerID = zonescene.GetComponent<ServerInfoComponent>().CurrentServerID,
            }) ;
            if(a2ccreaterole.Error != ErrorCode.ERR_Success)
            {
                Log.Debug("����ʧ��");
                return a2ccreaterole.Error;
            }

            #region ���������Ľ�ɫ��Ϣ�ṹ��ת��Ϊ�ֶδ洢
            RoleInfo newroleInfo = zonescene.GetComponent<RoleInfoComponent>().AddChild<RoleInfo>();
            newroleInfo.FromMessage(a2ccreaterole.RoleInfostruct);
            zonescene.GetComponent<RoleInfoComponent>().roleinfolist.Add(newroleInfo);
            #endregion


            await ETTask.CompletedTask;
            return ErrorCode.ERR_Success;
        }
    }
}