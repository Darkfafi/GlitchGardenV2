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

		if(_gridModelSO.Grid.TryGetElement(pos, out GameGridElement targetElement) && 
			targetElement.IsOccupied(out Unit homeUnit, out Unit awayUnit))
		{
			if(_detectionType.HasFlag(UnitOwnerDetectionType.Same))
			{
				condition |= homeUnit != null && homeUnit.Owner == unit.Owner;
				condition |= awayUnit != null && awayUnit.Owner == unit.Owner;
			}

			if(_detectionType.HasFlag(UnitOwnerDetectionType.Opposite))
			{
				condition |= homeUnit != null && homeUnit.Owner != unit.Owner;
				condition |= awayUnit != null && awayUnit.Owner != unit.Owner;
			}
		}

		return condition;
	}

	[System.Flags]
	public enum UnitOwnerDetectionType
	{
		Same = 1,
		Opposite = 2
	}
}