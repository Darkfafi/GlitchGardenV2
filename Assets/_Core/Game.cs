using UnityEngine;

public class Game : MonoBehaviour
{
	[field: SerializeField]
	public GameGrid Grid
	{
		get; private set;
	}

	[field: SerializeField]
	public Player HomePlayer
	{
		get; private set;
	}

	[field: SerializeField]
	public Player AwayPlayer
	{
		get; private set;
	}

	[field: SerializeField]
	public GameUI GameUI
	{
		get; private set;
	}

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

	protected void Awake()
	{
		// Setting Participants
		HomePlayer.SetData(_homePlayerModel);
		AwayPlayer.SetData(_awayPlayerModel);

		// Settings Game Board
		Grid.SetData(_gridData);

		_modelsController.Initialize();
		_mechanicsController.Initialize();

		GameUI.SetData(this);
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