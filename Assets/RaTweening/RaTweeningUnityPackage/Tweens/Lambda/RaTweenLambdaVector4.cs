using RaTweening.Lambda;
using System;
using UnityEngine;
using static RaTweening.Lambda.RaTweenLambdaBase<UnityEngine.Vector4>;
using static RaTweening.RaVector4Options;

namespace RaTweening.Lambda
{
	/// <summary>
	/// A <see cref="RaTweenLambdaBase{ValueT}"/> tween handles the logics of tweening a Vector4
	/// > Note: <see cref="RaTweenLambda"/> for all methods
	/// </summary>
	[Serializable]
	public class RaTweenLambdaVector4 : RaTweenLambdaBase<Vector4>
	{
		#region Editor Variables

		[Header("RaTweenLambdaVector4")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenLambdaVector4()
			: base()
		{

		}

		public RaTweenLambdaVector4(Vector4 start, Vector4 end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
			: base(start, end, duration, setter, isValidHandler)
		{

		}


		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenLambdaVector4 SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenLambdaVector4 OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenLambdaBase<Vector4> CloneLambdaTween()
		{
			RaTweenLambdaVector4 tween = new RaTweenLambdaVector4();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void LambdaEvaluation(float normalizedValue, Vector4 start, Vector4 end)
		{
			Vector4 delta = (end - start) * normalizedValue;
			WriteValue(ApplyExcludeAxes(Value, start + delta, _excludeAxes), normalizedValue);
		}

		protected override Vector4 GetEndByDelta(Vector4 start, Vector4 delta)
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
		/// Tweens a Vector4's X Axis Value
		/// </summary>
		/// <param name="start">The Vector4 value to start from</param>
		/// <param name="xValue">The Vector4's X Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector4 TweenVector4X(Vector4 start, float xValue, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector4(start, Vector4.one * xValue, duration, setter, isValidHandler).OnlyIncludeAxis(Axis.X).Play();
		}

		/// <summary>
		/// Tweens a Vector4's Y Axis Value
		/// </summary>
		/// <param name="start">The Vector4 value to start from</param>
		/// <param name="yValue">The Vector4's Y Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector4 TweenVector4Y(Vector4 start, float yValue, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector4(start, Vector4.one * yValue, duration, setter, isValidHandler).OnlyIncludeAxis(Axis.Y).Play();
		}

		/// <summary>
		/// Tweens a Vector4's Z Axis Value
		/// </summary>
		/// <param name="start">The Vector4 value to start from</param>
		/// <param name="zValue">The Vector4's Z Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector4 TweenVector4Z(Vector4 start, float zValue, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector4(start, Vector4.one * zValue, duration, setter, isValidHandler).OnlyIncludeAxis(Axis.Z).Play();
		}

		/// <summary>
		/// Tweens a Vector4's W Axis Value
		/// </summary>
		/// <param name="start">The Vector4 value to start from</param>
		/// <param name="wValue">The Vector4's W Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector4 TweenVector4W(Vector4 start, float wValue, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector4(start, Vector4.one * wValue, duration, setter, isValidHandler).OnlyIncludeAxis(Axis.W).Play();
		}

		/// <summary>
		/// Tweens a Vector4 Value
		/// </summary>
		/// <param name="start">The Vector4 value to start from</param>
		/// <param name="end">The Vector4 value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector4 TweenVector4(Vector4 start, Vector4 end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector4(start, end, duration, setter, isValidHandler).Play();
		}
	}
}