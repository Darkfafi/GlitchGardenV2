using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AnimationEventBase : MonoBehaviour, IAnimationEvent
{
	public string AnimationEventName => name;

	public abstract void FireEvent();
}
