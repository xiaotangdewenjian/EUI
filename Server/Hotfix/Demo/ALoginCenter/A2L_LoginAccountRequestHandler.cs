
using ET.Demo.ALoginCenter;
using System;

namespace ET
{
    //通过instanceid找到这个服务器。然后这个scene就是logincenter服务器
    [ActorMessageHandler]
    public class A2L_LoginAccountRequestHandler:AMActorRpcHandler<Scene,A2L_LoginAccountRequest,L2A_LoginAccountResponse>
    {
        protected override async ETTask Run(Scene scene, A2L_LoginAccountRequest request, L2A_LoginAccountResponse response, Action reply)
        {
            if(!scene.GetComponent<LoginInfiRecordComponent>().IsExist(request.AccountID))// 这个id是在account服务器随机生成的，传过来用来保存这个账户
            {
                reply();
                return;
            }
            long zone = scene.GetComponent<LoginInfiRecordComponent>().Get(request.AccountID);//拿到账号所在的区服，通过区服信息来拿到账号的gate网关，当然现在还不确定网关
            StartSceneConfig gateconfig = RealmGateAddressHelper.GetGate((int)zone);//通过区服拿到gate网关的具体配置，让网关和把玩家踢下线
            await ETTask.CompletedTask;
        }
    }
}
