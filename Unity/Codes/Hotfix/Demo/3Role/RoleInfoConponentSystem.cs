
namespace ET
{
    public class RoleInfoConponentDestroySystem:DestroySystem<RoleInfoComponent>
    {
        public override void Destroy(RoleInfoComponent self)
        {
            foreach (var ele in self.roleinfolist)
            {
                ele?.Dispose();
            }
            self.roleinfolist.Clear();
            self.CurrentRoleID = 0;
        }
    }
    [FriendClass(typeof(RoleInfoComponent))]
    public static class RoleInfoConponentSystem
    {

    }
}
