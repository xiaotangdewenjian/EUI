
using System;

namespace ET.Demo._6Realm
{
    public class A2R_GetRealm: AMActorRpcHandler<Scene, A2R_GetRealmKey, R2A_GetRealmKey>
    {
        protected override async ETTask Run(Scene scene, A2R_GetRealmKey request, R2A_GetRealmKey response, Action reply)
        {
            scene.DomainScene().AddComponent<TokenComponent>();
            string key = TimeHelper.ServerNow().ToString();

            #region realm验证玩家，accounid和realmkey
            scene.GetComponent<TokenComponent>().Remove(request.AccountId);
            scene.GetComponent<TokenComponent>().Add(request.AccountId, key);
            #endregion

            response.RealmKey = key.ToString();
            reply();
            await ETTask.CompletedTask;
        }
    }
}
