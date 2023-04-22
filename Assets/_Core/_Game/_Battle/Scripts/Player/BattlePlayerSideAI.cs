using RaCollection;
using RaDataHolder;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle
{
	public class BattlePlayerSideAI : RaMonoDataHolderBase<BattlePlayerSide>
	{
		[SerializeField]
		private BattleUnitsMechanicSO _unitsMechanicSO = null;

		public CurrencyConfig ResourceCurrency => Data.ResourceGenerator.AmountToGenerate.Currency;
		private UnitModel _nextToSpawn = null;
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
					if(!Data.Player.Inventory.Wallet.CanSpend(_nextToSpawn.BattleUnitData.Cost, out _))
					{
						continue;
					}

					BattleUnit.CoreData data = new BattleUnit.CoreData() { Owner = Data.Player, UnitModel = _nextToSpawn };
					var spawns = _unitsMechanicSO.GetCreateUnitPositions(data);

					if(spawns.Count > 0)
					{
						UnitModel unitToSpawn = _nextToSpawn;
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
			List<UnitModel> spawnableUnits = Data.Player.GetUnits(x => budget >= x.BattleUnitData.Cost.Amount);
			_nextToSpawn = spawnableUnits.Count > 0 ? spawnableUnits[Random.Range(0, spawnableUnits.Count)] : null;
		}
	}
}