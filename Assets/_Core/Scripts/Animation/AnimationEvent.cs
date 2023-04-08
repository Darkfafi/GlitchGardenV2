using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class AnimationEvent : AnimationEventBase
{
	[SerializeField]
	[FormerlySerializedAs("Event")]
	private UnityEvent _event = null;

	public override void FireEvent()
	{
		_event.Invoke();
	}
}
