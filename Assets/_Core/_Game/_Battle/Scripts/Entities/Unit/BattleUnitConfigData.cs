using System;

namespace Game.Battle
{
	[Serializable]
	public struct BattleUnitConfigData : IHaveCurrencyValue
	{
		public BattleUnit BattleUnitPrefab;
		public int HealthPoints;
		public CurrencyValue Cost;
		public bool FirstColumnUnit;

		CurrencyValue IHaveCurrencyValue.CurrencyValue => Cost;
	}
}