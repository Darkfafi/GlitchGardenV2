using RaFSM;

namespace Game.Battle
{
	public class BattleGameWinState : RaGOStateBase<BattleGame>
	{
		protected override void OnInit()
		{

		}

		protected override void OnPreSwitch()
		{
			// About to Enter, Register itself as user
			if(!IsCurrentState)
			{
				Dependency.GameUI.WinUIGroup.Users.Register(this);
			}
		}

		protected override void OnEnter()
		{

		}

		protected override void OnExit(bool isSwitch)
		{
			Dependency.GameUI.WinUIGroup.Users.Unregister(this);
		}

		protected override void OnDeinit()
		{

		}
	}
}