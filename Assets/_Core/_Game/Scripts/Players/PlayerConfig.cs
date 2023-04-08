using UnityEngine;

namespace Game
{
	public class PlayerConfig : ConfigBase<PlayerConfig.CoreData>
	{
		[System.Serializable]
		public struct CoreData
		{
			[field: SerializeField]
			public int HP
			{
				get; private set;
			}

			[field: SerializeField]
			public UnitConfig[] Units
			{
				get; private set;
			}

			[SerializeField]
			private bool _hasInfiniteResources;

			[SerializeField]
			private int _resources;

			public int Resources => _hasInfiniteResources ? CurrencyGenerator.INFINITE_RESOURCE : _resources;
		}
	}
}