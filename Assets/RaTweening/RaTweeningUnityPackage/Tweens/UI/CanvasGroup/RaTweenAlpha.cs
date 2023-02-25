using RaTweening.UI.RaCanvasGroup;
using System;
using UnityEngine;

namespace RaTweening.UI.RaCanvasGroup
{

	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Alpha of a CanvasGroup (Fading)
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenAlpha : RaTweenDynamic<CanvasGroup, float>
	{
		public RaTweenAlpha()
			: base()
		{

		}

		public RaTweenAlpha(CanvasGroup target, float startAlpha, float endAlpha, float duration)
			: base(target, startAlpha, endAlpha, duration)
		{

		}

		public RaTweenAlpha(CanvasGroup target, float endAlpha, float duration)
			: base(target, endAlpha, duration)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(1f);
			SetEndValue(0f);
		}

		protected override void DynamicEvaluation(float normalizedValue, CanvasGroup target, float start, float end)
		{
			float delta = end - start;
			target.alpha = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<CanvasGroup, float> DynamicClone()
		{
			RaTweenAlpha tween = new RaTweenAlpha();
			return tween;
		}

		protected override float ReadValue(CanvasGroup reference)
		{
			return reference.alpha;
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
		/// Tweens the CanvasGroup's Alpha Value.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="alpha">The alpha value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAlpha TweenAlpha(this CanvasGroup self, float alpha, float duration)
		{
			return new RaTweenAlpha(self, alpha, duration).Play();
		}

		/// <summary>
		/// Tweens the CanvasGroup's Alpha Value.
		/// </summary>
		/// <param name="startAlpha">The alpha value to tween from (value between 0 - 1)</param>
		/// <param name="endAlpha">The alpha value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAlpha TweenAlpha(this CanvasGroup self, float startAlpha, float endAlpha, float duration)
		{
			return new RaTweenAlpha(self, startAlpha, endAlpha, duration).Play();
		}

		/// <summary>
		/// Tweens the CanvasGroup's Alpha Value.
		/// </summary>
		/// <param name="startAlpha">The alpha value to tween from (value between 0 - 1)</param>
		/// <param name="endTarget">The reference to which's alpha to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAlpha TweenAlpha(this CanvasGroup self, float startAlpha, CanvasGroup endTarget, float duration)
		{
			RaTweenAlpha tween = new RaTweenAlpha(self, startAlpha, default, duration);
			tween.SetEndRef(endTarget);
			return tween.Play();
		}
	}

	#endregion
}