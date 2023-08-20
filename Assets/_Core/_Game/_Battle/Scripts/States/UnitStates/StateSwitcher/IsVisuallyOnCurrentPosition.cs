using RaFSM;

namespace Game.Battle
{
	public class IsVisuallyOnCurrentPosition : StateSwitcherBase
	{
		[UnityEngine.SerializeField]
		private BattleGridReferenceSO _gridReferenceSO = null;

		protected override bool CheckCondition(RaGOStateBase state)
		{
			BattleUnit unit = state.GetDependency<BattleUnit>();
			return _gridReferenceSO.Grid.ToGridPos(unit.transform.position) == unit.Position;
		}
	}
}