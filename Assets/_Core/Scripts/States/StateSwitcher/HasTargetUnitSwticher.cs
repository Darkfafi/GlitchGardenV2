using RaFSM;

public class HasTargetUnitSwticher : StateSwitcherBase
{
	protected override bool CheckCondition(RaGOStateBase state)
	{
		TargetUnitData targetUnitData = state.GetDependency<TargetUnitData>();
		return targetUnitData != null && targetUnitData.CurrentTarget != null;
	}
}
