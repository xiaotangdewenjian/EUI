
namespace ET
{
    public enum RoleInfoState
    {
        Normal = 0,
        Freeze,
    }
    public class RoleInfo:Entity,IAwake
    {
        public string Name;
        public int ServerID;        //区服id
        public int Status;
        public long AccountID;      //账号id
        public long LastLoginTime;
        public long CreateTime;
    }
}
