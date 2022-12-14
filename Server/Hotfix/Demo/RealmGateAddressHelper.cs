using System.Collections.Generic;


namespace ET
{
	public static class RealmGateAddressHelper
	{
		public static StartSceneConfig GetGate(int zone,long accountid)
		{
			List<StartSceneConfig> zoneGates = StartSceneConfigCategory.Instance.Gates[zone];
			
			int n = accountid.GetHashCode() % zoneGates.Count;

			return zoneGates[n];
		}
        public static StartSceneConfig GetRealm(int zone)
        {
            StartSceneConfig zoneRealm = StartSceneConfigCategory.Instance.Realms[zone];
            return zoneRealm;
        }
    }
}
