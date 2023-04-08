using RaFSM;

namespace Game.Battle
{
	public class HasTargetBattlePlayerSwitcher : StateSwitcherBase
	{
		protected override bool CheckCondition(RaGOStateBase state)
		{
			TargetBattlePlayerData targetPlayerData = state.GetDependency<TargetBattlePlayerData>();
			return targetPlayerData != null && targetPlayerData.CurrentTarget != null;
		}
	}
}