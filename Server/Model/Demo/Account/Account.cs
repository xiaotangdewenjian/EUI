﻿
namespace ET
{
    public enum AccountType
    {
        General = 0,
        BlackList = 1,
    }
    public class Account
    {
        public string AccountName;
        public string Password;
        public long CreateTime;
        public int SceneType;
    }
}
