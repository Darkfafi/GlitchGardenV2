using RaModelsSO;
using RaScenesSO;
using UnityEngine;

namespace Game.Battle
{
	public class BattleGameModelSO : RaModelSOBase
	{
		[Header("Defaults")]
		[SerializeField]
		private PlayerConfig _defaultPlayerConfig;

		[SerializeField]
		private PlayerConfig.CoreData _defaultEnemyConfigData;

		[SerializeField]
		private RaSceneSO _defaultReturnScene = null;

		[Header("Requirements")]
		[SerializeField]
		private RaSceneSO _battleScene = null;

		public PlayerModel Player
		{
			get; private set;
		}

		public PlayerModel Enemy
		{
			get; private set;
		}

		public RaSceneSO ReturnScene
		{
			get; private set;
		}

		public void GoToBattleGame(PlayerModel player, PlayerModel enemy, RaSceneSO returnScene)
		{
			Player = player;
			Enemy = enemy;
			ReturnScene = returnScene;

			DataSOCollection.GetModelSO<RaSceneModelSO>().LoadScene(_battleScene);
		}

		protected override void OnInit()
		{
			Player = new PlayerModel(_defaultPlayerConfig.Data);
			Enemy = new PlayerModel(_defaultEnemyConfigData);
			ReturnScene = _defaultReturnScene;
		}

		protected override void OnDeinit()
		{
			ReturnScene = default;
			Enemy = default;
			Player = default;
		}
	}
}