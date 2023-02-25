using RaTweening.RaAudioSource;
using System;
using UnityEngine;

namespace RaTweening.RaAudioSource
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Pitch of a AudioSource
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenPitch : RaTweenDynamic<AudioSource, float>
	{
		public RaTweenPitch()
			: base()
		{

		}

		public RaTweenPitch(AudioSource target, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{

		}

		public RaTweenPitch(AudioSource target, float endValue, float duration)
			: base(target, endValue, duration)
		{

		}

		#region Protected Methods

		protected override void DynamicEvaluation(float normalizedValue, AudioSource target, float start, float end)
		{
			float delta = end - start;
			target.pitch = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<AudioSource, float> DynamicClone()
		{
			RaTweenPitch tween = new RaTweenPitch();
			return tween;
		}

		protected override float ReadValue(AudioSource reference)
		{
			return reference.pitch;
		}

		protected override float GetEndByDelta(float start, float delta)
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
		/// Tweens the AudioSource's Pitch.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPitch TweenPitch(this AudioSource self, float value, float duration)
		{
			return new RaTweenPitch(self, value, duration).Play();
		}

		/// <summary>
		/// Tweens the AudioSource's Pitch.
		/// </summary>
		/// <param name="startValue">The pitch to tween from</param>
		/// <param name="endValue">The pitch to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPitch TweenPitch(this AudioSource self, float startValue, float endValue, float duration)
		{
			return new RaTweenPitch(self, startValue, endValue, duration).Play();
		}

		/// <summary>
		/// Tweens the AudioSource's Pitch.
		/// </summary>
		/// <param name="startValue">The pitch to tween from</param>
		/// <param name="endTarget">The reference to which's pitch to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenPitch TweenPitch(this AudioSource self, float startValue, AudioSource endTarget, float duration)
		{
			RaTweenPitch tween = new RaTweenPitch(self, startValue, default, duration);
			tween.SetEndRef(endTarget);
			return tween.Play();
		}
	}

	#endregion
}