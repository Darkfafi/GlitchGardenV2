using RaTweening.UI.RaRectTransform;
using UnityEngine;
using static RaTweening.RaVector3Options;

namespace RaTweening.UI.RaRectTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the AnchoredPosition3D of a RectTransform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenAnchorPos3D : RaTweenDynamic<RectTransform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenAnchorPos3D")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenAnchorPos3D()
			: base()
		{

		}

		public RaTweenAnchorPos3D(RectTransform target, Vector3 startPos, Vector3 endPos, float duration)
			: base(target, startPos, endPos, duration)
		{

		}

		public RaTweenAnchorPos3D(RectTransform target, Vector3 endPos, float duration)
			: base(target, endPos, duration)
		{

		}

		public RaTweenAnchorPos3D(RectTransform target, Vector3 startPos, RectTransform endPos, float duration)
			: base(target, startPos, default, duration)
		{
			SetEndRef(endPos);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenAnchorPos3D SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenAnchorPos3D OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<RectTransform, Vector3> DynamicClone()
		{
			RaTweenAnchorPos3D tween = new RaTweenAnchorPos3D();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, RectTransform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.anchoredPosition3D = ApplyExcludeAxes(target.anchoredPosition3D, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		protected override Vector3 ReadValue(RectTransform reference)
		{
			return reference.anchoredPosition3D;
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
		/// Tweens the RectTransform's AnchoredPosition3D's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="posX">The X Axis anchoredPosition3D to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos3D TweenAnchorPos3DX(this RectTransform self, float posX, float duration)
		{
			return new RaTweenAnchorPos3D(self, Vector3.one * posX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition3D's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="posY">The Y Axis anchoredPosition3D to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos3D TweenAnchorPos3DY(this RectTransform self, float posY, float duration)
		{
			return new RaTweenAnchorPos3D(self, Vector3.one * posY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition3D's Z Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="posZ">The Z Axis anchoredPosition3D to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos3D TweenAnchorPos3DZ(this RectTransform self, float posZ, float duration)
		{
			return new RaTweenAnchorPos3D(self, Vector3.one * posZ, duration)
				.OnlyIncludeAxis(Axis.Z)
				.Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition3D to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="pos">The anchoredPosition3D to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos3D TweenAnchorPos3D(this RectTransform self, Vector3 pos, float duration)
		{
			return new RaTweenAnchorPos3D(self, pos, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition3D to the given value
		/// </summary>
		/// <param name="startPos">The anchoredPosition3D to tween from</param>
		/// <param name="endPos">The anchoredPosition3D to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos3D TweenAnchorPos3D(this RectTransform self, Vector3 startPos, Vector3 endPos, float duration)
		{
			return new RaTweenAnchorPos3D(self, startPos, endPos, duration).Play();
		}

		/// <summary>
		/// Tweens the RectTransform's AnchoredPosition3D to the given value
		/// </summary>
		/// <param name="startPos">The anchoredPosition3D to tween from</param>
		/// <param name="endTarget">The reference to which's anchoredPosition3D to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenAnchorPos3D TweenAnchorPos3D(this RectTransform self, Vector3 startPos, RectTransform endTarget, float duration)
		{
			return new RaTweenAnchorPos3D(self, startPos, endTarget, duration).Play();
		}
	}

	#endregion
}