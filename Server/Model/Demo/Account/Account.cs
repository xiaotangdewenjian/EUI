
namespace ET
{
    public enum AccountType
    {
        General = 0,
        BlackList = 1,
    }
    public class Account : Entity, IAwake
    {
        public string AccountName { get; set; }
        public string Password { get; set; }
        public long CreateTime { get; set; }
        public int SceneType { get; set; }
    }
}
