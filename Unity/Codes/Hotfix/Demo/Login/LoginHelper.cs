using System;
using System.Net;


namespace ET
{
    //所谓feommessage就是把结构体转换为serverinfo以及roleinfo
    //所谓Tommessage就是把serverinfo以及roleinfo转换为结构体
    [FriendClass(typeof(AccountInfoComponent))]
    [FriendClass(typeof(ServerInfoComponent))]
    [FriendClass(typeof(RoleInfoComponent))]
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

            #region 登录成功把session存储到SessionComponent中，以便其他函数调用
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

            #region 收到服务器发回来的区有误
            if (a2cgetserverinfos.Error != ErrorCode.ERR_Success)
            {
                return a2cgetserverinfos.Error;
                Log.Debug("没有获取到服务器");
            }
            #endregion

            //serverinfo 装了两个字段，serverinfocomponent装了serverinfo,zonescene挂了serverinfocomponent
            foreach (var serverstruct in a2cgetserverinfos.ServerInfoProtoList)//这里面都是结构体
            {
                #region 将传回来的服务器区结构体转换为字段存储
                ServerInfo serverInfo = zoneScene.GetComponent<ServerInfoComponent>().AddChild<ServerInfo>();
                serverInfo.FromMessage(serverstruct);
                zoneScene.GetComponent<ServerInfoComponent>().serverinfolist.Add(serverInfo);
                #endregion
            }
            return ErrorCode.ERR_Success;
        }

        //创建角色要用到令牌，accountid,角色名字，要创建到的区服
        public static async ETTask<int> CreateRole(Scene zonescene, string name)
        {
            //通过client的令牌，accountid,以及自己设置的的名字和区服创建角色
            A2C_CreateRole a2ccreaterole = (A2C_CreateRole)await zonescene.GetComponent<SessionComponent>().Session.Call(new C2A_CreateRole()
            {
                AccountID = zonescene.GetComponent<AccountInfoComponent>().AccountID,
                Token = zonescene.GetComponent<AccountInfoComponent>().Token,
                Name = name,
                ServerID = 1,
            }) ;
            if(a2ccreaterole.Error != ErrorCode.ERR_Success)
            {
                Log.Debug("创建失败");
                return a2ccreaterole.Error;
            }

            #region 将传回来的角色信息结构体转换为字段存储
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
                Log.Debug("realm失败");
                return ErrorCode.ERR_Wrong;
            }

            #region 记录realm返回的信息，存在AccountInfoComponent里面
            zonescene.GetComponent<AccountInfoComponent>().RealmKey = a2C_GetRealmKey.RealmKey;
            zonescene.GetComponent<AccountInfoComponent>().RealmKeyAddress = a2C_GetRealmKey.RealmKeyAddress;
            #endregion

            #region 现在已经拿到了与realm通信的令牌和地址，要与account永别了
            zonescene.GetComponent<SessionComponent>().Session.Dispose();
            #endregion

            return ErrorCode.ERR_Success;
        }

        public static async ETTask<int> EnterGame(Scene zonescene)
        {
            string realmaddress = zonescene.GetComponent<AccountInfoComponent>().RealmKeyAddress;
            Session session = zonescene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(realmaddress));

            #region 通过realmkey，让realm帮我拿到gate的key（随机）和address（127.0.0.1:10003）
            R2C_LoginRealm r2C_LoginRealm = (R2C_LoginRealm)await session.Call(new C2R_LoginRealm()
            {
                AccountId = zonescene.GetComponent<AccountInfoComponent>().AccountID,
                RealmTokenKey = zonescene.GetComponent<AccountInfoComponent>().RealmKey,
            });

            if (r2C_LoginRealm.Error != ErrorCode.ERR_Success)
            {
                Log.Debug("登录realm失败");
                return ErrorCode.ERR_Wrong;
            }
            #endregion

            #region 和realm永别r2c_loginrealm还在
            session.Dispose();
            #endregion

            #region 把连接好的gsession存储到SessionComponent中，以便其他函数调用
            Session gsession = zonescene.GetComponent<NetKcpComponent>().Create(NetworkHelper.ToIPEndPoint(r2C_LoginRealm.GateAddress));
            zonescene.GetComponent<SessionComponent>().Session = gsession;
            #endregion

            //开登录gate
            G2C_LoginGameGate g2C_LoginGameGate = (G2C_LoginGameGate) await gsession.Call(new C2G_LoginGameGate() 
            {   Key = r2C_LoginRealm.GateSessionKey,
                Account = zonescene.GetComponent<AccountInfoComponent>().AccountID,
                RoleId = zonescene.GetComponent<RoleInfoComponent>().CurrentRoleID,
            });

            Log.Debug("登录gate成功");

            //开始进入游戏

            G2C_MyEnterGame g2C_MyEnterGame = (G2C_MyEnterGame)await gsession.Call(new C2G_MyEnterGame() { });

            if(g2C_MyEnterGame.Error != ErrorCode.ERR_Success)
            {
                Log.Debug("EnterGame第一阶段错误");
                return ErrorCode.ERR_Wrong;
            }
            Log.Debug("登录游戏成功");

            return ErrorCode.ERR_Success;


        }
    }
}