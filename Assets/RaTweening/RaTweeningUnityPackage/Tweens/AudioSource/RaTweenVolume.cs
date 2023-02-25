using RaTweening.RaAudioSource;
using System;
using UnityEngine;

namespace RaTweening.RaAudioSource
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Volume of a AudioSource
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenVolume : RaTweenDynamic<AudioSource, float>
	{
		public RaTweenVolume()
			: base()
		{

		}

		public RaTweenVolume(AudioSource target, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{

		}

		public RaTweenVolume(AudioSource target, float endValue, float duration)
			: base(target, endValue, duration)
		{

		}

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(0f);
			SetEndValue(1f);
		}

		protected override void DynamicEvaluation(float normalizedValue, AudioSource target, float start, float end)
		{
			float delta = end - start;
			target.volume = start + (delta * normalizedValue);
		}

		protected override RaTweenDynamic<AudioSource, float> DynamicClone()
		{
			RaTweenVolume tween = new RaTweenVolume();
			return tween;
		}

		protected override float ReadValue(AudioSource reference)
		{
			return reference.volume;
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
		/// Tweens the AudioSource's Volume.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenVolume TweenVolume(this AudioSource self, float value, float duration)
		{
			return new RaTweenVolume(self, value, duration).Play();
		}

		/// <summary>
		/// Tweens the AudioSource's Volume.
		/// </summary>
		/// <param name="startValue">The volume to tween from (value between 0 - 1)</param>
		/// <param name="endValue">The volume to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenVolume TweenVolume(this AudioSource self, float startValue, float endValue, float duration)
		{
			return new RaTweenVolume(self, startValue, endValue, duration).Play();
		}

		/// <summary>
		/// Tweens the AudioSource's Volume.
		/// </summary>
		/// <param name="startValue">The volume to tween from (value between 0 - 1)</param>
		/// <param name="endTarget">The reference to which's volume to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenVolume TweenVolume(this AudioSource self, float startValue, AudioSource endTarget, float duration)
		{
			RaTweenVolume tween = new RaTweenVolume(self, startValue, default, duration);
			tween.SetEndRef(endTarget);
			return tween.Play();
		}
	}

	#endregion
}