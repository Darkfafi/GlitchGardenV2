using RaFSM;

namespace Game.Battle
{
	public class BattleGameLoseState : RaGOStateBase<BattleGame>
	{
		protected override void OnInit()
		{

		}

		protected override void OnPreSwitch()
		{
			// About to Enter, Register itself as user
			if(!IsCurrentState)
			{
				Dependency.GameUI.LoseUIGroup.RegisterUser(this);
			}
		}

		protected override void OnEnter()
		{

		}

		protected override void OnExit(bool isSwitch)
		{
			Dependency.GameUI.LoseUIGroup.UnregisterUser(this);
		}

		protected override void OnDeinit()
		{

		}
	}
}