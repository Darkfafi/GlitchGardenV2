using UnityEngine;
using static RaTweening.RaVector3Options;
using RaTweening.RaTransform;

namespace RaTweening.RaTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Scale of a Transform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenScale : RaTweenDynamic<Transform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenScale")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenScale()
			: base()
		{

		}

		public RaTweenScale(Transform target, Vector3 startScale, Vector3 endScale, float duration)
			: base(target, startScale, endScale, duration)
		{

		}

		public RaTweenScale(Transform target, Vector3 endScale, float duration)
			: base(target, endScale, duration)
		{

		}

		public RaTweenScale(Transform target, Vector3 startScale, Transform endScale, float duration)
		   : base(target, startScale, default, duration)
		{
			SetEndRef(endScale);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenScale SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenScale OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<Transform, Vector3> DynamicClone()
		{
			RaTweenScale tween = new RaTweenScale();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Target != null ? ReadValue(Target) : Vector3.one);
			SetEndValue(Vector3.one);
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			target.localScale = ApplyExcludeAxes(target.localScale, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector3 ReadValue(Transform reference)
		{
			return reference.localScale;
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
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
		/// Tweens the Transform's Scale's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scaleX">The X Axis scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenScaleX(this Transform self, float scaleX, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scaleX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Scale's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scaleY">The Y Axis scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenScaleY(this Transform self, float scaleY, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scaleY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Scale's Z Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scaleZ">The Z Axis scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenScaleZ(this Transform self, float scaleZ, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scaleZ, duration)
				.OnlyIncludeAxis(Axis.Z)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Scale to the given value
		/// > Note: Affects all Axes \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scale">The scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenScale(this Transform self, float scale, float duration)
		{
			return new RaTweenScale(self, Vector3.one * scale, duration).Play();
		}

		/// <summary>
		/// Tweens the Transform's Scale to the given location
		/// > Note: Affects all Axes
		/// </summary>
		/// <param name="startScale">The scale to tween from</param>
		/// <param name="endScale">The scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenScale(this Transform self, float startScale, float endScale, float duration)
		{
			return new RaTweenScale(self, Vector3.one * startScale, Vector3.one * endScale, duration).Play();
		}

		/// <summary>
		/// Tweens the Transform's Scale to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scale">The scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenScale(this Transform self, Vector3 scale, float duration)
		{
			return new RaTweenScale(self, scale, duration).Play();
		}


		/// <summary>
		/// Tweens the Transform's Scale to the given location
		/// </summary>
		/// <param name="startScale">The scale to tween from</param>
		/// <param name="endScale">The scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenScale(this Transform self, Vector3 startScale, Vector3 endScale, float duration)
		{
			return new RaTweenScale(self, startScale, endScale, duration).Play();
		}

		/// <summary>
		/// Tweens the Transform's Scale to the given reference target's scale
		/// </summary>
		/// <param name="startScale">The scale to tween from</param>
		/// <param name="endTarget">The reference to which's scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenScale(this Transform self, Vector3 startScale, Transform endTarget, float duration)
		{
			return new RaTweenScale(self, startScale, endTarget, duration).Play();
		}
	}

	#endregion
}