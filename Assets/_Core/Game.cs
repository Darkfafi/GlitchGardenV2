using RaDataHolder;
using UnityEngine;
using RaFSM;

public class Game : MonoBehaviour
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
	private Transform _statesContainer = null;

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
		_fsm = new RaGOFiniteStateMachine(this, RaGOFiniteStateMachine.GetGOStates(_statesContainer));
	}

	protected void Start()
	{
		// Start
		_fsm.SwitchState(0);
	}

	protected void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			_fsm.GoToNextState(false);
		}
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
}

// Home Player
/*
 * Has Resources to pay for units
 * Gains Resources over time
 * Can place units from its list on the Buildable Tiles assigned to him
 * Has assigned column to protect from enemy units
 */

// Away Player
/*
 * Has Resources to pay for units
 * Gains Resources over time
 * Can place units from its list on the Buildable Tiles assigned to him
	* Will place units on first row, except for spiders and cacti
 * Wishes to push enemy units to assined row of Home Player
 * IDEA for future: Units have a home & away behaviour depending on their owner  
 */