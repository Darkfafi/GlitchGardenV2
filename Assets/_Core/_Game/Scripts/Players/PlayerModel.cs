using System;

namespace Game
{

	public class PlayerModel : IDisposable
	{
		public PlayerConfig Config
		{
			get; private set;
		}

		public PlayerModel(PlayerConfig config)
		{
			Config = config;
		}

		public void Dispose()
		{
			Config = null;
		}
	}
}