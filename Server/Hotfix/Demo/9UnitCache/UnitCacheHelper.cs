using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
    [ChildType]
    public static class UnitCacheHelper
    {
        public static async ETTask AddOrUpdateUnitCache<T>(this T self) where T:Entity,IUnitCache
        {
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit() { UnitId = self.Id};
            message.EntityTypes.Add(typeof(T).FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(self));
            await MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(self.Id).InstanceId, message);
        }
    
        public static async ETTask<T> GetUnitComponentCache<T>(long UnitId) where T:Entity,IUnitCache
        {
            Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit() { UnitId = UnitId };
            message.ComponentNameList.Add(typeof(T).Name);

            long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(UnitId).InstanceId;
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(instanceId, message);
            if(queryUnit.Error == ErrorCode.ERR_Success && queryUnit.EntityList.Count > 0)
            {
                return queryUnit.EntityList[0] as T;
            }
            return null;
        }


        public static async ETTask DeleteUnitCache(long unitId)
        {
            Other2UnitCache_DeleteUnit message = new Other2UnitCache_DeleteUnit() { UnitId = unitId };
            long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId;
            await MessageHelper.CallActor(instanceId, message);
        }

        //scene是gatemapconponent的scene
        public static async ETTask<Unit> GetUnitCache(Scene scene,long unitId)
        {
            #region 查询unitcache缓存服
            long instanceId = StartSceneConfigCategory.Instance.GetUnitCacheConfig(unitId).InstanceId;//unitcache缓存服的地址
            Other2UnitCache_GetUnit message = new Other2UnitCache_GetUnit() { UnitId = unitId };
            UnitCache2Other_GetUnit queryUnit = (UnitCache2Other_GetUnit)await MessageHelper.CallActor(instanceId, message);
            #endregion


            if (queryUnit.Error != ErrorCode.ERR_Success || queryUnit.EntityList.Count <= 0)
            {
                return null;
            }

            #region 拿到query中的unity
            int indexof = queryUnit.ComponentNameList.IndexOf(nameof(Unit));
            Unit unit = queryUnit.EntityList[indexof] as Unit;
            #endregion

            scene.AddChild(unit);

            #region 将组件挂载到unit身上
            foreach (Entity entity in queryUnit.EntityList)
            {
                if(entity == null || entity is Unit)
                {
                    continue;
                }
                unit.AddComponent(entity);
            }
            #endregion

            return unit;
        }

        public static void AddOrUpdateUnitAllCache(Unit unit)//unit是根据player创建的
        {
            Other2UnitCache_AddOrUpdateUnit message = new Other2UnitCache_AddOrUpdateUnit() { UnitId = unit.Id };
            message.EntityTypes.Add(unit.GetType().FullName);
            message.EntityBytes.Add(MongoHelper.ToBson(unit));

            foreach((Type key, Entity Entity) in unit.Components)
            {
                if(!typeof(IUnitCache).IsAssignableFrom(key))
                {
                    continue;
                }

                message.EntityTypes.Add(key.FullName);
                message.EntityBytes.Add(MongoHelper.ToBson(Entity));
            }

            MessageHelper.CallActor(StartSceneConfigCategory.Instance.GetUnitCacheConfig(unit.Id).InstanceId,message).Coroutine();
        }
    }
}
