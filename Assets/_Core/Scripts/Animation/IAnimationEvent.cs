using UnityEngine;

public interface IAnimationEvent
{
	string AnimationEventName
	{
		get;
	}

	void FireEvent();
}
