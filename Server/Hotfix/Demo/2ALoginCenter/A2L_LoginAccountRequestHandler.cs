
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
            #region 不存在则直接告诉账号服务器这次成功
            if (!scene.GetComponent<LoginInfiRecordComponent>().IsExist(request.AccountID))// 这个id是在account服务器随机生成的
            {
                response.Error = ErrorCode.ERR_Success;
                reply();
                return;
            }
            #endregion


            #region 拿到同一个账号所在的网关
            long zone = scene.GetComponent<LoginInfiRecordComponent>().Get(request.AccountID);//拿到账号所在的区服，通过区服信息来拿到账号的gate网关
            StartSceneConfig gateconfig = RealmGateAddressHelper.GetGate((int)zone, request.AccountID);//通过区服拿到gate网关的具体配置，让网关和把玩家踢下线
            #endregion

            
            var g2l_disconnectGateUnit = (G2L_DisconnectGateUnit)await MessageHelper.CallActor(gateconfig.InstanceId,new L2G_DisconnectGateUnit() { AccountID = request.AccountID });//登录中心给网关发消息

            //response.Error = g2l_disconnectGateUnit.Error;
            reply();

        }
    }
}
