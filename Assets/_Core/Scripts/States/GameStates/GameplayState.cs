using RaFlags;
using RaFSM;
using UI;
using UnityEngine;
using RaCollection;

public class GameplayState : RaGOFSMState<Game>
{
	[field: SerializeField]
	public UnitsMechanicSO UnitsMechanicSO
	{
		get; private set;
	}

	[SerializeField]
	private InGameConditionBase[] _ingameConditions = null;

	[SerializeField]
	private GameplayUIGroup _gameplayUIGroup = null;

#if UNITY_EDITOR
	protected override void OnValidate()
	{
		base.OnValidate();
		_ingameConditions = transform.GetComponentsInChildren<InGameConditionBase>();
	}
#endif

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

		// Run Generators
		Dependency.HomePlayerSide.ResourceGenerator.Run();
		Dependency.AwayPlayerSide.ResourceGenerator.Run();

		// Run InGameFlag Condtions
		_ingameConditions.ForEach((item, index) => { item.SetData(Dependency); });

		// Listen to InGameFlags changed
		Dependency.HomePlayerSide.InGameFlags.IsEmptyChangedEvent += OnHomeInGameFlagsStateChangedEvent;
		Dependency.AwayPlayerSide.InGameFlags.IsEmptyChangedEvent += OnAwayInGameFlagsStateChangedEvent;

		// Run Check
		OnHomeInGameFlagsStateChangedEvent(Dependency.HomePlayerSide.InGameFlags.IsEmpty(), Dependency.HomePlayerSide.InGameFlags);
		OnAwayInGameFlagsStateChangedEvent(Dependency.AwayPlayerSide.InGameFlags.IsEmpty(), Dependency.AwayPlayerSide.InGameFlags);
	}

	protected override void OnExit(bool isSwitch)
	{
		Dependency.AwayPlayerSide.InGameFlags.IsEmptyChangedEvent -= OnAwayInGameFlagsStateChangedEvent;
		Dependency.HomePlayerSide.InGameFlags.IsEmptyChangedEvent -= OnHomeInGameFlagsStateChangedEvent;

		_ingameConditions.ForEachReverse((item, index) => { item.ClearData(); });

		Dependency.AwayPlayerSide.ResourceGenerator.Stop();
		Dependency.HomePlayerSide.ResourceGenerator.Stop();

		if(_gameplayUIGroup.Users != null)
		{
			_gameplayUIGroup.Users.Unregister(this);
		}

		UnitsMechanicSO.IsEnabledFlags.Unregister(this);
		
		base.OnExit(isSwitch);
	}

	private void OnHomeInGameFlagsStateChangedEvent(bool isEmpty, RaFlagsTracker tracker)
	{
		if(isEmpty)
		{
			// Lost
			Debug.Log("Lost!");
		}
	}

	private void OnAwayInGameFlagsStateChangedEvent(bool isEmpty, RaFlagsTracker tracker)
	{
		if(isEmpty)
		{
			// Won
			Debug.Log("Won!");
		}
	}
}
