using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.PlayerLoop;

namespace ET
{
	[FriendClass(typeof(DlgRoleInfo))]
	public static  class DlgRoleInfoSystem
	{
		public class DlgRoleInfoUpdateSystem:UpdateSystem<DlgRoleInfo>
		{
			public override void Update(DlgRoleInfo self)
			{
				if(Input.GetKeyDown(KeyCode.P))
				{

				}
			}
		}


        public static async void RegisterUIEvent(this DlgRoleInfo self)
		{
			await self.Refresh();

        }

		public static void ShowWindow(this DlgRoleInfo self, Entity contextData = null)
		{
	
		}

        public static async ETTask Refresh(this DlgRoleInfo self)
		{
			Unit unit = UnitHelper.GetMyUnitFromCurrentScene(self.ZoneScene().CurrentScene());
			NumericComponent numericComponent = unit.GetComponent<NumericComponent>();

			self.View.E_GoldText.SetText(numericComponent.GetAsInt((int)NumericType.Speed).ToString());
			await ETTask.CompletedTask;
		}

    }
}
