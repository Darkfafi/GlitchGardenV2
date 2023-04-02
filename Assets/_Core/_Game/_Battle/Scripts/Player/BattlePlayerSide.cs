using RaCollection;
using RaDataHolder;
using RaFlags;
using UnityEngine;
using static Game.Battle.BattlePlayer;

namespace Game.Battle
{
	public class BattlePlayerSide : RaMonoDataHolderBase<PlayerModel>
	{
		[field: SerializeField]
		public Type SideType
		{
			get; private set;
		}

		[field: SerializeField]
		public BattlePlayer Player
		{
			get; private set;
		}

		[field: SerializeField]
		public CurrencyGenerator ResourceGenerator
		{
			get; private set;
		}

		public Health Health
		{
			get; private set;
		}

		public RaFlagsTracker InGameFlags
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			InGameFlags = new RaFlagsTracker();

			Player.SetData(new CoreData() { PlayerModel = Data, Type = SideType }, false);
			ResourceGenerator.SetData(Player.Wallet, false);

			if(Player.Model.Config.HP > 0)
			{
				Health = new Health(Player.Model.Config.HP);
			}
			else
			{
				Health = null;
			}
		}

		protected override void OnSetDataResolved()
		{
			Player.Resolve();
			ResourceGenerator.Resolve();
		}

		protected override void OnClearData()
		{
			ResourceGenerator.ClearData();
			Player.ClearData();

			if(InGameFlags != null)
			{
				InGameFlags.Dispose();
				InGameFlags = null;
			}

			Health = null;
		}

		public bool HasResourcesForUnits()
		{
			int budget = ResourceGenerator.GetBudget();
			return Player.Units.GetItems(x => budget >= x.BattleUnitConfigData.Cost.Amount).Count > 0;
		}
	}
}