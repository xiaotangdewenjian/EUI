
namespace ET
{
    public class ServerInfoComponentDestroySystem : DestroySystem<ServerInfoComponent>
    {
        public override void Destroy(ServerInfoComponent self)
        {
            foreach(var ele in self.serverinfolist)
            {
                ele.Dispose();
            }
            self.serverinfolist.Clear();
        }
    }

    public static class ServerInfoComponentSystem
    {
        public static void Add(this ServerInfoComponent self,ServerInfo serverInfo)
        {
            self.serverinfolist.Add(serverInfo);
        }
    }
}
