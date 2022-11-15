
namespace ET
{
    [ComponentOf(typeof(Scene))]
    public class AccountInfoComponent:Entity,IAwake,IDestroy
    {
        public string Token { get; set; }
        public long AccountID { get; set; }
    }
}
