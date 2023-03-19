using RaFSM;
using UnityEngine;

namespace Game.Battle
{
	public class SetTargetBattleUnitState : RaGOStateBase<BattleUnit, TargetBattleUnitData>
	{
		public StateEvent ChangedTargetEvent;
		public StateEvent NoChangeInTargetEvent;

		[SerializeField]
		private int _range = 1;

		[SerializeField]
		private BattleUnitOwnerDetectionType _detectionType = BattleUnitOwnerDetectionType.Opposite;

		[SerializeField]
		private BattleGridModelSO _gridModelSO = null;

		protected override void OnInit()
		{

		}

		protected override void OnEnter()
		{
			RefreshTarget();
		}

		protected override void OnExit(bool isSwitch)
		{

		}

		protected override void OnDeinit()
		{

		}

		private BattleUnit RefreshTarget()
		{
			BattleUnit unitToSet = null;
			for(int i = 1; i <= _range; i++)
			{
				Vector2Int targetPos = DependencyA.Position + DependencyA.Owner.GetOrientation(Vector2Int.right * i);
				if(_gridModelSO.Grid.TryGetElement(targetPos, out GameGridElement targetElement))
				{
					if(_detectionType.HasFlag(BattleUnitOwnerDetectionType.Same) &&
						targetElement.TryGetUnit(DependencyA.Owner.PlayerType, out unitToSet) &&
						unitToSet.IsActiveUnit)
					{
						break;
					}

					if(_detectionType.HasFlag(BattleUnitOwnerDetectionType.Opposite) &&
						targetElement.TryGetUnit(DependencyA.Owner.GetOppositePlayerType(), out unitToSet) &&
						unitToSet.IsActiveUnit)
					{
						break;
					}
				}
			}

			if(DependencyB.SetTarget(unitToSet))
			{
				ChangedTargetEvent.Invoke(this);
			}
			else
			{
				NoChangeInTargetEvent.Invoke(this);
			}

			return unitToSet;
		}
	}
}