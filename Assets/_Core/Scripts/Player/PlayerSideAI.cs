using RaCollection;
using RaDataHolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSideAI : RaMonoDataHolderBase<PlayerSide>
{
	[SerializeField]
	private UnitsMechanicSO _unitsMechanicSO = null;

	public CurrencyConfig ResourceCurrency => Data.ResourceGenerator.AmountToGenerate.Currency;
	private UnitConfig _nextToSpawn = null;
	private Coroutine _routine = null;

	protected override void OnSetData()
	{

	}

	protected override void OnSetDataResolved()
	{
		_routine = StartCoroutine(SpawnRoutine());
	}

	protected override void OnClearData()
	{
		if(_routine != null)
		{
			StopCoroutine(_routine);
			_routine = null;
		}
	}

	private IEnumerator SpawnRoutine()
	{
		while(HasData)
		{
			yield return new WaitForSeconds(1f);

			if(_nextToSpawn == null)
			{
				SetNextToSpawn();
			}

			if(_nextToSpawn != null)
			{
				if(!Data.Player.Wallet.CanSpend(_nextToSpawn.Cost, out _))
				{
					continue;
				}

				Unit.CoreData data = new Unit.CoreData() { Owner = Data.Player, Config = _nextToSpawn };
				var spawns = _unitsMechanicSO.GetCreateUnitPositions(data);

				if(spawns.Count > 0)
				{
					UnitConfig unitToSpawn = _nextToSpawn;
					_nextToSpawn = null;

					if(_unitsMechanicSO.CreateUnit(data, spawns[Random.Range(0, spawns.Count)]).IsSuccess)
					{
						_nextToSpawn = null;
						yield return new WaitForSeconds(1f);
					}
					else
					{
						_nextToSpawn = unitToSpawn;
					}
				};
			}
		}
		_routine = null;
	}

	private void SetNextToSpawn()
	{
		int budget = Data.ResourceGenerator.GetBudget();
		List<UnitConfig> spawnableUnits = Data.Player.Model.Units.GetItems(x => budget >= x.Cost.Amount);
		_nextToSpawn = spawnableUnits.Count > 0 ? spawnableUnits[Random.Range(0, spawnableUnits.Count)] : null;
	}
}
