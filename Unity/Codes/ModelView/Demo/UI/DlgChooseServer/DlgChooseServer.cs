namespace ET
{
	 [ComponentOf(typeof(UIBaseWindow))]
	public  class DlgChooseServer :Entity,IAwake,IUILogic
	{

		public DlgChooseServerViewComponent View { get => this.Parent.GetComponent<DlgChooseServerViewComponent>();} 

		 

	}
}
