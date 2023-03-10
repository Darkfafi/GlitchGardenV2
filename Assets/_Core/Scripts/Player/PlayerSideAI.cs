using RaCollection;
using RaDataHolder;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideAI : RaMonoDataHolderBase<PlayerSide>
{
	[SerializeField]
	private UnitsMechanicSO _unitsMechanicSO = null;

	[SerializeField]
	private GridModelSO _gridModelSO = null;

	public CurrencyConfig ResourceCurrency => Data.ResourceGenerator.AmountToGenerate.Currency;
	private UnitConfig _nextToSpawn = null;

	protected override void OnSetData()
	{

	}

	protected override void OnSetDataResolved()
	{
		Data.Player.Wallet.ValueChangedEvent += OnWalletValueChangedEvent;
		TrySpawnNextUnit();
	}

	protected override void OnClearData()
	{
		Data.Player.Wallet.ValueChangedEvent -= OnWalletValueChangedEvent;
	}

	private void OnWalletValueChangedEvent(CurrencyConfig currency, int newValue, int oldValue)
	{
		if(currency != ResourceCurrency)
		{
			return;
		}

		TrySpawnNextUnit();
	}

	private bool TrySpawnNextUnit()
	{
		if(_nextToSpawn == null)
		{
			SetNextToSpawn();
		}

		if(_nextToSpawn != null)
		{
			if(!_nextToSpawn.Cost.HasAmount(Data.Player.Wallet.GetAmount(ResourceCurrency)))
			{
				return false;
			}

			Unit.CoreData data = new Unit.CoreData() { Owner = Data.Player, Config = _nextToSpawn };
			var spawns = _unitsMechanicSO.GetCreateUnitPositions(data);

			if(spawns.Count > 0)
			{
				UnitConfig unitToSpawn = _nextToSpawn;
				_nextToSpawn = null;

				if(_unitsMechanicSO.CreateUnit(data, spawns[Random.Range(0, spawns.Count)]).IsSuccess)
				{
					SetNextToSpawn();
					return true;
				}
				else
				{
					_nextToSpawn = unitToSpawn;
				}
			};
		}

		return false;
	}

	private void SetNextToSpawn()
	{
		int budget = GetBudget();
		List<UnitConfig> spawnableUnits = Data.Player.Model.Units.GetItems(x => x.Cost.HasAmount(budget));
		_nextToSpawn = spawnableUnits.Count > 0 ? spawnableUnits[UnityEngine.Random.Range(0, spawnableUnits.Count)] : null;
	}

	private int GetBudget()
	{
		int budget = Data.Player.Wallet.GetAmount(Data.ResourceGenerator.AmountToGenerate.Currency);

		if(Data.ResourceGenerator.HasMaxResourcesEnabled)
		{
			budget += Data.ResourceGenerator.ResourcesRemaining;
		}
		else
		{
			budget = int.MaxValue;
		}

		return budget;
	}
}
