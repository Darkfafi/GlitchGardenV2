using System;
using RaTweening.Core;
using RaTweening.Lambda;
using UnityEngine;
using static RaTweening.Lambda.RaTweenLambdaBase<UnityEngine.Rect>;
using static RaTweening.RaRectOptions;

namespace RaTweening.Lambda
{
	/// <summary>
	/// A <see cref="RaTweenLambdaBase{ValueT}"/> tween handles the logics of tweening a Rect
	/// > Note: <see cref="RaTweenLambda"/> for all methods
	/// </summary>
	[Serializable]
	public class RaTweenLambdaRect : RaTweenLambdaBase<Rect>
	{
		#region Editor Variables

		[Header("RaTweenLambdaRect")]
		[SerializeField]
		[Tooltip("Which Values of the Rect to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Values")]
		private RectValue _excludeValues = RectValue.None;

		#endregion

		public RaTweenLambdaRect()
			: base()
		{

		}

		public RaTweenLambdaRect(Rect start, Rect end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
			: base(start, end, duration, setter, isValidHandler)
		{

		}


		#region Public Methods

		/// <summary>
		/// Makes it so the given Rect Values are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeValues">The Rect Values to exclude. This is a flagged value</param>
		public RaTweenLambdaRect SetExcludeValues(RectValue excludeValues)
		{
			if(CanBeModified())
			{
				_excludeValues = excludeValues;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given Rect Values which are not specified by the input are excluded. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="inclValues">The Rect Values to include. This is a flagged value</param>
		public RaTweenLambdaRect OnlyIncludeRectValues(RectValue inclValues)
		{
			if(CanBeModified())
			{
				_excludeValues = GetOnlyIncludeRectValues(inclValues);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenLambdaBase<Rect> CloneLambdaTween()
		{
			RaTweenLambdaRect tween = new RaTweenLambdaRect();
			tween._excludeValues = _excludeValues;
			return tween;
		}

		protected override void LambdaEvaluation(float normalizedValue, Rect start, Rect end)
		{
			Rect delta = new Rect(end.x - start.x, end.y - start.y, end.width - start.width, end.height - start.height);

			delta.x *= normalizedValue;
			delta.y *= normalizedValue;
			delta.width *= normalizedValue;
			delta.height *= normalizedValue;

			WriteValue(ApplyExcludeRectValues(Value, GetEndByDelta(start, delta), _excludeValues), normalizedValue);
		}

		protected override Rect GetEndByDelta(Rect start, Rect delta)
		{
			return new Rect(start.x + delta.x, start.y + delta.y, start.width + delta.width, start.height + delta.height);
		}

		#endregion
	}
}

namespace RaTweening
{
	public static partial class RaTweenLambda
	{
		/// <summary>
		/// Tweens a Rect's X Value
		/// </summary>
		/// <param name="start">The Rect value to start from</param>
		/// <param name="value">The Rect's X value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaRect TweenRectX(Rect start, float value, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			Rect rect = new Rect(value, value, value, value);
			return new RaTweenLambdaRect(start, rect, duration, setter, isValidHandler).OnlyIncludeRectValues(RectValue.X).Play();
		}

		/// <summary>
		/// Tweens a Rect's Y Value
		/// </summary>
		/// <param name="start">The Rect value to start from</param>
		/// <param name="value">The Rect's Y value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaRect TweenRectY(Rect start, float value, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			Rect rect = new Rect(value, value, value, value);
			return new RaTweenLambdaRect(start, rect, duration, setter, isValidHandler).OnlyIncludeRectValues(RectValue.Y).Play();
		}

		/// <summary>
		/// Tweens a Rect's Width Value
		/// </summary>
		/// <param name="start">The Rect value to start from</param>
		/// <param name="width">The Rect's Width value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaRect TweenRectWidth(Rect start, float value, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			Rect rect = new Rect(value, value, value, value);
			return new RaTweenLambdaRect(start, rect, duration, setter, isValidHandler).OnlyIncludeRectValues(RectValue.Width).Play();
		}

		/// <summary>
		/// Tweens a Rect's Height Value
		/// </summary>
		/// <param name="start">The Rect value to start from</param>
		/// <param name="value">The Rect's Height value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaRect TweenRectHeight(Rect start, float value, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			Rect rect = new Rect(value, value, value, value);
			return new RaTweenLambdaRect(start, rect, duration, setter, isValidHandler).OnlyIncludeRectValues(RectValue.Height).Play();
		}

		/// <summary>
		/// Tweens a Rect Value
		/// </summary>
		/// <param name="start">The Rect value to start from</param>
		/// <param name="end">The Rect value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaRect TweenRect(Rect start, Rect end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaRect(start, end, duration, setter, isValidHandler).Play();
		}
	}
}