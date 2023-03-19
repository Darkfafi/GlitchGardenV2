using RaDataHolder;
using Game.Battle.UI;
using UnityEngine;
using UI;

namespace Game.Battle
{
	public class BattleGameUI : RaMonoDataHolderBase<BattleGame>
	{
		[field: SerializeField]
		public BattleGameplayUIGroup GameplayUIGroup
		{
			get; private set;
		}

		[field: SerializeField]
		public GenericUIGroup WinUIGroup
		{
			get; private set;
		}

		[field: SerializeField]
		public GenericUIGroup LoseUIGroup
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			GameplayUIGroup.UnitsHUD.SetData(Data.HomePlayerSide.Player, false);
			GameplayUIGroup.ResourcesHUD.SetData(Data.HomePlayerSide.Player.Wallet, false);
		}

		protected override void OnClearData()
		{
			GameplayUIGroup.ResourcesHUD.ClearData();
			GameplayUIGroup.UnitsHUD.ClearData();
		}

		protected override void OnSetDataResolved()
		{
			GameplayUIGroup.UnitsHUD.Resolve();
			GameplayUIGroup.ResourcesHUD.Resolve();
		}
	}
}