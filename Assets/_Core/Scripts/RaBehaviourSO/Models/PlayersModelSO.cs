using UnityEngine;

[CreateAssetMenu(menuName = "Behaviours/Models/PlayersModelSO")]
public class PlayersModelSO : ModelSOBase
{
	public PlayerSide HomePlayerSide
	{
		get; private set;
	}

	public PlayerSide AwayPlayerSide
	{
		get; private set;
	}

	protected override void OnSetup()
	{
		HomePlayerSide = Game.HomePlayerSide;
		AwayPlayerSide = Game.AwayPlayerSide;
	}

	protected override void OnStart()
	{

	}

	protected override void OnEnd()
	{
		AwayPlayerSide = null;
		HomePlayerSide = null;
	}

	public Player GetPlayer(Player.Type playerType)
	{
		return GetPlayerSide(playerType).Player;
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
}
