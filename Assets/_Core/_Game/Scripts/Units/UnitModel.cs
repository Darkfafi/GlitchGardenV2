using Game.Battle;

namespace Game
{
	public class UnitModel : IInventoryItem
	{
		public string Id
		{
			get; private set;
		}

		public UnitConfig Config
		{
			get; private set;
		}

		public BattleUnitConfigData BattleUnitData
		{
			get; private set;
		}

		public UnitModel(UnitConfig config)
		{
			Id = System.Guid.NewGuid().ToString();
			Config = config;

			BattleUnitData = config.BattleUnitConfigData;
		}

		public void SetBattleUnitData(BattleUnitConfigData data)
		{
			BattleUnitData = data;
		}
	}
}