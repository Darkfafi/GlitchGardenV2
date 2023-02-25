using RaTweening.UI.RaScrollRect;
using UnityEngine;
using static RaTweening.RaVector2Options;
using UnityEngine.UI;

namespace RaTweening.UI.RaScrollRect
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the NormalizedPosition of a ScrollRect
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenScrollPos : RaTweenDynamic<ScrollRect, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenScrollPos")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenScrollPos()
			: base()
		{

		}

		public RaTweenScrollPos(ScrollRect target, Vector2 startNormalizedPos, Vector2 endNormalizedPos, float duration)
			: base(target, startNormalizedPos, endNormalizedPos, duration)
		{

		}

		public RaTweenScrollPos(ScrollRect target, Vector2 endNormalizedPos, float duration)
			: base(target, endNormalizedPos, duration)
		{

		}

		public RaTweenScrollPos(ScrollRect target, Vector2 startNormalizedPos, ScrollRect endNormalizedPos, float duration)
			: base(target, startNormalizedPos, default, duration)
		{
			SetEndRef(endNormalizedPos);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenScrollPos SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenScrollPos OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<ScrollRect, Vector2> DynamicClone()
		{
			RaTweenScrollPos tween = new RaTweenScrollPos();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, ScrollRect target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.normalizedPosition = ApplyExcludeAxes(target.normalizedPosition, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(ScrollRect reference)
		{
			return reference.normalizedPosition;
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
		/// Tweens the ScrollRect's NormalizedPosition's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="normalizedPosX">The X Axis normalizedPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScrollPos TweenScrollPosX(this ScrollRect self, float normalizedPosX, float duration)
		{
			return new RaTweenScrollPos(self, Vector2.one * normalizedPosX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the ScrollRect's NormalizedPosition's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="normalizedPosY">The Y Axis normalizedPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScrollPos TweenScrollPosY(this ScrollRect self, float normalizedPosY, float duration)
		{
			return new RaTweenScrollPos(self, Vector2.one * normalizedPosY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the ScrollRect's NormalizedPosition to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="normalizedPos">The normalizedPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScrollPos TweenScrollPos(this ScrollRect self, Vector2 normalizedPos, float duration)
		{
			return new RaTweenScrollPos(self, normalizedPos, duration).Play();
		}

		/// <summary>
		/// Tweens the ScrollRect's NormalizedPosition to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="startNormalizedPos">The normalizedPosition to tween from</param>
		/// <param name="endNormalizedPos">The normalizedPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScrollPos TweenScrollPos(this ScrollRect self, Vector2 startNormalizedPos, Vector2 endNormalizedPos, float duration)
		{
			return new RaTweenScrollPos(self, startNormalizedPos, endNormalizedPos, duration).Play();
		}

		/// <summary>
		/// Tweens the ScrollRect's NormalizedPosition to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="startNormalizedPos">The normalizedPosition to tween from</param>
		/// <param name="endTarget">The reference to which's normalizedPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScrollPos TweenScrollPos(this ScrollRect self, Vector2 startNormalizedPos, ScrollRect endTarget, float duration)
		{
			return new RaTweenScrollPos(self, startNormalizedPos, endTarget, duration).Play();
		}
	}

	#endregion
}