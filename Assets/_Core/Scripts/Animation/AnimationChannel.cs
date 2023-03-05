using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationChannel : MonoBehaviour
{
	[SerializeField]
	private Transform[] _eventChannels = null;

	private Animator _animator = null;
	private Dictionary<string, AnimationEvent> _keyToEvent = null;

	protected void Awake()
	{
		TryInitialize();
	}

	public void SetBoolTrue(string key)
	{
		TryInitialize();
		_animator.SetBool(key, true);
	}

	public void SetBoolFalse(string key)
	{
		TryInitialize();
		_animator.SetBool(key, false);
	}

	public void SetTrigger(string key)
	{
		TryInitialize();
		_animator.SetTrigger(key);
	}

	public void FireEvent(string eventPath)
	{
		TryInitialize();
		if(_keyToEvent.TryGetValue(eventPath, out AnimationEvent e))
		{
			e.Event.Invoke();
		}
		else
		{
			Debug.LogError($"No {nameof(AnimationEvent)} found at {eventPath}");
		}
	}

	private void TryInitialize()
	{
		if(_keyToEvent == null)
		{
			_animator = GetComponent<Animator>();
			_keyToEvent = new Dictionary<string, AnimationEvent>();

			for(int i = 0; i < _eventChannels.Length; i++)
			{
				Transform parent = _eventChannels[i];
				AnimationEvent[] events = parent.GetComponentsInChildren<AnimationEvent>();
				for(int j = 0; j < events.Length; j++)
				{
					AnimationEvent e = events[j];
					string path = $"{parent.name}/{e.name}";
					_keyToEvent[path] = e;
				}
			}
		}
	}
}
