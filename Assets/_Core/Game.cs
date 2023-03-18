using RaDataHolder;
using UnityEngine;
using RaFSM;

public class Game : MonoBehaviour, IRaFSMState
{
	[field: SerializeField]
	public GameUI GameUI
	{
		get; private set;
	}

	[field: SerializeField]
	public GameGrid Grid
	{
		get; private set;
	}

	[field: SerializeField]
	public PlayerSide HomePlayerSide
	{
		get; private set;
	}

	[field: SerializeField]
	public PlayerSide AwayPlayerSide
	{
		get; private set;
	}

	[field: SerializeField]
	public PlayerSideAI AwayPlayerSideAI
	{
		get; private set;
	}

	[SerializeField]
	private RaGOFSMState _gameFSM = null;

	[Header("Settings")]
	[SerializeField]
	private PlayerModel _homePlayerModel = null;
	[SerializeField]
	private PlayerModel _awayPlayerModel = null;
	[SerializeField]
	private GameGrid.CoreData _gridData = default;

	[Header("Controllers")]
	[SerializeField]
	private ModelsSOController _modelsController;
	[SerializeField]
	private GameMechanicsSOController _mechanicsController;

	private RaGOFiniteStateMachine _fsm = null;

	protected void Awake()
	{
		IRaDataSetResolver[] gameBoard = new IRaDataSetResolver[]
		{
			// Setting Participants
			HomePlayerSide.SetData(_homePlayerModel, false),
			AwayPlayerSide.SetData(_awayPlayerModel, false),

			// Settings Game Board
			Grid.SetData(_gridData, false),

			// AI Setup
			AwayPlayerSideAI.SetData(AwayPlayerSide, false),
		};

		// Model Accessor
		_modelsController.Initialize();

		// Mechanics
		_mechanicsController.Initialize();

		// Resolve Game Board
		gameBoard.ResolveAll();

		// UI
		GameUI.SetData(this, true);

		// Finite State Machine
		_fsm = new RaGOFiniteStateMachine(this, new RaGOStateBase[] { _gameFSM });
	}

	protected void Start()
	{
		// Start
		_fsm.SwitchState(0);
	}

	protected void OnDestroy()
	{
		GameUI.ClearData();

		_fsm.Dispose();

		_mechanicsController.Deinitialize();
		_modelsController.Deinitialize();

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

	public PlayerSide GetPlayerSide(Player.Type playerType)
	{
		switch(playerType)
		{
			case Player.Type.Home:
				return HomePlayerSide;
			case Player.Type.Away:
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