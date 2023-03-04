using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/Mechanics/UnitsMechanicSO")]
public class UnitsMechanicSO : GameMechanicSOBase
{
	#region Core

	protected override void OnSetup()
	{

	}

	protected override void OnStart()
	{

	}

	protected override void OnEnd()
	{

	}

	#endregion

	#region Creation

	public bool CanCreateUnit(Unit.CoreData coreData, Vector2Int position)
	{
		return CanCreateUnit(coreData, position, out _);
	}

	public bool CanCreateUnit(Unit.CoreData coreData, Vector2Int position, out ElementUnitSpot unitSpot)
	{
		if
		(
			TryGetDependency(out GridModelSO gridModelSO) &&
			// Has Element
			gridModelSO.Grid.TryGetElement(position, out GameGridElement element) &&
			// Has Unit Spot for Owner
			element.TryGetElementUnitSpot(coreData.Owner, out unitSpot) &&
			// Is Buildable Spot
			unitSpot.SpotData.IsBuildable &&
			// Does not contain a Unit on Owner Spot
			unitSpot.Unit == null
		)
		{
			return true;
		}

		unitSpot = default;
		return false;
	}

	public bool CreateUnit(Unit.CoreData coreData, Vector2Int position)
	{
		if(CanCreateUnit(coreData, position, out ElementUnitSpot unitSpot))
		{
			Unit unit = Instantiate(coreData.Config.UnitPrefab);
			unit.SetData(coreData, false);
			unit.SetPosition(position);

			unitSpot.SetPreview(null);
			unitSpot.SetUnit(unit);

			unit.Resolve();
			return true;
		}
		return false;
	}

	#endregion

	#region Movement
	public bool CanMoveUnit(Unit unit, Vector2Int newPosition)
	{
		return CanMoveUnit(unit, newPosition, out _, out _);
	}

	public bool CanMoveUnit(Unit unit, Vector2Int newPosition, out ElementUnitSpot oldSpot, out ElementUnitSpot newSpot)
	{
		if(
			TryGetDependency(out GridModelSO gridModelSO) &&
			// Has Old & New Element on Grid
			gridModelSO.Grid.TryGetElement(unit.Position, out GameGridElement oldElement) &&
			gridModelSO.Grid.TryGetElement(newPosition, out GameGridElement newElement) &&
			// Can find appropriate Unit Spots on Old & New Element
			oldElement.TryGetElementUnitSpot(unit.Owner, out oldSpot) &&
			newElement.TryGetElementUnitSpot(unit.Owner, out newSpot) &&
			// Has No Unit on new spot
			newSpot.Unit != null)
		{
			return true;
		}

		oldSpot = default;
		newSpot = default;
		return false;
	}

	public bool MoveUnit(Unit unit, Vector2Int newPosition)
	{
		if(CanMoveUnit(unit, newPosition,
			out ElementUnitSpot oldSpot,
			out ElementUnitSpot newSpot))
		{
			unit.SetPosition(newPosition);
			
			oldSpot.SetPreview(null);
			oldSpot.SetUnit(null);

			newSpot.SetPreview(null);
			newSpot.SetUnit(unit);

			unit.transform.position = newSpot.GetUnitLocation();

			return true;
		}
		return false;
	}

	#endregion

	#region Destruction

	public bool CanDestroyUnit(Unit unit)
	{
		return CanDestroyUnit(unit, out _);
	}

	public bool CanDestroyUnit(Unit unit, out ElementUnitSpot unitSpot)
	{
		if
		(
			TryGetDependency(out GridModelSO gridModelSO) &&
			// Has Element
			gridModelSO.Grid.TryGetElement(unit.Position, out GameGridElement element) &&
			// Has Unit Spot for Owner
			element.TryGetElementUnitSpot(unit.Owner, out unitSpot) &&
			// Spot Contains Unit
			unitSpot.Unit == unit
		)
		{
			return true;
		}

		unitSpot = default;
		return false;
	}

	public bool DestroyUnit(Unit unit)
	{
		if(CanDestroyUnit(unit, out ElementUnitSpot unitSpot))
		{
			Destroy(unit);
			unitSpot.SetUnit(null);
			return true;
		}
		return false;
	}

	#endregion
}