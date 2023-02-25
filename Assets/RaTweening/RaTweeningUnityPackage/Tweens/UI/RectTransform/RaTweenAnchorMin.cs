using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaRectTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the AnchorMin of a RectTransform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenAnchorMin : RaTweenDynamic<RectTransform, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenAnchorMin")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenAnchorMin()
			: base()
		{

		}

		public RaTweenAnchorMin(RectTransform target, Vector2 startMin, Vector2 endMin, float duration)
			: base(target, startMin, endMin, duration)
		{

		}

		public RaTweenAnchorMin(RectTransform target, Vector2 endMin, float duration)
			: base(target, endMin, duration)
		{

		}

		public RaTweenAnchorMin(RectTransform target, Vector2 startMin, RectTransform endMin, float duration)
			: base(target, startMin, default, duration)
		{
			SetEndRef(endMin);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenAnchorMin SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenAnchorMin OnlyIncludeAxis(Axis inclAxes)
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
			RaTweenAnchorMin tween = new RaTweenAnchorMin();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.anchorMin = ApplyExcludeAxes(target.anchorMin, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(RectTransform reference)
		{
			return reference.anchorMin;
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
		/// Tweens the RectTransform's AnchorMin's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="minX">The X Axis anchorMin to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMin TweenAnchorMinX(this RectTransform self, float minX, float duration)
		{
			return new RaTweenAnchorMin(self, Vector2.one * minX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchorMin's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="minY">The Y Axis anchorMin to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMin TweenAnchorMinY(this RectTransform self, float minY, float duration)
		{
			return new RaTweenAnchorMin(self, Vector2.one * minY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchorMin to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="min">The anchorMin to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMin TweenAnchorMin(this RectTransform self, Vector2 min, float duration)
		{
			return new RaTweenAnchorMin(self, min, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchorMin to the given value
		/// </summary>
		/// <param name="startMin">The anchorMin to tween from</param>
		/// <param name="endMin">The anchorMin to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMin TweenAnchorMin(this RectTransform self, Vector2 startMin, Vector2 endMin, float duration)
		{
			return new RaTweenAnchorMin(self, startMin, endMin, duration).Play();
		}


		/// <summary>
		/// Tweens the RectTransform's AnchorMin to the given value
		/// </summary>
		/// <param name="startMin">The anchorMin to tween from</param>
		/// <param name="endTarget">The reference to which's anchorMin to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorMin TweenAnchorMin(this RectTransform self, Vector2 startMin, RectTransform endTarget, float duration)
		{
			return new RaTweenAnchorMin(self, startMin, endTarget, duration).Play();
		}
	}

	#endregion
}