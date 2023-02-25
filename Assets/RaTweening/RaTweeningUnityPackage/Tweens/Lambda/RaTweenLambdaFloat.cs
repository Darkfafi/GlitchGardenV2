using System;
using RaTweening.Core;
using RaTweening.Lambda;
using static RaTweening.Lambda.RaTweenLambdaBase<float>;

namespace RaTweening.Lambda
{
	/// <summary>
	/// A <see cref="RaTweenLambdaBase{ValueT}"/> tween handles the logics of tweening a Float
	/// > Note: <see cref="RaTweenLambda"/> for all methods
	/// </summary>
	[Serializable]
	public class RaTweenLambdaFloat : RaTweenLambdaBase<float>
	{
		public RaTweenLambdaFloat()
			: base()
		{

		}

		public RaTweenLambdaFloat(float start, float end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
			: base(start, end, duration, setter, isValidHandler)
		{

		}

		#region Protected Methods

		protected override RaTweenLambdaBase<float> CloneLambdaTween()
		{
			return new RaTweenLambdaFloat();
		}

		protected override void LambdaEvaluation(float normalizedValue, float start, float end)
		{
			float delta = (end - start) * normalizedValue;
			WriteValue(start + delta, normalizedValue);
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
	public static partial class RaTweenLambda
	{
		/// <summary>
		/// Tweens a Float Value
		/// </summary>
		/// <param name="start">The Float value to start from</param>
		/// <param name="end">The Float value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaFloat TweenFloat(float start, float end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaFloat(start, end, duration, setter, isValidHandler).Play();
		}
	}
}