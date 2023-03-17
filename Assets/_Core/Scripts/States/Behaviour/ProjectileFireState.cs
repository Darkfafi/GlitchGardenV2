using RaFSM;
using UnityEngine;
using UnityEngine.Events;

public class ProjectileFireState : RaGOStateBase<Unit>
{
	[Header("Events")]
	public UnityEvent ProjectileFiredEvent;

	[Header("Functionality")]
	[SerializeField]
	private Projectile _projectilePrefab = null;

	[SerializeField]
	private Transform _projectileOrigin = null;

	[Header("Settings")]
	[SerializeField]
	private bool _fireOnEnter = true;

	protected override void OnInit()
	{

	}

	protected override void OnEnter()
	{
		if(_fireOnEnter)
		{
			Fire();
		}
	}

	protected override void OnExit(bool isSwitch)
	{

	}

	protected override void OnDeinit()
	{

	}

	public void Fire()
	{
		Projectile projectile = Instantiate(_projectilePrefab, _projectileOrigin.position, Quaternion.identity);
		projectile.Fire(Dependency.Owner);
		ProjectileFiredEvent.Invoke();
	}
}
