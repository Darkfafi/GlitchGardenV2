using RaModelsSO;
using UnityEngine;

namespace Game.Battle
{
	public class BattleGameModelSO : RaModelSOBase
	{
		[SerializeField]
		private PlayerConfig _defaultPlayerConfig;

		[SerializeField]
		private PlayerConfig.CoreData _defaultEnemyConfigData;

		public PlayerModel Player
		{
			get; private set;
		}

		public PlayerModel Enemy
		{
			get; private set;
		}

		public void GoToBattleGame(PlayerModel player, PlayerModel enemy)
		{
			Player = player;
			Enemy = enemy;
		}

		protected override void OnInit()
		{
			Player = new PlayerModel(_defaultPlayerConfig.Data);
			Enemy = new PlayerModel(_defaultEnemyConfigData);
		}

		protected override void OnDeinit()
		{
			Enemy = default;
			Player = default;
		}
	}
}