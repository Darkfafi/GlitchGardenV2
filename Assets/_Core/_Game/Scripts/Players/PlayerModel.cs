using System;
using RaCollection;

namespace Game
{
	public class PlayerModel : IDisposable
	{
		public PlayerConfig.CoreData ConfigData
		{
			get; private set;
		}

		public Health Health
		{
			get; private set;
		}

		public Inventory Inventory
		{
			get; private set;
		}

		public PlayerModel(PlayerConfig.CoreData coreData)
		{
			Inventory = new Inventory();

			ConfigData = coreData;

			if(coreData.HP > 0)
			{
				Health = new Health(coreData.HP);
			}
			else
			{
				Health = null;
			}

			if(coreData.Units != null)
			{
				coreData.Units.ForEach(unitConfig => Inventory.Items.Add(new UnitModel(unitConfig)));
			}
		}

		public void Dispose()
		{
			if(Health != null)
			{
				Health.Dispose();
				Health = default;
			}

			if(Inventory != null)
			{
				Inventory.Clear();
				Inventory = null;
			}

			ConfigData = default;
		}
	}
}