using System;
using RaTweening.Core;
using RaTweening.Lambda;
using UnityEngine;
using static RaTweening.Lambda.RaTweenLambdaBase<UnityEngine.Color>;
using static RaTweening.RaColorOptions;

namespace RaTweening.Lambda
{
	/// <summary>
	/// A <see cref="RaTweenLambdaBase{ValueT}"/> tween handles the logics of tweening a Color
	/// > Note: <see cref="RaTweenLambda"/> for all methods
	/// </summary>
	[Serializable]
	public class RaTweenLambdaColor : RaTweenLambdaBase<Color>
	{
		#region Editor Variables

		[SerializeField]
		[Tooltip("Which Channels of the Color Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Channels")]
		private Channel _excludeChannels = Channel.None;

		#endregion

		public RaTweenLambdaColor()
			: base()
		{

		}

		public RaTweenLambdaColor(Color start, Color end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
			: base(start, end, duration, setter, isValidHandler)
		{

		}


		#region Public Methods

		/// <summary>
		/// Makes it so the given color channels are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeChannels">The channels to exclude. This is a flagged value</param>
		public RaTweenLambdaColor SetExcludeChannels(Channel excludeChannels)
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
		public RaTweenLambdaColor OnlyIncludeChannels(Channel inclChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = GetOnlyIncludeChannels(inclChannels);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenLambdaBase<Color> CloneLambdaTween()
		{
			RaTweenLambdaColor tween = new RaTweenLambdaColor();
			tween._excludeChannels = _excludeChannels;
			return tween;
		}

		protected override void LambdaEvaluation(float normalizedValue, Color start, Color end)
		{
			Color delta = (end - start) * normalizedValue;
			WriteValue(start + delta, normalizedValue);
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
	public static partial class RaTweenLambda
	{
		/// <summary>
		/// Tweens the Color's Red Channel Value
		/// </summary>
		/// <param name="start">The Color value to start from</param>
		/// <param name="red">The red value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaColor TweenColorR(Color start, float red, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaColor(start, Color.white * red, duration, setter, isValidHandler).OnlyIncludeChannels(Channel.R).Play();
		}

		/// <summary>
		/// Tweens the Color's Green Channel Value
		/// </summary>
		/// <param name="start">The Color value to start from</param>
		/// <param name="green">The green value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaColor TweenColorG(Color start, float green, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaColor(start, Color.white * green, duration, setter, isValidHandler).OnlyIncludeChannels(Channel.G).Play();
		}


		/// <summary>
		/// Tweens the Color's Blue Channel Value
		/// </summary>
		/// <param name="start">The Color value to start from</param>
		/// <param name="blue">The blue value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaColor TweenColorB(Color start, float blue, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaColor(start, Color.white * blue, duration, setter, isValidHandler).OnlyIncludeChannels(Channel.B).Play();
		}

		/// <summary>
		/// Tweens the Color's Alpha Channel Value
		/// </summary>
		/// <param name="start">The Color value to start from</param>
		/// <param name="alpha">The alpha value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaColor TweenColorA(Color start, float alpha, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaColor(start, Color.white * alpha, duration, setter, isValidHandler).OnlyIncludeChannels(Channel.A).Play();
		}

		/// <summary>
		/// Tweens a Color Value
		/// </summary>
		/// <param name="start">The Color value to start from</param>
		/// <param name="end">The Color value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaColor TweenColor(Color start, Color end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaColor(start, end, duration, setter, isValidHandler).Play();
		}
	}
}