using System;

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

		public PlayerModel(PlayerConfig.CoreData config)
		{
			ConfigData = config;

			if(config.HP > 0)
			{
				Health = new Health(config.HP);
			}
			else
			{
				Health = null;
			}
		}

		public void Dispose()
		{
			if(Health != null)
			{
				Health.Dispose();
				Health = default;
			}

			ConfigData = default;
		}
	}
}