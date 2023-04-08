using RaDataHolder;
using RaFSM;
using RaModelsSO;
using UnityEngine;

namespace Game.Battle
{
	public class BattleGameSceneRoot : SceneRootBase, IRaFSMCycler
	{
		[field: SerializeField]
		public BattleGameUI GameUI
		{
			get; private set;
		}

		[field: SerializeField]
		public BattleGameGrid Grid
		{
			get; private set;
		}

		[field: SerializeField]
		public BattlePlayerSide HomePlayerSide
		{
			get; private set;
		}

		[field: SerializeField]
		public BattlePlayerSide AwayPlayerSide
		{
			get; private set;
		}

		[field: SerializeField]
		public BattlePlayerSideAI AwayPlayerSideAI
		{
			get; private set;
		}

		[SerializeField]
		private RaGOFSMState _gameFSM = null;

		[Header("Dependencies")]
		[SerializeField]
		private RaModelSOCollection _models = null;
		[SerializeField]
		private BattleGameGrid.CoreData _gridData = default;

		[Header("Controllers")]
		[SerializeField]
		private BattleGameReferenceSOController _referencesController;
		[SerializeField]
		private BattleGameMechanicsSOController _mechanicsController;

		private RaGOFiniteStateMachine _fsm = null;

		protected override void OnSetup()
		{
			BattleGameModelSO battleGameModelSO = _models.GetModelSO<BattleGameModelSO>();

			IRaDataSetResolver[] gameBoard = new IRaDataSetResolver[]
			{
				// Setting Participants
				HomePlayerSide.SetData(battleGameModelSO.Player, false),
				AwayPlayerSide.SetData(battleGameModelSO.Enemy, false),

				// Settings Game Board
				Grid.SetData(_gridData, false),

				// AI Setup
				AwayPlayerSideAI.SetData(AwayPlayerSide, false),
			};

			// Model Accessor
			_referencesController.Initialize();

			// Mechanics
			_mechanicsController.Initialize();

			// Resolve Game Board
			gameBoard.ResolveAll();

			// UI
			GameUI.SetData(this, true);

			// Finite State Machine
			_fsm = new RaGOFiniteStateMachine(this, new RaGOStateBase[] { _gameFSM });
		}

		protected override void OnStart()
		{
			// Start
			_fsm.SwitchState(0);
		}

		protected override void OnEnd()
		{
			GameUI.ClearData();

			_fsm.Dispose();

			_mechanicsController.Deinitialize();
			_referencesController.Deinitialize();

			IRaDataClearResolver[] gameBoard = new IRaDataClearResolver[]
			{
				// Clearing Game Board
				Grid.ClearData(false),

				// Clearing Participants
				AwayPlayerSide.ClearData(false),
				HomePlayerSide.ClearData(false)
			};

			gameBoard.ResolveAll();
		}

		public BattlePlayerSide GetPlayerSide(BattlePlayer.Type playerType)
		{
			switch(playerType)
			{
				case BattlePlayer.Type.Home:
					return HomePlayerSide;
				case BattlePlayer.Type.Away:
					return AwayPlayerSide;
			}
			return default;

		}

		public void GoToNextState()
		{
			if(_fsm != null)
			{
				_fsm.GoToNextState(false);
			}
		}
	}
}