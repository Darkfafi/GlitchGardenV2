using RaFSM;
using UnityEngine;

public class SetTargetUnitState : RaGOStateBase<Unit, TargetUnitData>
{
	public StateEvent ChangedTargetEvent;
	public StateEvent NoChangeInTargetEvent;

	[SerializeField]
	private int _range = 1;

	[SerializeField]
	private UnitOwnerDetectionType _detectionType = UnitOwnerDetectionType.Opposite;

	[SerializeField]
	private GridModelSO _gridModelSO = null;

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

	private Unit RefreshTarget()
	{
		Unit unitToSet = null;
		for(int i = 1; i <= _range; i++)
		{
			Vector2Int targetPos = DependencyA.Position + DependencyA.Owner.GetOrientation(Vector2Int.right * i);
			if(_gridModelSO.Grid.TryGetElement(targetPos, out GameGridElement targetElement))
			{
				if(_detectionType.HasFlag(UnitOwnerDetectionType.Same) && 
					targetElement.TryGetUnit(DependencyA.Owner.PlayerType, out unitToSet))
				{
					break;
				}

				if(_detectionType.HasFlag(UnitOwnerDetectionType.Opposite) && 
					targetElement.TryGetUnit(DependencyA.Owner.GetOppositePlayerType(), out unitToSet))
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
