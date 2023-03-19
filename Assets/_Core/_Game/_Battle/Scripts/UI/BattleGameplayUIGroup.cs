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
	}
}