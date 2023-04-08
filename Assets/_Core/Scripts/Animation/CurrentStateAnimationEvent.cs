using UnityEngine;
using UnityEngine.Events;
using RaFSM;

public class CurrentStateAnimationEvent : AnimationEventBase
{
	[SerializeField]
	private Entry[] _entries = null;

	public override void FireEvent()
	{
		if(_entries != null)
		{
			for(int i = 0; i < _entries.Length; i++)
			{
				Entry entry = _entries[i];
				if(entry.State != null && entry.State.IsCurrentState)
				{
					entry.Event.Invoke();
					return;
				}
			}
		}

		Debug.LogError($"Non of the Entries had a state which was the current state, and the {nameof(FireEvent)} is thus ignored.");
	}

	[System.Serializable]
	private struct Entry
	{
		public RaGOStateBase State;
		public UnityEvent Event;
	}
}
