using RaTweening.RaLight;
using System;
using UnityEngine;

namespace RaTweening.RaLight
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween that is the base class that handles the logics of tweening the Range of a Light
	/// </summary>
	[Serializable]
	public class RaTweenRange : RaTweenDynamic<Light, float>
	{
		public RaTweenRange()
			: base()
		{

		}

		public RaTweenRange(Light target, float endValue, float duration)
			: base(target, endValue, duration)
		{

		}

		public RaTweenRange(Light target, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{

		}

		#region Protected Methods

		protected override RaTweenDynamic<Light, float> DynamicClone()
		{
			return new RaTweenRange();
		}

		protected override void DynamicEvaluation(float normalizedValue, Light target, float start, float end)
		{
			float delta = end - start;
			target.range = start + (delta * normalizedValue);
		}

		protected override float GetEndByDelta(float start, float delta)
		{
			return start + delta;
		}

		protected override float ReadValue(Light reference)
		{
			return reference.range;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		/// <summary>
		/// Tweens the Light's Range.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRange TweenRange(this Light self, float value, float duration)
		{
			return new RaTweenRange(self, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Light's Range.
		/// </summary>
		/// <param name="startValue">The value to tween from</param>
		/// <param name="endValue">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRange TweenRange(this Light self, float startValue, float endValue, float duration)
		{
			return new RaTweenRange(self, startValue, endValue, duration).Play();
		}
	}

	#endregion
}