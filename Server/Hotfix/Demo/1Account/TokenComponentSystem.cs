
namespace ET
{
    [FriendClass(typeof(TokenComponent))]
    public static class TokenComponentSystem
    {
        public static void Add(this TokenComponent self, long Key, string Token)
        {
            self.TokenDictionary.Add(Key, Token);
            self.TimeOutRemoveKey(Key).Coroutine();
        }
        public static string Get(this TokenComponent self, long Key)
        {
            return self.TokenDictionary[Key];
        }
        public static void Remove(this TokenComponent self, long Key)
        {
            self.TokenDictionary.Remove(Key);
        }
        public static async ETTask TimeOutRemoveKey(this TokenComponent self, long key)
        {
            await TimerComponent.Instance.WaitAsync(600000);
            self.Remove(key);
        }
    }
}
