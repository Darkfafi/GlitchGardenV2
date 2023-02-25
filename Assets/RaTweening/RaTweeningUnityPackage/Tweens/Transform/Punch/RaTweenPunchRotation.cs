using RaTweening.Core;
using System;
using UnityEngine;
using RaTweening.RaTransform;

namespace RaTweening.RaTransform
{
	/// <summary>
	/// A <see cref="RaTweenPunchBase{TargetT}"/> tween handles the logics of tweening the Punch of the Local Rotation of a Transform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenPunchRotation : RaTweenPunchBase<Transform>
	{
		public RaTweenPunchRotation()
			: base()
		{

		}

		public RaTweenPunchRotation(Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
			: base(target, punch, duration, vibrato, elasticity)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetPunch(Vector3.forward);
		}

		protected override Vector3 ReadValue(Transform target)
		{
			return target.localRotation.eulerAngles;
		}

		protected override RaTweenPunchBase<Transform> RaTweenPunchClone()
		{
			return new RaTweenPunchRotation();
		}

		protected override RaTween CreateSectionTween(Transform target, Vector3 end, float duration)
		{
			return new RaTweenRotation(target, end, duration)
				.SetLocalRotation();
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
		/// Tweens a Punch on the Transform's Local Rotation
		/// </summary>
		/// <param name="punch">The direction and strength to apply the punch with</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="vibrato">This will cause a vibration within the punch, else it will be a smooth sine wave punch</param>
		/// <param name="elasticity">Represents how much (0 to 1) the vector will go beyond the starting value when bouncing backwards. 1 creates a full oscillation between the punch direction and the opposite direction, while 0 oscillates only between the punch and the start value.</param>
		public static RaTweenPunchRotation TweenPunchRot(this Transform self, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			return new RaTweenPunchRotation(self, punch, duration, vibrato, elasticity).Play();
		}
	}

	#endregion
}