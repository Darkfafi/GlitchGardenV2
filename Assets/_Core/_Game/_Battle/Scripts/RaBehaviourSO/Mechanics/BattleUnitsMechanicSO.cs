using RaCollection;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle
{
	[CreateAssetMenu(menuName = "Behaviours/Mechanics/BattleUnitsMechanicSO")]
	public class BattleUnitsMechanicSO : BattleGameMechanicSOBase
	{
		private RaCollection<BattleUnit> _units;

		public IReadOnlyRaCollection<BattleUnit> Units => _units;

		#region Core

		protected override void OnSetup()
		{
			_units = new RaCollection<BattleUnit>();
			base.OnSetup();
		}

		protected override void OnStart()
		{

		}

		protected override void OnEnd()
		{

		}

		#endregion

		#region Creation

		public List<Vector2Int> GetCreateUnitPositions(BattleUnit.CoreData coreData, bool includeCost = true)
		{
			if(!IsEnabled)
			{
				return new List<Vector2Int>();
			}

			List<Vector2Int> spawns = new List<Vector2Int>();
			if(TryGetDependency(out BattleGridReferenceSO gridReference))
			{
				gridReference.Grid.ForEach((pos, element) =>
				{
					if(CanCreateUnit(coreData, pos, includeCost).IsSuccess)
					{
						spawns.Add(pos);
					}
				});
			}
			return spawns;
		}

		public MechanicResponse CanCreateUnit(BattleUnit.CoreData coreData, Vector2Int position, bool includeCost = true)
		{
			return CanCreateUnit(coreData, position, out _, includeCost);
		}

		public MechanicResponse CanCreateUnit(BattleUnit.CoreData coreData, Vector2Int position, out ElementBattleUnitSpot unitSpot, bool includeCost = true)
		{
			unitSpot = default;

			if(!IsEnabled)
			{
				return MechanicResponse.CreateFailedResponse("Mechanic Disabled", null);
			}

			if(!TryGetDependency(out BattleGridReferenceSO gridReference))
			{
				return MechanicResponse.CreateFailedResponse("No Grid Reference Found", null);
			}

			if(!gridReference.Grid.TryGetElement(position, out GameGridElement element))
			{
				return MechanicResponse.CreateFailedResponse($"No Element found at {position}", null);
			}

			if(!element.TryGetElementUnitSpot(coreData.Owner, out unitSpot))
			{
				return MechanicResponse.CreateFailedResponse($"No UnitSpot found for {coreData.Owner} on {element}", null);
			}

			if(element.IsOccupied(out BattleUnit homeUnit, out BattleUnit awayUnit))
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
					if(!coreData.Owner.Inventory.Wallet.CanSpend(coreData.BattleUnitData.Cost, out var notEnoughCurrencyData))
					{
						return MechanicResponse.CreateFailedResponse($"Not enough Resources to buy {coreData.BattleUnitData.Cost}", (nameof(notEnoughCurrencyData), notEnoughCurrencyData));
					}
				}

				if(coreData.BattleUnitData.FirstColumnUnit)
				{
					int playerColumn = gridReference.Grid.GetPlayerColumn(coreData.Owner.PlayerType);
					bool valid = position.x == playerColumn;

					if(!valid)
					{
						return MechanicResponse.CreateFailedResponse($"Unit {coreData.Config} requires to be placed on the first column of the owner {coreData.Owner}. Which is not {position}");
					}
				}
			}

			return MechanicResponse.CreateSuccessResponse();
		}

		public MechanicResponse CreateUnit(BattleUnit.CoreData coreData, Vector2Int position)
		{
			MechanicResponse response = CanCreateUnit(coreData, position, out ElementBattleUnitSpot unitSpot);
			if(response.IsSuccess)
			{
				if(coreData.Owner.Inventory.Wallet.Spend(coreData.BattleUnitData.Cost, out var notEnoughCurrencyData))
				{
					BattleUnit unit = Instantiate(coreData.BattleUnitData.BattleUnitPrefab);
					unit.SetData(coreData, false);
					{
						unit.SetPosition(position);

						unitSpot.SetPreview(null);
						unitSpot.SetUnit(unit);
					}

					_units.Add(unit);
					unit.Resolve();

					response = MechanicResponse.CreateSuccessResponse((nameof(unitSpot), unitSpot), (nameof(unit), unit));
				}
				else
				{
					response = MechanicResponse.CreateFailedResponse($"Not enough Resources to buy {coreData.BattleUnitData.Cost}", (nameof(notEnoughCurrencyData), notEnoughCurrencyData));
				}
			}

			return response;
		}

		#endregion

		#region Replacement

		public MechanicResponse CanReplaceUnit(BattleUnit unitToReplace, UnitModel newUnit)
		{
			return CanReplaceUnit(unitToReplace, newUnit, out _);
		}

		public MechanicResponse CanReplaceUnit(BattleUnit unitToReplace, UnitModel newUnit, out ElementBattleUnitSpot unitSpot)
		{
			unitSpot = default;
			Vector2Int position = unitToReplace.Position;

			if(!IsEnabled)
			{
				return MechanicResponse.CreateFailedResponse("Mechanic Disabled", null);
			}

			if(newUnit == null)
			{
				return MechanicResponse.CreateFailedResponse($"Unit to replace with is Null");
			}

			if(!TryGetDependency(out BattleGridReferenceSO gridReference))
			{
				return MechanicResponse.CreateFailedResponse("No Grid Reference Found", null);
			}

			if(!gridReference.Grid.TryGetElement(position, out GameGridElement element))
			{
				return MechanicResponse.CreateFailedResponse($"No Element found at {position}", null);
			}

			if(!element.TryGetElementUnitSpot(unitToReplace.Owner, out unitSpot))
			{
				return MechanicResponse.CreateFailedResponse($"No UnitSpot found for {unitToReplace.Owner} on {element}", null);
			}

			if(element.IsOccupied(out BattleUnit homeUnit, out BattleUnit awayUnit) && homeUnit != unitToReplace && awayUnit != unitToReplace)
			{
				return MechanicResponse.CreateFailedResponse($"Element {element} contains does not contain {unitToReplace}. Home Unit: {homeUnit} or Away Unit: {awayUnit}", (nameof(element), element), (nameof(homeUnit), homeUnit), (nameof(awayUnit), awayUnit));
			}

			return MechanicResponse.CreateSuccessResponse();
		}

		public MechanicResponse ReplaceUnit(BattleUnit unitToReplace, UnitModel newUnit)
		{
			MechanicResponse response = CanReplaceUnit(unitToReplace, newUnit, out ElementBattleUnitSpot unitSpot);
			if(response.IsSuccess)
			{
				Vector2Int position = unitToReplace.Position;
				BattlePlayer owner = unitToReplace.Owner;

				// Remove old Unit
				_units.Remove(unitToReplace);
				Destroy(unitToReplace.gameObject);

				// Create new Unit
				BattleUnit unit = Instantiate(newUnit.BattleUnitData.BattleUnitPrefab);
				unit.SetData(new BattleUnit.CoreData {  UnitModel = newUnit, Owner = owner }, false);
				{
					unit.SetPosition(position);

					unitSpot.SetPreview(null);
					unitSpot.SetUnit(unit);
				}

				_units.Add(unit);
				unit.Resolve();
			}

			return response;
		}

		public void ReplaceUnitHook(UnitReplacementMechanicParams instruction)
		{
			ReplaceUnit(instruction.UnitToReplace, instruction.CreateUnitModel());
		}

		#endregion

		#region Movement

		public MechanicResponse CanMoveUnit(BattleUnit unit, Vector2Int newPosition)
		{
			return CanMoveUnit(unit, newPosition, out _, out _);
		}

		public MechanicResponse CanMoveUnit(BattleUnit unit, Vector2Int newPosition, out ElementBattleUnitSpot oldSpot, out ElementBattleUnitSpot newSpot)
		{
			oldSpot = default;
			newSpot = default;

			if(!IsEnabled)
			{
				return MechanicResponse.CreateFailedResponse("Mechanic Disabled", null);
			}

			if(!TryGetDependency(out BattleGridReferenceSO gridReference))
			{
				return MechanicResponse.CreateFailedResponse("No Grid Reference Found", null);
			}

			if(!gridReference.Grid.TryGetElement(unit.Position, out GameGridElement oldElement))
			{
				return MechanicResponse.CreateFailedResponse($"No Element found at {unit.Position}", null);
			}

			if(!gridReference.Grid.TryGetElement(newPosition, out GameGridElement element))
			{
				return MechanicResponse.CreateFailedResponse($"No Element found at {newPosition}", null);
			}

			if(!oldElement.TryGetElementUnitSpot(unit.Owner, out oldSpot))
			{
				return MechanicResponse.CreateFailedResponse($"No UnitSpot found on {oldElement}", null);
			}

			if(!element.TryGetElementUnitSpot(unit.Owner, out newSpot))
			{
				return MechanicResponse.CreateFailedResponse($"No UnitSpot found on {element}", null);
			}

			if(element.IsOccupied(out BattleUnit homeUnit, out BattleUnit awayUnit))
			{
				return MechanicResponse.CreateFailedResponse($"Element {element} contains Unit. Home Unit: {homeUnit} or Away Unit: {awayUnit}", (nameof(element), element), (nameof(homeUnit), homeUnit), (nameof(awayUnit), awayUnit));
			}

			return MechanicResponse.CreateSuccessResponse();
		}

		public MechanicResponse MoveUnit(BattleUnit unit, Vector2Int newPosition, out ElementBattleUnitSpot newSpot)
		{
			MechanicResponse response = CanMoveUnit(unit, newPosition,
				out ElementBattleUnitSpot oldSpot,
				out newSpot);

			if(response.IsSuccess)
			{
				unit.SetPosition(newPosition);

				oldSpot.SetPreview(null);
				oldSpot.SetUnit(null);

				newSpot.SetPreview(null);
				newSpot.SetUnit(unit);

				return MechanicResponse.CreateSuccessResponse((nameof(oldSpot), oldSpot), (nameof(newSpot), newSpot), (nameof(unit), unit));
			}
			return response;
		}

		#endregion

		#region Destruction

		public MechanicResponse CanDestroyUnit(BattleUnit unit)
		{
			return CanDestroyUnit(unit, out _);
		}

		public MechanicResponse CanDestroyUnit(BattleUnit unit, out ElementBattleUnitSpot unitSpot)
		{
			unitSpot = default;

			if(!IsEnabled)
			{
				return MechanicResponse.CreateFailedResponse("Mechanic Disabled", null);
			}

			if(!TryGetDependency(out BattleGridReferenceSO gridReference))
			{
				return MechanicResponse.CreateFailedResponse("No Grid Reference Found", null);
			}

			if(!gridReference.Grid.TryGetElement(unit.Position, out GameGridElement element))
			{
				return MechanicResponse.CreateFailedResponse($"No Element found at {unit.Position}", null);
			}

			if(!element.TryGetElementUnitSpot(unit.Owner, out unitSpot))
			{
				return MechanicResponse.CreateFailedResponse($"No UnitSpot found on {element}", null);
			}

			if(unitSpot.Unit != unit)
			{
				return MechanicResponse.CreateFailedResponse($"Unit {unit} not found at {unitSpot}, but found {unitSpot.Unit} instead", null);
			}

			return MechanicResponse.CreateSuccessResponse();
		}

		public MechanicResponse DestroyUnit(BattleUnit unit)
		{
			MechanicResponse response = CanDestroyUnit(unit, out ElementBattleUnitSpot unitSpot);

			if(response.IsSuccess)
			{
				_units.Remove(unit);
				Destroy(unit.gameObject);
				unitSpot.SetUnit(null);
			}

			return response;
		}

		public void DestroyUnitHook(BattleUnit unit)
		{
			DestroyUnit(unit);
		}

		#endregion
	}
}