using RaTweening.RaCamera;
using System;
using UnityEngine;

namespace RaTweening.RaCamera
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween that is the base class that handles the logics of tweening the Perspective FieldOfView of a Camera
	/// > Note: This tween only affects a camera which is set to Perspective (3D Camera)
	/// </summary>
	[Serializable]
	public class RaTweenFieldOfView : RaTweenDynamic<Camera, float>
	{
		public RaTweenFieldOfView()
			: base()
		{

		}

		public RaTweenFieldOfView(Camera target, float endValue, float duration)
			: base(target, endValue, duration)
		{

		}

		public RaTweenFieldOfView(Camera target, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{

		}

		#region Protected Methods

		protected override RaTweenDynamic<Camera, float> DynamicClone()
		{
			return new RaTweenFieldOfView();
		}

		protected override void DynamicEvaluation(float normalizedValue, Camera target, float start, float end)
		{
			float delta = end - start;
			target.fieldOfView = start + (delta * normalizedValue);
		}

		protected override float GetEndByDelta(float start, float delta)
		{
			return start + delta;
		}

		protected override float ReadValue(Camera reference)
		{
			return reference.fieldOfView;
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
		/// Tweens the Camera's Perspective FieldOfView.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenFieldOfView TweenFieldOfView(this Camera self, float value, float duration)
		{
			return new RaTweenFieldOfView(self, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Camera's Perspective FieldOfView.
		/// </summary>
		/// <param name="startValue">The value to tween from</param>
		/// <param name="endValue">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenFieldOfView TweenFieldOfView(this Camera self, float startValue, float endValue, float duration)
		{
			return new RaTweenFieldOfView(self, startValue, endValue, duration).Play();
		}
	}

	#endregion
}