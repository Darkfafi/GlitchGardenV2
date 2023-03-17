using RaFSM;
using UnityEngine;

public class GameplayState : RaGOFSMState<Game>
{
	[field: SerializeField]
	public UnitsMechanicSO UnitsMechanicSO
	{
		get; private set;
	}

	protected override void OnPreSwitch()
	{
		if(IsCurrentState)
		{
			UnitsMechanicSO.IsEnabledFlags.Unregister(this);
			Dependency.GameUI.UnitsHUD.InUseFlags.Unregister(this);
		}
		else
		{
			UnitsMechanicSO.IsEnabledFlags.Register(this);
			Dependency.GameUI.UnitsHUD.InUseFlags.Register(this);
		}
	}
}
