using RaDataHolder;
using UI;
using UnityEngine;

public class GameUI : RaMonoDataHolderBase<Game>
{
	[field: SerializeField]
	public UnitsHUD UnitsHUD
	{
		get; private set;
	}

	[field: SerializeField]
	public WalletCurrencyUIElement ResourcesHUD
	{
		get; private set;
	}

	protected override void OnSetData()
	{
		UnitsHUD.SetData(Data.HomePlayer, false);
		ResourcesHUD.SetData(Data.HomePlayer.Wallet, false);
	}

	protected override void OnClearData()
	{
		ResourcesHUD.ClearData();
		UnitsHUD.ClearData();
	}

	protected override void OnSetDataResolved()
	{
		UnitsHUD.Resolve();
		ResourcesHUD.Resolve();
	}
}

// Home Player
/*
 * Has Resources to pay for units
 * Gains Resources over time
 * Can place units from its list on the Buildable Tiles assigned to him
 * Has assigned column to protect from enemy units
 */

// Away Player
/*
 * Has Resources to pay for units
 * Gains Resources over time
 * Can place units from its list on the Buildable Tiles assigned to him
	* Will place units on first row, except for spiders and cacti
 * Wishes to push enemy units to assined row of Home Player
 * IDEA for future: Units have a home & away behaviour depending on their owner  
 */