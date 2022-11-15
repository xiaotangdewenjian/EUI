
namespace ET
{
    public class AccountInfoComponentSystemDestory:DestroySystem<AccountInfoComponent>
    {
        public override void Destroy(AccountInfoComponent self)
        {
            self.Token = string.Empty;
            self.AccountID = 0;
        }
    }
    public static class AccountInfoComponentSystem
    {

    }
}
