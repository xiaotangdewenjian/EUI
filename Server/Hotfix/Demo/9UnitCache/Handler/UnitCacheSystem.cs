

namespace ET
{
    [ObjectSystem]
    public class UnitCacheDestroySystem: DestroySystem<UnitCache>
    {
        public override void Destroy(UnitCache self)
        {
            foreach(Entity entity in self.CacheCompoenntsDictionary.Values)
            {
                entity.Dispose();
            }
            self.CacheCompoenntsDictionary.Clear();
            self.key = null;
        }
    }

    [FriendClass(typeof(UnitCache))]
    public static class UnitCacheSystem
    {
        public static void AddOrUpdate(this UnitCache self, Entity entity)
        {
            if(entity == null)
            {
                return;
            }

            Entity oldentity;
            //根据上面的entity的id得到原本就存在CacheCompoenntsDictionary中的oldentity
            if (self.CacheCompoenntsDictionary.ContainsKey(entity.Id))
            {
                oldentity = self.CacheCompoenntsDictionary[entity.Id];
                if (entity != oldentity)//新老不一样了就要更新了
                {
                    oldentity.Dispose();
                    self.CacheCompoenntsDictionary.Remove(entity.Id);
                }
            }
            self.CacheCompoenntsDictionary.Add(entity.Id, entity);
        }
        public static async ETTask<Entity> Get(this UnitCache self, long id)
        {
            Entity entity = null;
            if(!self.CacheCompoenntsDictionary.TryGetValue(id, out entity))
            {
                entity = await DBManagerComponent.Instance.GetZoneDB(self.DomainZone()).Query<Entity>(id, self.key);
                if(entity != null)
                {
                    self.AddOrUpdate(entity);
                }
            }
            return entity;
        }


        public static void Delete(this UnitCache self, long id)
        {
            if(self.CacheCompoenntsDictionary.TryGetValue(id, out Entity entity))
            {
                entity.Dispose();
                self.CacheCompoenntsDictionary.Remove(id);
            }
        }
    }
}


















