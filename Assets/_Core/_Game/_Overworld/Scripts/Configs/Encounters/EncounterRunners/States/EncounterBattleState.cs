using Game.Battle;
using RaScenesSO;
using UnityEngine;

namespace Game.Campaign
{
	public class EncounterBattleState : EncounterStateBase
	{
		[field: SerializeField, Header("Battle Encounter")]
		public PlayerConfig.CoreData EnemyConfigData
		{
			get; private set;
		}

		private RaSceneModelSO _sceneModelSO = null;
		private BattleGameModelSO _battleGameModelSO = null;
		private RaSceneSO _enterScene = null;

		protected override void OnInit()
		{
			_battleGameModelSO = ModelLocator.GetModelSO<BattleGameModelSO>();
			_sceneModelSO = ModelLocator.GetModelSO<RaSceneModelSO>();
			_enterScene = _sceneModelSO.CurrentScene;
		}

		protected override void OnEnter()
		{
			OverworldGameModelSO overworldModel = ModelLocator.GetModelSO<OverworldGameModelSO>();

			_sceneModelSO.SceneLoadEndedEvent += OnSceneLoadedEvent;

			PlayerModel enemy = new PlayerModel(EnemyConfigData);

			_battleGameModelSO.GoToBattleGame(overworldModel.Player, enemy, new BattleGameModelSO.ReturnData()
			{
				Id = Dependency.Encounter.Id,
				ReturnScene = _enterScene,
			});
		}

		protected override void OnExit(bool isSwitch)
		{
			_sceneModelSO.SceneLoadEndedEvent -= OnSceneLoadedEvent;
		}

		protected override void OnDeinit()
		{
			_enterScene = null;
			_sceneModelSO = null;
			_battleGameModelSO = null;
		}

		private void OnSceneLoadedEvent(RaSceneSO scene)
		{
			if(IsCurrentState)
			{
				if(	_enterScene == scene && 
					_battleGameModelSO.ReturnSceneData.Id == Dependency.Encounter.Id)
				{
					Dependency.GoToNextState();
				}
			}
		}
	}
}