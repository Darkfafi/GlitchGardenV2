using UnityEngine;

namespace RaTweening.TimeScale
{
	/// <summary>
	/// The Base Class for ScriptableObjects representing a <see cref="ITimeScaleChannel"/>
	/// > Note: This is used to give TimeScaleChannel editability access for a <see cref="RaTweenerComponent"/>
	/// </summary>
	public abstract class TimeScaleChannelSO : ScriptableObject, ITimeScaleChannel
	{
		public abstract float TimeScale
		{
			get;
		}
	}
}
