﻿namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgRoleInfo :Entity,IAwake,IUILogic,IUpdate
	{

		public DlgRoleInfoViewComponent View { get => this.Parent.GetComponent<DlgRoleInfoViewComponent>();} 

		 

	}
}
