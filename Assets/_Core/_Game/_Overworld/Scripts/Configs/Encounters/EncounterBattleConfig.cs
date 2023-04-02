using Game.Battle;
using RaModelsSO;
using UnityEngine;

namespace Game.Campaign
{
	public class EncounterBattleConfig : EncounterConfigBase
	{
		[field: SerializeField]
		public PlayerConfig EnemyConfig
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
			PlayerModel enemy = new PlayerModel(EnemyConfig);
			battleGameModel.GoToBattleGame(overworldModel.Player, enemy);
		}
	}
}