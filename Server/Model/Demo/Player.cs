namespace ET
{

    public enum PlayerState
    {
        Disconnect,
        Gate,
        Game,
    }

    public class playerawakesystem:AwakeSystem<Player,long,long>
    {
        public override void Awake(Player self, long a, long b)
        {
            self.AccountID = a;
            self.UnitId = b;
        }
    }


    public sealed class Player : Entity, IAwake<string>,IAwake<long,long>
	{
		public long AccountID { get; set; }
		
		public long UnitId { get; set; }

        public PlayerState PlayerState { get; set; }

        public long sessioninstanceid { get; set; }
    }
}