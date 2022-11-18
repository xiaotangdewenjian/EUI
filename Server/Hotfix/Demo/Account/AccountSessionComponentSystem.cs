
namespace ET
{
    public class AccountSessionComponentDestroySystem:DestroySystem<AccountSessionComponent>
    {
        public override void Destroy(AccountSessionComponent self)
        {
            self.AccountSessionDictionary.Clear();
        }
    }
    [FriendClass(typeof(AccountSessionComponent))]
    public static class AccountSessionComponentSystem
    {
        public static long Get(this AccountSessionComponent self, long accountID)
        {
            if (self.AccountSessionDictionary.ContainsKey(accountID))
            return self.AccountSessionDictionary[accountID];
            else
            return 0;
        }
        public static void Add(this AccountSessionComponent self, long accountID,long sessinoinstanceid)
        {
            self.AccountSessionDictionary.Add(accountID, sessinoinstanceid);
        }
        public static void Remove(this AccountSessionComponent self, long accountID)
        {
            self.AccountSessionDictionary.Remove(accountID);
        }


    }

}
