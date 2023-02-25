using RaTweening.UI.RaImage;
using System;
using UnityEngine.UI;

namespace RaTweening.UI.RaImage
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Fill of an Image
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenFill : RaTweenDynamic<Image, float>
	{
		public RaTweenFill()
			: base()
		{

		}

		public RaTweenFill(Image target, float startFill, float endFill, float duration)
			: base(target, startFill, endFill, duration)
		{

		}

		public RaTweenFill(Image target, float endFill, float duration)
			: base(target, endFill, duration)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(1f);
			SetEndValue(0f);
		}

		protected override void DynamicEvaluation(float normalizedValue, Image target, float start, float end)
		{
			float delta = end - start;
			target.fillAmount = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<Image, float> DynamicClone()
		{
			RaTweenFill tween = new RaTweenFill();
			return tween;
		}

		protected override float ReadValue(Image reference)
		{
			return reference.fillAmount;
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
		/// Tweens the Image's Fill Value.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="fill">The fill value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenFill TweenFill(this Image self, float fill, float duration)
		{
			return new RaTweenFill(self, fill, duration).Play();
		}

		/// <summary>
		/// Tweens the Image's Fill Value.
		/// </summary>
		/// <param name="startFill">The fill value to tween from (value between 0 - 1)</param>
		/// <param name="endFill">The fill value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenFill TweenFill(this Image self, float startFill, float endFill, float duration)
		{
			return new RaTweenFill(self, startFill, endFill, duration).Play();
		}
	}

	#endregion
}
