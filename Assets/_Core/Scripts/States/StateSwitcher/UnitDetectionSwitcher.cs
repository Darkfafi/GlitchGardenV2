using RaFSM;
using UnityEngine;

public class UnitDetectionSwitcher : StateSwitcherBase
{
	[SerializeField]
	private GridModelSO _gridModelSO = null;

	[SerializeField]
	private UnitOwnerDetectionType _detectionType = UnitOwnerDetectionType.Opposite;

	protected override bool CheckCondition(RaGOStateBase state)
	{
		Unit unit = state.GetDependency<Unit>();
		Vector2Int pos = unit.Position;
		pos += unit.Owner.GetOrientation(Vector2Int.right);

		bool condition = false;

		if(_gridModelSO.Grid.TryGetElement(pos, out GameGridElement targetElement))
		{
			if(_detectionType.HasFlag(UnitOwnerDetectionType.Same))
			{
				condition |= targetElement.TryGetUnit(unit.Owner.PlayerType, out _);
			}

			if(_detectionType.HasFlag(UnitOwnerDetectionType.Opposite))
			{
				condition |= targetElement.TryGetUnit(unit.Owner.GetOppositePlayerType(), out _);
			}
		}

		return condition;
	}
}
