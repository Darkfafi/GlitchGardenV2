using RaDataHolder;
using UI;
using UnityEngine;

public class GameUI : RaMonoDataHolderBase<Game>
{
	[field: SerializeField]
	public GameplayUIGroup GameplayUIGroup
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