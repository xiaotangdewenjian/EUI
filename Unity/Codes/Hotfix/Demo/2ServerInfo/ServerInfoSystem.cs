
namespace ET
{
    [FriendClass(typeof(ServerInfo))]
    public static class ServerInfoSystem
    {
        public static void FromMessage(this ServerInfo self, ServerInfoProto serverstruct)
        {
            self.Id = serverstruct.ID;
            self.ServerName = serverstruct.ServerName;
            self.Status = serverstruct.Status;
        }
        public static ServerInfoProto ToMessage(this ServerInfo self)
        {
            return new ServerInfoProto() { ID = (int)self.Id, ServerName = self.ServerName, Status = self.Status };
        }

    }
}
