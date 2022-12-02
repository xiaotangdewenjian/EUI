namespace ET
{
    public class SceneChangeFinishEventAsyncCreateUIHelp : AEventAsync<EventType.SceneChangeFinish>
    {
        protected override async ETTask Run(EventType.SceneChangeFinish args)
        {
            //UIHelper.Create(args.CurrentScene, UIType.UIHelp, UILayer.Mid).Coroutine();
            args.ZoneScene.GetComponent<UIComponent>().CloseWindow(WindowID.WindowID_Role);
            await ETTask.CompletedTask;
        }
    }
}
