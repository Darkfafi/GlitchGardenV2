using RaFSM;

namespace Game.Battle
{
	public class HasTargetBattleUnitSwticher : StateSwitcherBase
	{
		protected override bool CheckCondition(RaGOStateBase state)
		{
			TargetBattleUnitData targetUnitData = state.GetDependency<TargetBattleUnitData>();
			return targetUnitData != null && targetUnitData.CurrentTarget != null;
		}
	}
}