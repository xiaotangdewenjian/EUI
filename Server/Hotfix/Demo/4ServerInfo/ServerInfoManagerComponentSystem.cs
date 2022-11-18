
using CommandLine;
using MongoDB.Driver.Linq;

namespace ET
{
    public class ServerInfoManagerComponentAwakeSystem:AwakeSystem<ServerInfoManagerComponent>
    {
        public override void Awake(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    public class ServerInfoManagerComponentDestroySystem:DestroySystem<ServerInfoManagerComponent>
    {
        public override void Destroy(ServerInfoManagerComponent self)
        {
            foreach(var ele in self.ServerInfoList)
            {
                ele?.Dispose();
            }
            self.ServerInfoList.Clear();
        }
    }

    public class ServerInfoManagerComponentLoadSystem:LoadSystem<ServerInfoManagerComponent>
    {
        public override void Load(ServerInfoManagerComponent self)
        {
            self.Awake().Coroutine();
        }
    }

    [FriendClass(typeof(ServerInfoManagerComponent))]
    [FriendClass(typeof(ServerInfo))]

    public static class ServerInfoManagerComponentSystem
    {
        public static async ETTask Awake(this ServerInfoManagerComponent self)
        {
            var DBserverinfolist = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<ServerInfo>(d => true);
            if(DBserverinfolist.Count == 0 || DBserverinfolist == null)
            {
                self.ServerInfoList.Clear();
                var excelserverinfo = ServerInfoConfigCategory.Instance.GetAll();
                foreach(var info in excelserverinfo.Values)
                {
                    ServerInfo newserverInfo = self.AddChildWithId<ServerInfo>(info.Id);
                    newserverInfo.ServerName = info.ServerName;
                    newserverInfo.Status = (int)ServerStatus.Normal;
                    self.ServerInfoList.Add(newserverInfo);
                    await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(newserverInfo);
                }

                Log.Error("没有好的服务器");
                return;
            }

            self.ServerInfoList.Clear();
            foreach(var DBserverinfo in DBserverinfolist)
            {
                self.AddChild(DBserverinfo);
                self.ServerInfoList.Add(DBserverinfo);
            }
            await ETTask.CompletedTask;
        }
    }
}
