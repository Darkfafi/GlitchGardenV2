using RaFSM;

public class BehaviourRootState : RaGOFSMState<Unit>
{
	protected override void OnEnter()
	{
		base.OnEnter();
		Dependency.Health.HealthChangedEvent += OnHealthChangedEvent;
		Dependency.Collider.enabled = true;
	}

	protected override void OnExit(bool isSwitch)
	{
		Dependency.Collider.enabled = false;
		Dependency.Health.HealthChangedEvent -= OnHealthChangedEvent;
		base.OnExit(isSwitch);
	}

	private void OnHealthChangedEvent(Health health, int delta)
	{
		if(!health.IsAlive)
		{
			Dependency.SetState(Unit.State.Death);
		}
	}
}
