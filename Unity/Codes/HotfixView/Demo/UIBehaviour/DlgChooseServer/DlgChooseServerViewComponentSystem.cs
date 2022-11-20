
using UnityEngine;
using UnityEngine.UI;
namespace ET
{
	[ObjectSystem]
	public class DlgChooseServerViewComponentAwakeSystem : AwakeSystem<DlgChooseServerViewComponent> 
	{
		public override void Awake(DlgChooseServerViewComponent self)
		{
			self.uiTransform = self.GetParent<UIBaseWindow>().uiTransform;
		}
	}


	[ObjectSystem]
	public class DlgChooseServerViewComponentDestroySystem : DestroySystem<DlgChooseServerViewComponent> 
	{
		public override void Destroy(DlgChooseServerViewComponent self)
		{
			self.DestroyWidget();
		}
	}
}
