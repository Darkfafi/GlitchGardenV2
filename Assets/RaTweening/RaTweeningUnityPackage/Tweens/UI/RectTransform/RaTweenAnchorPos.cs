using RaTweening.UI.RaRectTransform;
using System;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the AnchoredPosition of a RectTransform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenAnchorPos : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenAnchorPos")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenAnchorPos()
			: base()
		{

		}

		public RaTweenAnchorPos(RectTransform target, Vector2 startPos, Vector2 endPos, float duration)
			: base(target, startPos, endPos, duration)
		{

		}

		public RaTweenAnchorPos(RectTransform target, Vector2 endPos, float duration)
			: base(target, endPos, duration)
		{

		}

		public RaTweenAnchorPos(RectTransform target, Vector2 startPos, RectTransform endPos, float duration)
			: base(target, startPos, default, duration)
		{
			SetEndRef(endPos);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenAnchorPos SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenAnchorPos OnlyIncludeAxis(Axis inclAxes)
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
			RaTweenAnchorPos tween = new RaTweenAnchorPos();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.anchoredPosition = ApplyExcludeAxes(target.anchoredPosition, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.anchoredPosition;
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
		/// Tweens the RectTransform's AnchoredPosition's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="posX">The X Axis anchoredPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos TweenAnchorPosX(this RectTransform self, float posX, float duration)
		{
			return new RaTweenAnchorPos(self, Vector2.one * posX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="posY">The Y Axis anchoredPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos TweenAnchorPosY(this RectTransform self, float posY, float duration)
		{
			return new RaTweenAnchorPos(self, Vector2.one * posY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="pos">The anchoredPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos TweenAnchorPos(this RectTransform self, Vector2 pos, float duration)
		{
			return new RaTweenAnchorPos(self, pos, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition to the given value
		/// </summary>
		/// <param name="startPos">The anchoredPosition to tween from</param>
		/// <param name="endPos">The anchoredPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos TweenAnchorPos(this RectTransform self, Vector2 startPos, Vector2 endPos, float duration)
		{
			return new RaTweenAnchorPos(self, startPos, endPos, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition to the given value
		/// </summary>
		/// <param name="startPos">The anchoredPosition to tween from</param>
		/// <param name="endTarget">The reference to which's anchoredPosition to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos TweenAnchorPos(this RectTransform self, Vector2 startPos, RectTransform endTarget, float duration)
		{
			return new RaTweenAnchorPos(self, startPos, endTarget, duration).Play();
		}
	}

	#endregion
}