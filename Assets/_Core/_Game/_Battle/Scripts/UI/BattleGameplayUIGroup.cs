using UI;
using UnityEngine;

namespace Game.Battle.UI
{
	public class BattleGameplayUIGroup : GenericUIGroup
	{
		[field: SerializeField]
		public BattleUnitsHUD UnitsHUD
		{
			get; private set;
		}

		[field: SerializeField]
		public WalletCurrencyUIElement ResourcesHUD
		{
			get; private set;
		}

		public void SetData(BattlePlayer player)
		{
			UnitsHUD.SetData(player, false);
			ResourcesHUD.SetData(player.Wallet, false);
		}

		public void ResolveData()
		{
			UnitsHUD.Resolve();
			ResourcesHUD.Resolve();
		}

		public void ClearData()
		{
			ResourcesHUD.ClearData();
			UnitsHUD.ClearData();
		}

		protected override void OnUseStarted()
		{
			base.OnUseStarted();
			UnitsHUD.SetInteractable(true);
		}

		protected override void OnUseEnded()
		{
			UnitsHUD.SetInteractable(false);
			base.OnUseEnded();
		}
	}
}