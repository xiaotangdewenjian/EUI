
using ET.Demo.ALoginCenter;

namespace ET
{
    [FriendClass(typeof(LoginInfiRecordComponent))]
    public static class LoginInfiRecordComponentSystem
    {
        public static void Add(this LoginInfiRecordComponent self, long Key, int value)
        {
            self.AccountLoginInfoDict.Add(Key, value);
        }
        public static long Get(this LoginInfiRecordComponent self, long key)
        {
            if (!self.AccountLoginInfoDict.ContainsKey(key))
                return -1;
            return self.AccountLoginInfoDict[key];
        }
        public static void Remove(this LoginInfiRecordComponent self, long value)
        {
            self.AccountLoginInfoDict.Remove(value);
        }
        public static bool IsExist(this LoginInfiRecordComponent self, long key)
        {
            return self.AccountLoginInfoDict.ContainsKey(key);
        }

    }
}
