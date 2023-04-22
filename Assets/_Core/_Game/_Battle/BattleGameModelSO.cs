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

		public ReturnData ReturnSceneData
		{
			get; private set;
		}

		public void GoToBattleGame(PlayerModel player, PlayerModel enemy, ReturnData returnSceneData)
		{
			Player = player;
			Enemy = enemy;
			ReturnSceneData = returnSceneData;

			Locator.GetModelSO<RaSceneModelSO>().LoadScene(_battleScene);
		}

		public void EndBattleGame()
		{
			Locator.GetModelSO<RaSceneModelSO>().LoadScene(ReturnSceneData.ReturnScene);
		}

		protected override void OnInit()
		{
			Player = new PlayerModel(_defaultPlayerConfig.Data);
			Enemy = new PlayerModel(_defaultEnemyConfigData);
			ReturnSceneData = new ReturnData()
			{
				Id = (-1).ToString(),
				ReturnScene = _defaultReturnScene,
			};
		}

		protected override void OnDeinit()
		{
			ReturnSceneData = default;
			Enemy = default;
			Player = default;
		}

		public struct ReturnData
		{
			public string Id;
			public RaSceneSO ReturnScene;
		}
	}
}