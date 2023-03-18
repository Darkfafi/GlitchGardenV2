using RaFlags;
using UnityEngine;

public abstract class UsableElementBase : MonoBehaviour
{
	private RaFlagsTracker _users = null;

	public RaFlagsTracker Users
	{
		get
		{
			TryInitialize();
			return _users;
		}
	}

	private bool _isDestroyed = false;

	protected abstract void OnInitialization();
	protected abstract void OnDeinitialization();

	protected abstract void OnUseStarted();
	protected abstract void OnUseEnded();

	public void RegisterUser(object user, out bool switchedToInUse)
	{
		bool isEmpty = Users.IsEmpty();
		Users.Register(user);
		switchedToInUse = isEmpty != Users.IsEmpty();
	}

	public void UnregisterUser(object user, out bool switchedToUnused)
	{
		bool isEmpty = Users.IsEmpty();
		Users.Unregister(user);
		switchedToUnused = isEmpty != Users.IsEmpty();
	}

	private void OnChangedInUse(bool isEmpty, RaFlagsTracker tracker)
	{
		if(isEmpty)
		{
			OnUseEnded();
		}
		else
		{
			OnUseStarted();
		}
	}

	protected void Awake()
	{
		TryInitialize();
	}

	protected void OnDestroy()
	{
		TryDeinitialize();
	}

	private void TryDeinitialize()
	{
		if(_isDestroyed)
		{
			return;
		}
		_isDestroyed = true;
		OnDeinitialization();
		if(_users != null)
		{
			_users.Dispose();
			_users = null;
		}
	}

	private void TryInitialize()
	{
		if(_users != null || _isDestroyed)
		{
			return;
		}
		_users = new RaFlagsTracker(OnChangedInUse);
		OnInitialization();
	}
}

