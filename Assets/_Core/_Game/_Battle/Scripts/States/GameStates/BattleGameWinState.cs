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
				Dependency.GameUI.WinUIGroup.RegisterUser(this);
			}
		}

		protected override void OnEnter()
		{

		}

		protected override void OnExit(bool isSwitch)
		{
			Dependency.GameUI.WinUIGroup.UnregisterUser(this);
		}

		protected override void OnDeinit()
		{

		}
	}
}