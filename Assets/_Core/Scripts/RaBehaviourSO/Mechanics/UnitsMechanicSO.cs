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

	public MechanicResponse CanCreateUnit(Unit.CoreData coreData, Vector2Int position)
	{
		if(!TryGetDependency(out GridModelSO gridModel))
		{
			return MechanicResponse.CreateFailedResponse("No GridModel Found", null);
		}

		if(!gridModel.Grid.TryGetElement(position, out GameGridElement element))
		{
			return MechanicResponse.CreateFailedResponse($"No Element found at {position}", null);
		}

		if(!element.TryGetElementUnitSpot(coreData.Owner, out var unitSpot))
		{
			return MechanicResponse.CreateFailedResponse($"No UnitSpot found for {coreData.Owner} on {element}", null);
		}

		if(!unitSpot.SpotData.IsBuildable)
		{
			return MechanicResponse.CreateFailedResponse($"UnitSpot {unitSpot} is not Buildable", (nameof(unitSpot), unitSpot));
		}

		if(unitSpot.Unit != null)
		{
			return MechanicResponse.CreateFailedResponse($"UnitSpot {unitSpot} contains Unit {unitSpot.Unit}", (nameof(unitSpot), unitSpot));
		}

		if(coreData.Config != null && !coreData.Owner.Wallet.CanSpend(coreData.Config.Cost, out var notEnoughCurrencyData))
		{
			return MechanicResponse.CreateFailedResponse($"Not enough Resources to buy {coreData.Config.Cost}", (nameof(notEnoughCurrencyData), notEnoughCurrencyData));
		}

		return MechanicResponse.CreateSuccessResponse((nameof(unitSpot), unitSpot));
	}

	public MechanicResponse CreateUnit(Unit.CoreData coreData, Vector2Int position)
	{
		MechanicResponse response = CanCreateUnit(coreData, position);
		if(response.IsSuccess && response.Locator.TryGetValue(out ElementUnitSpot unitSpot))
		{
			if(coreData.Owner.Wallet.Spend(coreData.Config.Cost, out var notEnoughCurrencyData))
			{
				Unit unit = Instantiate(coreData.Config.UnitPrefab);
				unit.SetData(coreData, false);
				{
					unit.SetPosition(position);

					unitSpot.SetPreview(null);
					unitSpot.SetUnit(unit);
				}
				unit.Resolve();
				response = MechanicResponse.CreateSuccessResponse((nameof(unitSpot), unitSpot), (nameof(unit), unit)); 
			}
			else
			{
				response = MechanicResponse.CreateFailedResponse($"Not enough Resources to buy {coreData.Config.Cost}", (nameof(notEnoughCurrencyData), notEnoughCurrencyData));
			}
		}

		return response;
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