using RaTweening.RaLight;
using System;
using UnityEngine;
using static RaTweening.RaColorOptions;

namespace RaTweening.RaLight
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Color of a Light
	/// > Note: This can also be used for fading logics \n
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenColor : RaTweenDynamic<Light, Color>
	{
		#region Editor Variables

		[Header("RaTweenColor")]
		[SerializeField]
		[Tooltip("Which Channels of the Color Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Channels")]
		private Channel _excludeChannels = Channel.None;

		#endregion

		public RaTweenColor()
			: base()
		{

		}

		public RaTweenColor(Light target, Color startColor, Color endColor, float duration)
			: base(target, startColor, endColor, duration)
		{

		}

		public RaTweenColor(Light target, Color endColor, float duration)
			: base(target, endColor, duration)
		{

		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given color channels are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeChannels">The channels to exclude. This is a flagged value</param>
		public RaTweenColor SetExcludeChannels(Channel excludeChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = excludeChannels;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given color channels which are not specified by the input are excluded. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="inclChannels">The color channels to include. This is a flagged value</param>
		public RaTweenColor OnlyIncludeChannels(Channel inclChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = GetOnlyIncludeChannels(inclChannels);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Target != null ? ReadValue(Target) : Color.white);
			SetEndValue(Color.white);
		}

		protected override void DynamicEvaluation(float normalizedValue, Light target, Color start, Color end)
		{
			Color delta = end - start;
			target.color = ApplyExcludeChannels(target.color, start + (delta * normalizedValue), _excludeChannels);
		}

		protected override RaTweenDynamic<Light, Color> DynamicClone()
		{
			RaTweenColor tween = new RaTweenColor();
			tween._excludeChannels = _excludeChannels;
			return tween;
		}

		protected override Color ReadValue(Light reference)
		{
			return reference.color;
		}

		protected override Color GetEndByDelta(Color start, Color delta)
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
		/// Tweens the Light's Color's Red Channel
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="red">The red value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenColor TweenColorR(this Light self, float red, float duration)
		{
			return new RaTweenColor(self, Color.white * red, duration)
				.OnlyIncludeChannels(Channel.R)
				.Play();
		}

		/// <summary>
		/// Tweens the Light's Color's Green Channel
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="green">The green value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenColor TweenColorG(this Light self, float green, float duration)
		{
			return new RaTweenColor(self, Color.white * green, duration)
				.OnlyIncludeChannels(Channel.G)
				.Play();
		}

		/// <summary>
		/// Tweens the Light's Color's Blue Channel
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="blue">The blue value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenColor TweenColorB(this Light self, float blue, float duration)
		{
			return new RaTweenColor(self, Color.white * blue, duration)
				.OnlyIncludeChannels(Channel.B)
				.Play();
		}

		/// <summary>
		/// Tweens the Light's Color's Alpha Channel. 
		/// > Note: This can be used for Fading. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="alpha">The alpha value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenColor TweenColorA(this Light self, float alpha, float duration)
		{
			return new RaTweenColor(self, Color.white * alpha, duration)
				.OnlyIncludeChannels(Channel.A)
				.Play();
		}

		/// <summary>
		/// Tweens the Light's Color Channel.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="color">The color value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenColor TweenColor(this Light self, Color color, float duration)
		{
			return new RaTweenColor(self, color, duration).Play();
		}

		/// <summary>
		/// Tweens the Light's Color Channel.
		/// </summary>
		/// <param name="startColor">The color value to tween from</param>
		/// <param name="endColor">The color value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenColor TweenColor(this Light self, Color startColor, Color endColor, float duration)
		{
			return new RaTweenColor(self, startColor, endColor, duration).Play();
		}
	}

	#endregion
}