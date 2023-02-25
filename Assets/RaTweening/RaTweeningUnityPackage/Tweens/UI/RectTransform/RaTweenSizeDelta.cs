using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the SizeDelta of a RectTransform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenSizeDelta : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenSizeDelta")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenSizeDelta()
			: base()
		{

		}

		public RaTweenSizeDelta(RectTransform target, Vector2 startSize, Vector2 endSize, float duration)
			: base(target, startSize, endSize, duration)
		{

		}

		public RaTweenSizeDelta(RectTransform target, Vector2 endSize, float duration)
			: base(target, endSize, duration)
		{

		}

		public RaTweenSizeDelta(RectTransform target, Vector2 startSize, RectTransform endSize, float duration)
			: base(target, startSize, default, duration)
		{
			SetEndRef(endSize);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenSizeDelta SetExcludeAxis(Axis excludeAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = excludeAxes;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given axes which are not specified by the input are excluded. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="inclAxes">The axes to include. This is a flagged value</param>
		public RaTweenSizeDelta OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<RectTransform, Vector2> DynamicClone()
		{
			RaTweenSizeDelta tween = new RaTweenSizeDelta();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.sizeDelta = ApplyExcludeAxes(target.sizeDelta, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.sizeDelta;
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
		/// Tweens the RectTransform's SizeDelta's Width to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="width">The X Axis SizeDelta to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenSizeDelta TweenSizeWidth(this RectTransform self, float width, float duration)
		{
			return new RaTweenSizeDelta(self, Vector2.one * width, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's SizeDelta's Height to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="height">The Y Axis SizeDelta to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenSizeDelta TweenSizeHeight(this RectTransform self, float height, float duration)
		{
			return new RaTweenSizeDelta(self, Vector2.one * height, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's SizeDelta to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="size">The SizeDelta to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenSizeDelta TweenSize(this RectTransform self, Vector2 size, float duration)
		{
			return new RaTweenSizeDelta(self, size, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's SizeDelta to the given value
		/// </summary>
		/// <param name="startSize">The SizeDelta to tween from</param>
		/// <param name="endSize">The SizeDelta to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenSizeDelta TweenSize(this RectTransform self, Vector2 startSize, Vector2 endSize, float duration)
		{
			return new RaTweenSizeDelta(self, startSize, endSize, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's SizeDelta to the given value
		/// </summary>
		/// <param name="startSize">The SizeDelta to tween from</param>
		/// <param name="endTarget">The reference to which's SizeDelta to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenSizeDelta TweenSize(this RectTransform self, Vector2 startSize, RectTransform endTarget, float duration)
		{
			return new RaTweenSizeDelta(self, startSize, endTarget, duration).Play();
		}
	}

	#endregion
}