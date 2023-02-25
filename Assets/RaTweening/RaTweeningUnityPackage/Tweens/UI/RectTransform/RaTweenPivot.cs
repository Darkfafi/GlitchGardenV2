using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Pivot of a RectTransform
	/// > Note: Changing the Pivot at Runtime causes the actual position of the object to change. \n
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenPivot : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenPivot")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenPivot()
			: base()
		{

		}

		public RaTweenPivot(RectTransform target, Vector2 startPivot, Vector2 endPivot, float duration)
			: base(target, startPivot, endPivot, duration)
		{

		}

		public RaTweenPivot(RectTransform target, Vector2 endPivot, float duration)
			: base(target, endPivot, duration)
		{

		}

		public RaTweenPivot(RectTransform target, Vector2 startPivot, RectTransform endPivot, float duration)
			: base(target, startPivot, default, duration)
		{
			SetEndRef(endPivot);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenPivot SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenPivot OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Target != null ? ReadValue(Target) : Vector2.one * 0.5f);
			SetEndValue(Vector2.one * 0.5f);
		}

		protected override RaTweenDynamic<RectTransform, Vector2> DynamicClone()
		{
			RaTweenPivot tween = new RaTweenPivot();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.pivot = ApplyExcludeAxes(target.pivot, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.pivot;
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
		/// Tweens the RectTransform's Pivot's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="pivotX">The X Axis pivot to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPivot TweenPivotX(this RectTransform self, float pivotX, float duration)
		{
			return new RaTweenPivot(self, Vector2.one * pivotX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's Pivot's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="pivotY">The Y Axis pivot to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPivot TweenPivotY(this RectTransform self, float pivotY, float duration)
		{
			return new RaTweenPivot(self, Vector2.one * pivotY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's Pivot to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="pivot">The pivot to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPivot TweenPivot(this RectTransform self, Vector2 pivot, float duration)
		{
			return new RaTweenPivot(self, pivot, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's Pivot to the given value
		/// > Note: Changing the Pivot at Runtime causes the actual position of the object to change. \n
		/// </summary>
		/// <param name="startPivot">The pivot to tween from</param>
		/// <param name="endPivot">The pivot to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPivot TweenPivot(this RectTransform self, Vector2 startPivot, Vector2 endPivot, float duration)
		{
			return new RaTweenPivot(self, startPivot, endPivot, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's Pivot to the given value
		/// > Note: Changing the Pivot at Runtime causes the actual position of the object to change. \n
		/// </summary>
		/// <param name="startPivot">The pivot to tween from</param>
		/// <param name="endTarget">The reference to which's pivot to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPivot TweenPivot(this RectTransform self, Vector2 startPivot, RectTransform endTarget, float duration)
		{
			return new RaTweenPivot(self, startPivot, endTarget, duration).Play();
		}
	}

	#endregion
}