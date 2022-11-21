using System;
using System.Net;


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
                ServerID = 1,
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

        public static async ETTask<int> GetRole(Scene zonescene)
        {
            A2C_GetRole a2C_GetRole = (A2C_GetRole) await zonescene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRole()
            {
                AccountID=zonescene.GetComponent<AccountInfoComponent>().AccountID,
                Token=zonescene.GetComponent<AccountInfoComponent>().Token,
                ServerID = 1,
            });
            if(a2C_GetRole.Error != ErrorCode.ERR_Success)
            {
                return ErrorCode.ERR_Wrong;
            }
            zonescene.GetComponent<RoleInfoComponent>().roleinfolist.Clear();
            foreach(var ele in a2C_GetRole.RoleInfoList)
            {
                RoleInfo roleInfo = zonescene.GetComponent<RoleInfoComponent>().AddChild<RoleInfo>();
                roleInfo.FromMessage(ele);
                zonescene.GetComponent<RoleInfoComponent>().roleinfolist.Add(roleInfo);
            }
            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> GetReamKey(Scene zonescene)
        {
            A2C_GetRealmKey a2C_GetRealmKey =(A2C_GetRealmKey)await zonescene.GetComponent<SessionComponent>().Session.Call(new C2A_GetRealmKey()
            {
                AccountID = zonescene.GetComponent<AccountInfoComponent>().AccountID,
                Token = zonescene.GetComponent<AccountInfoComponent>().Token,
                ServerID = 1,
            });
            if(a2C_GetRealmKey.Error != ErrorCode.ERR_Success)
            {
                Log.Debug("realmʧ��");
                return ErrorCode.ERR_Wrong;
            }

            #region ��¼realm���ص���Ϣ������AccountInfoComponent����
            zonescene.GetComponent<AccountInfoComponent>().RealmKey = a2C_GetRealmKey.RealmKey;
            zonescene.GetComponent<AccountInfoComponent>().RealmKeyAddress = a2C_GetRealmKey.RealmKeyAddress;
            #endregion

            #region �����Ѿ��õ�����realmͨ�ŵ����ƺ͵�ַ��Ҫ��account������
            zonescene.GetComponent<SessionComponent>().Session.Dispose();
            #endregion

            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> EnterGame(Scene zonescene)
        {
            string realmaddress = zonescene.GetComponent<AccountInfoComponent>().RealmKeyAddress;
            Session session = zonescene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(realmaddress));

            #region ͨ��realmkey����realm�����õ�gate��key���������address��127.0.0.1:10003��
            R2C_LoginRealm r2C_LoginRealm = (R2C_LoginRealm)await session.Call(new C2R_LoginRealm()
            {
                AccountId = zonescene.GetComponent<AccountInfoComponent>().AccountID,
                RealmTokenKey = zonescene.GetComponent<AccountInfoComponent>().RealmKey,
            });

            if (r2C_LoginRealm.Error != ErrorCode.ERR_Success)
            {
                Log.Debug("��¼realmʧ��");
                return ErrorCode.ERR_Wrong;
            }
            #endregion

            #region ��realm����r2c_loginrealm����
            session.Dispose();
            #endregion

            #region �����Ӻõ�gsession�洢��SessionComponent�У��Ա�������������
            Session gsession = zonescene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2C_LoginRealm.GateAddress));
            zonescene.GetComponent<SessionComponent>().Session = gsession;
            #endregion

            //����¼gate
            G2C_LoginGameGate g2C_LoginGameGate = (G2C_LoginGameGate) await gsession.Call(new C2G_LoginGameGate() 
            {   Key = r2C_LoginRealm.GateSessionKey,
                Account = zonescene.GetComponent<AccountInfoComponent>().AccountID,
                RoleId = zonescene.GetComponent<RoleInfoComponent>().CurrentRoleID,
            });

            Log.Debug("��¼gate�ɹ�");

            //��ʼ������Ϸ

            G2C_MyEnterGame g2C_MyEnterGame = (G2C_MyEnterGame)await gsession.Call(new C2G_MyEnterGame() { });

            if(g2C_MyEnterGame.Error != ErrorCode.ERR_Success)
            {
                Log.Debug("EnterGame��һ�׶δ���");
                return ErrorCode.ERR_Wrong;
            }
            Log.Debug("��¼��Ϸ�ɹ�");

            return ErrorCode.ERR_Success;


        }
    }
}