using RaTweening.RaCamera;
using System;
using UnityEngine;

namespace RaTweening.RaCamera
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween that is the base class that handles the logics of tweening the Aspect Ratio of a Camera
	/// </summary>
	[Serializable]
	public class RaTweenAspect : RaTweenDynamic<Camera, float>
	{
		public RaTweenAspect()
			: base()
		{

		}

		public RaTweenAspect(Camera target, float endValue, float duration)
			: base(target, endValue, duration)
		{

		}

		public RaTweenAspect(Camera target, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{

		}

		#region Protected Methods

		protected override RaTweenDynamic<Camera, float> DynamicClone()
		{
			return new RaTweenAspect();
		}

		protected override void DynamicEvaluation(float normalizedValue, Camera target, float start, float end)
		{
			float delta = end - start;
			target.aspect = start + (delta * normalizedValue);
		}

		protected override float GetEndByDelta(float start, float delta)
		{
			return start + delta;
		}

		protected override float ReadValue(Camera reference)
		{
			return reference.aspect;
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
		/// Tweens the Camera's Aspect Ratio.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAspect TweenAspect(this Camera self, float value, float duration)
		{
			return new RaTweenAspect(self, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Camera's Aspect Ratio.
		/// </summary>
		/// <param name="startValue">The value to tween from</param>
		/// <param name="endValue">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAspect TweenAspect(this Camera self, float startValue, float endValue, float duration)
		{
			return new RaTweenAspect(self, startValue, endValue, duration).Play();
		}
	}

	#endregion
}