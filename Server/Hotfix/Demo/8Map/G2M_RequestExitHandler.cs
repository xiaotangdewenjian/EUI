
using System;

namespace ET.Demo._8Map
{
    public class G2M_RequestExitHandler : AMActorLocationRpcHandler<Unit, G2M_RequestExitGame, M2G_RequestExitGame>
    {
        protected override async ETTask Run(Unit unit, G2M_RequestExitGame request, M2G_RequestExitGame response, Action reply)
        {
            await unit.RemoveLocation();//从定位服务器移除
            //Unit unit = UnitFactory.Create(gatemapcomponent.Scene, player.Id,UnitType.Player)这里添加的UnitComponent
            unit.DomainScene().GetComponent<UnitComponent>().Remove(unit.Id);
        }
    }
}
