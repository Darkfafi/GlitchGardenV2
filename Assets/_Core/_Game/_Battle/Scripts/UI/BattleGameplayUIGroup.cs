using UI;
using UnityEngine;

namespace Game.Battle.UI
{
	public class BattleGameplayUIGroup : GenericUIGroup<BattlePlayer>
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

		[field: SerializeField]
		public HealthIconsDisplay PlayerHealthHUD
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			UnitsHUD.SetData(Data, false);
			ResourcesHUD.SetData(Data.Wallet, false);
			PlayerHealthHUD.SetData(Data.Health, false);
		}

		protected override void OnResolveData()
		{
			UnitsHUD.Resolve();
			ResourcesHUD.Resolve();
			PlayerHealthHUD.Resolve();
		}

		protected override void OnClearData()
		{
			PlayerHealthHUD.ClearData();
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