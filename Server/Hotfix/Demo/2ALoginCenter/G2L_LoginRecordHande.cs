
using ET.Demo.ALoginCenter;
using System;

namespace ET.Demo._2ALoginCenter
{
    public class G2L_LoginRecordHande : AMActorRpcHandler<Scene, G2L_AddLoginRecord, L2G_AddLoginRecord>
    {
        protected override async ETTask Run(Scene scene, G2L_AddLoginRecord request, L2G_AddLoginRecord response, Action reply)
        {
            scene.GetComponent<LoginInfiRecordComponent>().Remove(request.AccountId);
            scene.GetComponent<LoginInfiRecordComponent>().Add(request.AccountId, request.ServerId);
            await ETTask.CompletedTask;
            response.Error = ErrorCode.ERR_Success;
            reply();
        }
    }
}
