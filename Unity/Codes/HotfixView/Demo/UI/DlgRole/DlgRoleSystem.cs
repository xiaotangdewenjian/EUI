using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace ET
{
	[FriendClass(typeof(DlgRole))]
	[FriendClass(typeof(RoleInfoComponent))]
	public static  class DlgRoleSystem
	{

		public static void RegisterUIEvent(this DlgRole self)
		{
			self.View.ECreateRoleButton.onClick.AddListener(self.CreateRole);
			self.View.EEnterGameButton.onClick.AddListener(self.EnterGame);
		}

		public static void ShowWindow(this DlgRole self, Entity contextData = null)
		{
		}


        public static void CreateRole(this DlgRole self)
		{
			LoginHelper.CreateRole(self.DomainScene(), "神龙大侠").Coroutine();
		}

        public static async void EnterGame(this DlgRole self)
		{
			LoginHelper.GetRole(self.DomainScene()).Coroutine();

			self.DomainScene().GetComponent<RoleInfoComponent>().CurrentRoleID = self.DomainScene().GetComponent<RoleInfoComponent>().roleinfolist[0].Id;

			await TimerComponent.Instance.WaitAsync(1000);
			
			LoginHelper.GetReamKey(self.DomainScene()).Coroutine();

            await TimerComponent.Instance.WaitAsync(1000);

            LoginHelper.EnterGame(self.DomainScene()).Coroutine();

            await TimerComponent.Instance.WaitAsync(1000);

        }

    }
}
