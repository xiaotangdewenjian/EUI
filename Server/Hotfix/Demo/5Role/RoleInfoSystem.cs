
namespace ET
{
    [FriendClass(typeof(RoleInfo))]
    public static class RoleInfoSystem
    {
        public static void FromMessage(this RoleInfo self, RoleInfoProto roleInfoProto)
        {
            self.Id = roleInfoProto.ID;
            self.Name = roleInfoProto.Name;
            self.Status = roleInfoProto.State;
            self.AccountID = roleInfoProto.AccountID;
            self.CreateTime = roleInfoProto.CreateTime;
            self.ServerID = roleInfoProto.ServerID;
            self.LastLoginTime = roleInfoProto.LastLoginTime;
        }

        public static RoleInfoProto ToMessage(this RoleInfo self)
        {
            return new RoleInfoProto() { ID = self.Id, Name = self.Name, State = self.Status, AccountID = self.AccountID, CreateTime = self.CreateTime, ServerID = (int)self.AccountID, LastLoginTime = self.LastLoginTime };
        }

    }
}
