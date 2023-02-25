using RaTweening.UI.RaSlider;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace RaTweening.UI.RaSlider
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Value of a Slider
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenValue : RaTweenDynamic<Slider, float>
	{
		public RaTweenValue()
			: base()
		{

		}

		public RaTweenValue(Slider target, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{

		}

		public RaTweenValue(Slider target, float endValue, float duration)
			: base(target, endValue, duration)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(0f);
			SetEndValue(1f);
		}

		protected override void DynamicEvaluation(float normalizedValue, Slider target, float start, float end)
		{
			float delta = end - start;
			target.value = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<Slider, float> DynamicClone()
		{
			RaTweenValue tween = new RaTweenValue();
			return tween;
		}

		protected override float ReadValue(Slider reference)
		{
			return reference.value;
		}

		protected override float GetEndByDelta(float start, float delta)
		{
			return start + delta;
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
		/// Tweens the Slider's Value.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenValue TweenValue(this Slider self, float value, float duration)
		{
			return new RaTweenValue(self, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Slider's Value.
		/// </summary>
		/// <param name="startValue">The value to tween from (value between 0 - 1)</param>
		/// <param name="endValue">The value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenValue TweenValue(this Slider self, float startValue, float endValue, float duration)
		{
			return new RaTweenValue(self, startValue, endValue, duration).Play();
		}

		/// <summary>
		/// Tweens the Slider's Value.
		/// </summary>
		/// <param name="startValue">The value to tween from (value between 0 - 1)</param>
		/// <param name="endTarget">The reference to which's value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenValue TweenValue(this Slider self, float startValue, Slider endTarget, float duration)
		{
			RaTweenValue tween = new RaTweenValue(self, startValue, default, duration);
			tween.SetEndRef(endTarget);
			return tween.Play();
		}
	}

	#endregion
}