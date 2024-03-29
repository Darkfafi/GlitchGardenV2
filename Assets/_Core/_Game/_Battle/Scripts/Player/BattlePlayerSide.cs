﻿using RaCollection;
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

		public RaFlagsTracker InGameFlags
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			InGameFlags = new RaFlagsTracker();

			Player.SetData(new CoreData() { PlayerModel = Data, Type = SideType }, false);
			ResourceGenerator.SetData(new CurrencyGenerator.CoreData() { Wallet = Player.Inventory.Wallet, MaxResources = Player.Model.ConfigData.Resources }, false);
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
		}

		public bool HasResourcesForUnits()
		{
			int budget = ResourceGenerator.GetBudget();
			return Player.GetUnits(x => budget >= x.BattleUnitData.Cost.Amount).Count > 0;
		}
	}
}