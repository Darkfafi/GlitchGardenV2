using System;
using RaTweening.Core;
using RaTweening.Lambda;
using UnityEngine;
using static RaTweening.Lambda.RaTweenLambdaBase<UnityEngine.Vector3>;
using static RaTweening.RaVector3Options;

namespace RaTweening.Lambda
{
	/// <summary>
	/// A <see cref="RaTweenLambdaBase{ValueT}"/> tween handles the logics of tweening a Vector3
	/// > Note: <see cref="RaTweenLambda"/> for all methods
	/// </summary>
	[Serializable]
	public class RaTweenLambdaVector3 : RaTweenLambdaBase<Vector3>
	{
		#region Editor Variables

		[Header("RaTweenLambdaVector3")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenLambdaVector3()
			: base()
		{

		}

		public RaTweenLambdaVector3(Vector3 start, Vector3 end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
			: base(start, end, duration, setter, isValidHandler)
		{

		}


		#region Public Methods

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenLambdaVector3 SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenLambdaVector3 OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenLambdaBase<Vector3> CloneLambdaTween()
		{
			RaTweenLambdaVector3 tween = new RaTweenLambdaVector3();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void LambdaEvaluation(float normalizedValue, Vector3 start, Vector3 end)
		{
			Vector3 delta = (end - start) * normalizedValue;
			WriteValue(ApplyExcludeAxes(Value, start + delta, _excludeAxes), normalizedValue);
		}

		protected override Vector3 GetEndByDelta(Vector3 start, Vector3 delta)
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
		/// Tweens a Vector3's X Axis Value
		/// </summary>
		/// <param name="start">The Vector3 value to start from</param>
		/// <param name="xValue">The Vector3's X Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector3 TweenVector3X(Vector3 start, float xValue, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector3(start, Vector3.one * xValue, duration, setter, isValidHandler).OnlyIncludeAxis(Axis.X).Play();
		}

		/// <summary>
		/// Tweens a Vector3's Y Axis Value
		/// </summary>
		/// <param name="start">The Vector3 value to start from</param>
		/// <param name="yValue">The Vector3's Y Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector3 TweenVector3Y(Vector3 start, float yValue, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector3(start, Vector3.one * yValue, duration, setter, isValidHandler).OnlyIncludeAxis(Axis.Y).Play();
		}

		/// <summary>
		/// Tweens a Vector3's Z Axis Value
		/// </summary>
		/// <param name="start">The Vector3 value to start from</param>
		/// <param name="zValue">The Vector3's Z Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector3 TweenVector3Z(Vector3 start, float zValue, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector3(start, Vector3.one * zValue, duration, setter, isValidHandler).OnlyIncludeAxis(Axis.Z).Play();
		}

		/// <summary>
		/// Tweens a Vector3 Value
		/// </summary>
		/// <param name="start">The Vector3 value to start from</param>
		/// <param name="end">The Vector3 value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		/// <param name="setter">The Setter Callback which can be used to apply the tween value to a target</param>
		/// <param name="isValidHandler">An optional IsValid check, which should return true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)</param>
		public static RaTweenLambdaVector3 TweenVector3(Vector3 start, Vector3 end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
		{
			return new RaTweenLambdaVector3(start, end, duration, setter, isValidHandler).Play();
		}
	}
}