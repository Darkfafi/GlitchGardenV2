using RaTweening.Core;
using System;
using RaTweening.RaTransform;
using UnityEngine;

namespace RaTweening.RaTransform
{
	/// <summary>
	/// A <see cref="RaTweenShakeBase{TargetT}"/> tween handles the logics of tweening the Shake of the Local Rotation of a Transform
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenShakeRotation : RaTweenShakeBase<Transform>
	{
		public RaTweenShakeRotation()
			: base()
		{

		}
		public RaTweenShakeRotation(Transform target, Vector3 shake, float duration, int vibrato = 10, float randomness = 90f)
			: base(target, shake, duration, vibrato, randomness)
		{
		}

		public RaTweenShakeRotation(Transform target, float shake, float duration, int vibrato = 10, float randomness = 90f, bool ignoreZAxis = true)
			: base(target, shake, duration, vibrato, randomness, ignoreZAxis)
		{
		}

		#region Protected Methods

		protected override Vector3 ReadValue(Transform target)
		{
			return target.localRotation.eulerAngles;
		}

		protected override RaTweenShakeBase<Transform> RaTweenShakeClone()
		{
			return new RaTweenShakeRotation();
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
		/// Tweens a Shake on the Transform's Local Rotation
		/// > Note: Recommended range is between 0 to 90
		/// </summary>
		/// <param name="shake">The Strength the shake will occur in</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="vibrato">This will cause a vibration within the shake, else it will be a linear shake/param>
		/// <param name="randomness">Adds a random offset to the shake (0 to 180). Setting it to 0 will shake along a single direction.</param>
		/// <param name="ignoreZAxis">When True, only applies the shake on the X and Y axis</param>
		public static RaTweenShakeRotation TweenShakeRot(this Transform self, float shake, float duration, int vibrato = 10, float randomness = 90f, bool ignoreZAxis = true)
		{
			return new RaTweenShakeRotation(self, shake, duration, vibrato, randomness, ignoreZAxis).Play();
		}

		/// <summary>
		/// Tweens a Shake on the Transform's Local Rotation
		/// > Note: Recommended range is between 0 to 90
		/// </summary>
		/// <param name="shake">The Diration and Strength the shake will occur in</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="vibrato">This will cause a vibration within the shake, else it will be a linear shake/param>
		/// <param name="randomness">Adds a random offset to the shake (0 to 180). Setting it to 0 will shake along a single direction.</param>
		public static RaTweenShakeRotation TweenShakeRot(this Transform self, Vector3 shake, float duration, int vibrato = 10, float randomness = 90f)
		{
			return new RaTweenShakeRotation(self, shake, duration, vibrato, randomness).Play();
		}
	}

	#endregion
}