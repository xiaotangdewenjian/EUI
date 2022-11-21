using ET.Demo.ALoginCenter;
using System;

namespace ET.Demo._2ALoginCenter
{
    public class G2L_RemoveLoginRecordHandler : AMActorRpcHandler<Scene, G2L_RemoveLoginRecord, L2G_RemoveLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_RemoveLoginRecord request, L2G_RemoveLoginRecord response, Action reply)
        {
            if(request.ServerId == scene.GetComponent<LoginInfiRecordComponent>().Get(request.AccountId))
            {
                scene.GetComponent<LoginInfiRecordComponent>().Remove(request.AccountId);
            }
            await ETTask.CompletedTask;
            reply();
        }
    }
}
