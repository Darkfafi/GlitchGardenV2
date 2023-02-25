using RaTweening.Core;
using System;
using UnityEngine;
using RaTweening.RaTransform;

namespace RaTweening.RaTransform
{
	/// <summary>
	/// A <see cref="RaTweenPunchBase{TargetT}"/> tween handles the logics of tweening the Punch of a Scale of a Transform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenPunchScale : RaTweenPunchBase<Transform>
	{
		public RaTweenPunchScale()
			: base()
		{

		}

		public RaTweenPunchScale(Transform target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
			: base(target, punch, duration, vibrato, elasticity)
		{

		}

		#region Protected Methods

		protected override Vector3 ReadValue(Transform target)
		{
			return target.localScale;
		}

		protected override RaTweenPunchBase<Transform> RaTweenPunchClone()
		{
			return new RaTweenPunchScale();
		}

		protected override RaTween CreateSectionTween(Transform target, Vector3 end, float duration)
		{
			return new RaTweenScale(target, end, duration);
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
		/// Tweens a Punch on the Transform's Scale
		/// </summary>
		/// <param name="punch">The direction and strength to apply the punch with</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="vibrato">This will cause a vibration within the punch, else it will be a smooth sine wave punch</param>
		/// <param name="elasticity">Represents how much (0 to 1) the vector will go beyond the starting value when bouncing backwards. 1 creates a full oscillation between the punch direction and the opposite direction, while 0 oscillates only between the punch and the start value.</param>
		public static RaTweenPunchScale TweenPunchScale(this Transform self, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
		{
			return new RaTweenPunchScale(self, punch, duration, vibrato, elasticity).Play();
		}
	}

	#endregion
}