﻿

using System;
using System.Data;

namespace ET
{
    [FriendClass(typeof(UnitCache))]
    public class UnitCacheComponentAwakeSystem:AwakeSystem<UnitCacheComponent>
    {
        public override void Awake(UnitCacheComponent self)
        {
            self.UnitCacheKeyList.Clear();
            foreach(Type type in Game.EventSystem.GetTypes().Values)
            {
                if(type != typeof(IUnitCache) && typeof(IUnitCache).IsAssignableFrom(type))
                {
                    self.UnitCacheKeyList.Add(type.Name);
                    Log.Debug(type.Name);
                }
            }
            
            foreach(string key in self.UnitCacheKeyList)
            {
                UnitCache unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }
        }
    }



    [ObjectSystem]
    public class UnitCacheComponentDestroySystem:DestroySystem<UnitCacheComponent>
    {
        public override void Destroy(UnitCacheComponent self)
        {
            foreach(var unitCache in self.UnitCaches.Values)
            {
                unitCache.Dispose();
            }
            self.UnitCaches.Clear();
        }
    }





    [FriendClass(typeof(UnitCache))]
    [FriendClass(typeof(UnitCacheComponent))]
    public static class UnitCacheComponentSystem
    {
        public static async ETTask<Entity> Get(this UnitCacheComponent self,long id, string key)
        {
            if(!self.UnitCaches.TryGetValue(key,out UnitCache unitCache))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }
            return await unitCache.Get(id);
        }

        public static async ETTask<T> Get<T>(this UnitCacheComponent self, long id) where T : Entity
        {
            string key = typeof(T).Name;
            if(!self.UnitCaches.TryGetValue(key, out UnitCache unitCache))
            {
                unitCache = self.AddChild<UnitCache>();
                unitCache.key = key;
                self.UnitCaches.Add(key, unitCache);
            }
            return await unitCache.Get(id) as T;
        }



        public static async ETTask AddOrUpdate(this UnitCacheComponent self, long id, ListComponent<Entity> entityList)
        {
            using (ListComponent<Entity> list = ListComponent<Entity>.Create())
            {
                foreach(Entity entity in entityList)
                {
                    string key = entity.GetType().Name;
                    if(!self.UnitCaches.TryGetValue(key, out UnitCache unitCache))
                    {
                        unitCache = self.AddChild<UnitCache>();
                        unitCache.key = key;
                        self.UnitCaches.Add(key, unitCache);
                    }

                    if(list.Count > 0)
                    {
                        await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Save(id, list);
                    }
                }
            }
        }


        public static void Delete(this UnitCacheComponent self, long id)
        {
            foreach(UnitCache cache in self.UnitCaches.Values)
            {
                cache.Delete(id);
            }
        }











    }
}













