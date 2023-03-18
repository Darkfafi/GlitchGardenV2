using RaDataHolder;
using UnityEngine;

public abstract class InGameConditionBase : RaMonoDataHolderBase<Game>
{
	[SerializeField]
	private Player.Type _playerType = Player.Type.Home;

	[SerializeField]
	private bool _setCondition = false;

	public Player.Type PlayerSideType => _playerType;
	public PlayerSide PlayerSide => Data != null ? Data.GetPlayerSide(_playerType) : null;

	protected override void OnSetData()
	{
		OnStartRunning();
	}

	protected override void OnSetDataResolved()
	{
		SetInGame(CheckCondition());
	}

	protected override void OnClearData()
	{
		OnStopRunning();
		SetInGame(false);
	}

	protected abstract void OnStartRunning();
	protected abstract void OnStopRunning();

	protected abstract bool CheckCondition();

	protected void SetInGame(bool inGame)
	{
		if(_setCondition == inGame)
		{
			return;
		}

		if(!HasData)
		{
			return;
		}

		_setCondition = inGame;

		if(inGame)
		{
			PlayerSide.InGameFlags.Register(this);
		}
		else
		{
			PlayerSide.InGameFlags.Unregister(this);
		}
	}
}
