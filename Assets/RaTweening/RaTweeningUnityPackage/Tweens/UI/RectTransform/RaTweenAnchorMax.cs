using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the AnchorMax of a RectTransform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenAnchorMax : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenAnchorMax")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenAnchorMax()
			: base()
		{

		}

		public RaTweenAnchorMax(RectTransform target, Vector2 startMax, Vector2 endMax, float duration)
			: base(target, startMax, endMax, duration)
		{

		}

		public RaTweenAnchorMax(RectTransform target, Vector2 endMax, float duration)
			: base(target, endMax, duration)
		{

		}

		public RaTweenAnchorMax(RectTransform target, Vector2 startMax, RectTransform endMax, float duration)
			: base(target, startMax, default, duration)
		{
			SetEndRef(endMax);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenAnchorMax SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenAnchorMax OnlyIncludeAxis(Axis inclAxes)
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
			RaTweenAnchorMax tween = new RaTweenAnchorMax();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.anchorMax = ApplyExcludeAxes(target.anchorMax, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.anchorMax;
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
		/// Tweens the RectTransform's AnchorMax's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="maxX">The X Axis anchorMax to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMax TweenAnchorMaxX(this RectTransform self, float maxX, float duration)
		{
			return new RaTweenAnchorMax(self, Vector2.one * maxX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchorMax's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="maxX">The Y Axis anchorMax to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMax TweenAnchorMaxY(this RectTransform self, float maxY, float duration)
		{
			return new RaTweenAnchorMax(self, Vector2.one * maxY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchorMax to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="max">The anchorMax to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMax TweenAnchorMax(this RectTransform self, Vector2 max, float duration)
		{
			return new RaTweenAnchorMax(self, max, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchorMax to the given value
		/// </summary>
		/// <param name="startMax">The anchorMax to tween from</param>
		/// <param name="endMax">The anchorMax to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMax TweenAnchorMax(this RectTransform self, Vector2 startMax, Vector2 endMax, float duration)
		{
			return new RaTweenAnchorMax(self, startMax, endMax, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchorMax to the given value
		/// </summary>
		/// <param name="startMax">The anchorMax to tween from</param>
		/// <param name="endTarget">The reference to which's anchorMax to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMax TweenAnchorMax(this RectTransform self, Vector2 startMax, RectTransform endTarget, float duration)
		{
			return new RaTweenAnchorMax(self, startMax, endTarget, duration).Play();
		}
	}

	#endregion
}