using Game.Battle;
using RaModelsSO;
using RaScenesSO;
using UnityEngine;

namespace Game.Campaign
{
	public class EncounterBattleConfig : EncounterConfigBase
	{
		[field: SerializeField]
		public PlayerConfig.CoreData EnemyConfigData
		{
			get; private set;
		}

		[Header("Requirements")]
		[SerializeField]
		private RaModelSOCollection _models;

		public override void Enter(CampaignEncounter encounter)
		{
			OverworldGameModelSO overworldModel = _models.GetModelSO<OverworldGameModelSO>();
			BattleGameModelSO battleGameModel = _models.GetModelSO<BattleGameModelSO>();
			RaSceneModelSO sceneModelSO = _models.GetModelSO<RaSceneModelSO>();


			PlayerModel enemy = new PlayerModel(EnemyConfigData);
			battleGameModel.GoToBattleGame(overworldModel.Player, enemy, sceneModelSO.CurrentScene);
		}
	}
}