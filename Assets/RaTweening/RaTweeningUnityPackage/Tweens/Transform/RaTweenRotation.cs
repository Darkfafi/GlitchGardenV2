using System;
using UnityEngine;
using static RaTweening.RaVector3Options;
using RaTweening.RaTransform;

namespace RaTweening.RaTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Rotation of a Transform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenRotation : RaTweenDynamic<Transform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenRotation")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		[SerializeField]
		private bool _localRotation = false;

		#endregion

		public RaTweenRotation()
			: base()
		{

		}

		public RaTweenRotation(Transform target, Vector3 startRot, Vector3 endRot, float duration)
			: base(target, startRot, endRot, duration)
		{

		}

		public RaTweenRotation(Transform target, Vector3 endRot, float duration)
			: base(target, endRot, duration)
		{

		}

		#region Public Methods

		/// <summary>
		/// Makes it so the start and end rotation represent values in local space rather than world space 
		/// </summary>
		/// <param name="isLocal">When true, this feature is enabled, else it is disabled</param>
		public RaTweenRotation SetLocalRotation(bool isLocal = true)
		{
			if(CanBeModified())
			{
				_localRotation = isLocal;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenRotation SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenRotation OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			WriteValue(target, Quaternion.SlerpUnclamped
			(
				Quaternion.Euler(start),
				Quaternion.Euler(ApplyExcludeAxes(ReadValue(target), end, _excludeAxes)),
				normalizedValue
			));
		}

		protected override RaTweenDynamic<Transform, Vector3> DynamicClone()
		{
			RaTweenRotation tween = new RaTweenRotation();
			tween._excludeAxes = _excludeAxes;
			tween._localRotation = _localRotation;
			return tween;
		}

		protected override Vector3 ReadValue(Transform reference)
		{
			if(_localRotation)
			{
				return reference.localRotation.eulerAngles;
			}
			else
			{
				return reference.rotation.eulerAngles;
			}
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		#endregion

		#region Private Methods

		private void WriteValue(Transform reference, Quaternion value)
		{
			if(_localRotation)
			{
				reference.localRotation = value;
			}
			else
			{
				reference.rotation = value;
			}
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
		/// Tweens the Transform's Rotation's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="rotX">The X Axis rotation to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRotation TweenRotateX(this Transform self, float rotX, float duration)
		{
			return new RaTweenRotation(self, Vector3.one * rotX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Rotation's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="rotY">The Y Axis rotation to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRotation TweenRotateY(this Transform self, float rotY, float duration)
		{
			return new RaTweenRotation(self, Vector3.one * rotY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Rotation's Z Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="rotZ">The Z Axis rotation to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRotation TweenRotateZ(this Transform self, float rotZ, float duration)
		{
			return new RaTweenRotation(self, Vector3.one * rotZ, duration)
				.OnlyIncludeAxis(Axis.Z)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Rotation to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="rot">The rotation to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRotation TweenRotate(this Transform self, Vector3 rot, float duration)
		{
			return new RaTweenRotation(self, rot, duration).Play();
		}

		/// <summary>
		/// Tweens the Transform's Rotation to the given location
		/// </summary>
		/// <param name="startRot">The rotation to tween from</param>
		/// <param name="endRot">The rotation to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRotation TweenRotate(this Transform self, Vector3 startRot, Vector3 endRot, float duration)
		{
			return new RaTweenRotation(self, startRot, endRot, duration).Play();
		}

		/// <summary>
		/// Tweens the Transform's Rotation to the given reference target's rotation
		/// </summary>
		/// <param name="startRot">The rotation to tween from</param>
		/// <param name="endTarget">The reference to which's scale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenScale TweenRotate(this Transform self, Vector3 startRot, Transform endTarget, float duration)
		{
			return new RaTweenScale(self, startRot, endTarget, duration).Play();
		}
	}

	#endregion
}