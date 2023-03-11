using System.Collections.Generic;
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

	public List<Vector2Int> GetCreateUnitPositions(Unit.CoreData coreData, bool includeCost = true)
	{
		List<Vector2Int> spawns = new List<Vector2Int>();
		if(TryGetDependency(out GridModelSO gridModel))
		{
			gridModel.Grid.ForEach((pos, element) => 
			{
				if(CanCreateUnit(coreData, pos, includeCost).IsSuccess)
				{
					spawns.Add(pos);
				}
			});
		}
		return spawns;
	}

	public MechanicResponse CanCreateUnit(Unit.CoreData coreData, Vector2Int position, bool includeCost = true)
	{
		return CanCreateUnit(coreData, position, out _, includeCost);
	}

	public MechanicResponse CanCreateUnit(Unit.CoreData coreData, Vector2Int position, out ElementUnitSpot unitSpot, bool includeCost = true)
	{
		unitSpot = default;
		if(!TryGetDependency(out GridModelSO gridModel))
		{
			return MechanicResponse.CreateFailedResponse("No GridModel Found", null);
		}

		if(!gridModel.Grid.TryGetElement(position, out GameGridElement element))
		{
			return MechanicResponse.CreateFailedResponse($"No Element found at {position}", null);
		}

		if(!element.TryGetElementUnitSpot(coreData.Owner, out unitSpot))
		{
			return MechanicResponse.CreateFailedResponse($"No UnitSpot found for {coreData.Owner} on {element}", null);
		}

		if(element.IsOccupied(out Unit homeUnit, out Unit awayUnit))
		{
			return MechanicResponse.CreateFailedResponse($"Element {element} contains Unit. Home Unit: {homeUnit} or Away Unit: {awayUnit}", (nameof(element), element), (nameof(homeUnit), homeUnit), (nameof(awayUnit), awayUnit));
		}

		if(!unitSpot.SpotData.IsBuildable)
		{
			return MechanicResponse.CreateFailedResponse($"UnitSpot {unitSpot} is not Buildable", (nameof(unitSpot), unitSpot));
		}

		if(coreData.Config != null)
		{
			if(includeCost)
			{
				if(!coreData.Owner.Wallet.CanSpend(coreData.Config.Cost, out var notEnoughCurrencyData))
				{
					return MechanicResponse.CreateFailedResponse($"Not enough Resources to buy {coreData.Config.Cost}", (nameof(notEnoughCurrencyData), notEnoughCurrencyData));
				}
			}

			if(coreData.Config.FirstColumnUnit)
			{
				bool valid = true;
				switch(coreData.Owner.PlayerType)
				{
					case Player.Type.Home:
						if(position.x != 0)
						{
							valid = false;
						}
						break;
					case Player.Type.Away:
						if(position.x != gridModel.Grid.GridData.Size.x - 1)
						{
							valid = false;
						}
						break;
				}

				if(!valid)
				{
					return MechanicResponse.CreateFailedResponse($"Unit {coreData.Config} requires to be placed on the first column of the owner {coreData.Owner}. Which is not {position}");
				}
			}
		}

		return MechanicResponse.CreateSuccessResponse();
	}

	public MechanicResponse CreateUnit(Unit.CoreData coreData, Vector2Int position)
	{
		MechanicResponse response = CanCreateUnit(coreData, position, out ElementUnitSpot unitSpot);
		if(response.IsSuccess)
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
	public MechanicResponse CanMoveUnit(Unit unit, Vector2Int newPosition)
	{
		return CanMoveUnit(unit, newPosition, out _, out _);
	}

	public MechanicResponse CanMoveUnit(Unit unit, Vector2Int newPosition, out ElementUnitSpot oldSpot, out ElementUnitSpot newSpot)
	{
		oldSpot = default;
		newSpot = default;

		if(!TryGetDependency(out GridModelSO gridModel))
		{
			return MechanicResponse.CreateFailedResponse("No GridModel Found", null);
		}

		if(!gridModel.Grid.TryGetElement(unit.Position, out GameGridElement oldElement))
		{
			return MechanicResponse.CreateFailedResponse($"No Element found at {unit.Position}", null);
		}

		if(!gridModel.Grid.TryGetElement(newPosition, out GameGridElement element))
		{
			return MechanicResponse.CreateFailedResponse($"No Element found at {newPosition}", null);
		}

		if(!oldElement.TryGetElementUnitSpot(unit.Owner, out oldSpot))
		{
			return MechanicResponse.CreateFailedResponse($"No Element found at {newPosition}", null);
		}

		if(!element.TryGetElementUnitSpot(unit.Owner, out newSpot))
		{
			return MechanicResponse.CreateFailedResponse($"No Element found at {newPosition}", null);
		}

		if(element.IsOccupied(out Unit homeUnit, out Unit awayUnit))
		{
			return MechanicResponse.CreateFailedResponse($"Element {element} contains Unit. Home Unit: {homeUnit} or Away Unit: {awayUnit}", (nameof(element), element), (nameof(homeUnit), homeUnit), (nameof(awayUnit), awayUnit));
		}

		return MechanicResponse.CreateSuccessResponse();
	}

	public MechanicResponse MoveUnit(Unit unit, Vector2Int newPosition)
	{
		MechanicResponse response = CanMoveUnit(unit, newPosition,
			out ElementUnitSpot oldSpot,
			out ElementUnitSpot newSpot);

		if(response.IsSuccess)
		{
			unit.SetPosition(newPosition);
			
			oldSpot.SetPreview(null);
			oldSpot.SetUnit(null);

			newSpot.SetPreview(null);
			newSpot.SetUnit(unit);

			unit.transform.position = newSpot.GetUnitLocation();

			return MechanicResponse.CreateSuccessResponse((nameof(oldSpot), oldSpot), (nameof(newSpot), newSpot), (nameof(unit), unit));
		}
		return response;
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