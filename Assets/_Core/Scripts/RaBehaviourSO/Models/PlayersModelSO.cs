using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/Models/PlayersModelSO")]
public class PlayersModelSO : ModelSOBase
{
	public Player HomePlayer
	{
		get; private set;
	}

	public Player AwayPlayer
	{
		get; private set;
	}

	protected override void OnSetup()
	{
		HomePlayer = Game.HomePlayer;
		AwayPlayer = Game.AwayPlayer;
	}

	protected override void OnStart()
	{

	}

	protected override void OnEnd()
	{
		AwayPlayer = null;
		HomePlayer = null;
	}
}
