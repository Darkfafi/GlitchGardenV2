public class HasUnitsToPlayInGameCondition : InGameConditionBase
{
	protected override void OnStartRunning()
	{
		PlayerSide.Player.Wallet.ValueChangedEvent += OnWalletValueChangedEvent;
	}

	protected override void OnStopRunning()
	{
		PlayerSide.Player.Wallet.ValueChangedEvent -= OnWalletValueChangedEvent;
	}

	protected override bool CheckCondition() => PlayerSide.HasResourcesForUnits();

	private void OnWalletValueChangedEvent(CurrencyConfig currency, int newValue, int oldValue)
	{
		if(PlayerSide.ResourceGenerator.AmountToGenerate.Currency != currency)
		{
			return;
		}

		SetInGame(CheckCondition());
	}
}
