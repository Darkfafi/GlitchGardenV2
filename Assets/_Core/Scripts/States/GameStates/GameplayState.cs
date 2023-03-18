using RaFlags;
using RaFSM;
using UnityEngine;
using UI;

public class GameplayState : RaGOFSMState<Game>
{
	[field: SerializeField]
	public UnitsMechanicSO UnitsMechanicSO
	{
		get; private set;
	}

	[SerializeField]
	private GameplayUIGroup _gameplayUIGroup = null;

	protected override void OnPreSwitch()
	{
		// About to Enter, Register itself as user of various features / elements
		if(!IsCurrentState)
		{
			UnitsMechanicSO.IsEnabledFlags.Register(this);
			_gameplayUIGroup.Users.Register(this);
		}
	}

	protected override void OnEnter()
	{
		base.OnEnter();
		Dependency.HomePlayerSide.InGameFlags.IsEmptyChangedEvent += OnHomeInGameFlagsStateChangedEvent;
		Dependency.AwayPlayerSide.InGameFlags.IsEmptyChangedEvent += OnAwayInGameFlagsStateChangedEvent;
	}

	protected override void OnExit(bool isSwitch)
	{
		Dependency.HomePlayerSide.InGameFlags.IsEmptyChangedEvent -= OnHomeInGameFlagsStateChangedEvent;
		Dependency.AwayPlayerSide.InGameFlags.IsEmptyChangedEvent -= OnAwayInGameFlagsStateChangedEvent;

		_gameplayUIGroup.Users.Unregister(this);
		UnitsMechanicSO.IsEnabledFlags.Unregister(this);
		base.OnExit(isSwitch);
	}

	private void OnHomeInGameFlagsStateChangedEvent(bool isEmpty, RaFlagsTracker tracker)
	{
		if(isEmpty)
		{
			// Lost
		}
	}

	private void OnAwayInGameFlagsStateChangedEvent(bool isEmpty, RaFlagsTracker tracker)
	{
		if(isEmpty)
		{
			// Won
		}
	}
}
