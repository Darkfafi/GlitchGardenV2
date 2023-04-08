using System;

namespace Game
{
	public class PlayerModel : IDisposable
	{
		public PlayerConfig.CoreData ConfigData
		{
			get; private set;
		}

		public PlayerModel(PlayerConfig.CoreData config)
		{
			ConfigData = config;
		}

		public void Dispose()
		{
			ConfigData = default;
		}
	}
}