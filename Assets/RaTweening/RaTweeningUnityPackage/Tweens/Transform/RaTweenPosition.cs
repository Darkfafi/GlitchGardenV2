using System;
using UnityEngine;
using static RaTweening.RaVector3Options;
using RaTweening.RaTransform;

namespace RaTweening.RaTransform
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Position of a Transform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenPosition : RaTweenDynamic<Transform, Vector3>
	{
		#region Editor Variables

		[Header("RaTweenPosition")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		[SerializeField]
		[Tooltip("When true, it makes it so the Start and End values of the tween are relative to the Target's Parent.\n " +
			"So if the parent is on x: 100 and this tween's End is x: 5. Then it's new global End Position would be x: 105")]
		private bool _localPosition = false;

		#endregion

		public RaTweenPosition()
			: base()
		{

		}

		public RaTweenPosition(Transform target, Vector3 startPos, Vector3 endPos, float duration)
			: base(target, startPos, endPos, duration)
		{

		}

		public RaTweenPosition(Transform target, Vector3 endPos, float duration)
			: base(target, endPos, duration)
		{

		}

		public RaTweenPosition(Transform target, Vector3 startPos, Transform endPos, float duration)
		: base(target, startPos, duration)
		{
			SetEndRef(endPos);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the start and end position represent values in local space rather than world space 
		/// </summary>
		/// <param name="isLocal">When true, this feature is enabled, else it is disabled</param>
		public RaTweenPosition SetLocalPosition(bool isLocal = true)
		{
			if(CanBeModified())
			{
				_localPosition = isLocal;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenPosition SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenPosition OnlyIncludeAxis(Axis inclAxes)
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
			RaTweenPosition tween = new RaTweenPosition();
			tween._excludeAxes = _excludeAxes;
			tween._localPosition = _localPosition;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, Transform target, Vector3 start, Vector3 end)
		{
			Vector3 delta = end - start;
			WriteValue(target, ApplyExcludeAxes(ReadValue(target), start + (delta * normalizedValue), _excludeAxes));
		}

		protected override Vector3 ReadValue(Transform reference)
		{
			if(_localPosition)
			{
				return reference.localPosition;
			}
			else
			{
				return reference.position;
			}
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
		{
			return start + delta;
		}

		#endregion

		#region Private Methods

		private void WriteValue(Transform reference, Vector3 value)
		{
			if(_localPosition)
			{
				reference.localPosition = value;
			}
			else
			{
				reference.position = value;
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
		/// Tweens the Transform's Position's X Axis to the given location
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="posX">The X Axis position to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPosition TweenMoveX(this Transform self, float posX, float duration)
		{
			return new RaTweenPosition(self, Vector3.one * posX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Position's Y Axis to the given location
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="posY">The Y Axis position to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPosition TweenMoveY(this Transform self, float posY, float duration)
		{
			return new RaTweenPosition(self, Vector3.one * posY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Position's Z Axis to the given location
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="posZ">The Z Axis position to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPosition TweenMoveZ(this Transform self, float posZ, float duration)
		{
			return new RaTweenPosition(self, Vector3.one * posZ, duration)
				.OnlyIncludeAxis(Axis.Z)
				.Play();
		}

		/// <summary>
		/// Tweens the Transform's Position to the given location
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="pos">The position to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPosition TweenMove(this Transform self, Vector3 pos, float duration)
		{
			return new RaTweenPosition(self, pos, duration).Play();
		}

		/// <summary>
		/// Tweens the Transform's Position to the given location
		/// </summary>
		/// <param name="startPos">The position to tween from</param>
		/// <param name="endPos">The position to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPosition TweenMove(this Transform self, Vector3 startPos, Vector3 endPos, float duration)
		{
			return new RaTweenPosition(self, startPos, endPos, duration).Play();
		}

		/// <summary>
		/// Tweens the Transform's Position to the given reference target's position
		/// </summary>
		/// <param name="startPos">The position to tween from</param>
		/// <param name="endTarget">The reference to which's position to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPosition TweenMove(this Transform self, Vector3 startPos, Transform endTarget, float duration)
		{
			return new RaTweenPosition(self, startPos, endTarget, duration).Play();
		}
	}

	#endregion
}