using System;
using System.Collections.Generic;
using System.Linq;

namespace ET
{
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



    }
}
