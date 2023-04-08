using RaFSM;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle
{
	public class SetTargetBattleUnitState : RaGOStateBase<BattleUnit, TargetBattleUnitData>
	{
		public RaGOStateEvent ChangedTargetEvent;
		public RaGOStateEvent NoChangeInTargetEvent;

		[SerializeField]
		private int _range = 1;

		[SerializeField]
		private BattlePlayerDetectionType _detectionType = BattlePlayerDetectionType.Opposite;

		[Header("References")]
		[SerializeField]
		[FormerlySerializedAs("_gridModelSO")]
		private BattleGridReferenceSO _gridReferenceSO = null;

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
				if(_gridReferenceSO.Grid.TryGetElement(targetPos, out GameGridElement targetElement))
				{
					if(_detectionType.HasFlag(BattlePlayerDetectionType.Same) &&
						targetElement.TryGetUnit(DependencyA.Owner.PlayerType, out unitToSet) &&
						unitToSet.IsActiveUnit)
					{
						break;
					}

					if(_detectionType.HasFlag(BattlePlayerDetectionType.Opposite) &&
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