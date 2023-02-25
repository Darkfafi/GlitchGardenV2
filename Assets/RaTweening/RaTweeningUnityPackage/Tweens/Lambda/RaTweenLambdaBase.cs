using System;
using UnityEngine;

namespace RaTweening.Lambda
{
	/// <summary>
	/// This class acts as the base class for all Lambda Tweens \n
	/// This tween type can <see cref="SetStartDynamic(bool)"/> to live calculate its start value \n
	/// This tween type can use <see cref="SetEndIsDelta(bool)"/> to live calculate its end value \n
	/// This tween type can use <see cref="SetDynamicSetupStep(RaTweenDynamicSetupStep)"/> to determine when to calculate the <see cref="SetStartDynamic(bool)"/> and <see cref="SetEndIsDelta(bool)"/> values \n
	/// It allows for a Setter and an optional IsValid check method to be passed to it in order to execute the evaluation of a tween. \n
	/// > Note: Lambda Tweens are used to create a tween where you can define the setter logics of the tween yourself in code. (for when you try to do custom logics or the tween does not have a clear target) \n
	/// > Note: Dynamic values don't work well together with modified tween evaluations for they rely heavily on the start and end position being in sequence. \n
	/// </summary>
	[Serializable]
	public abstract class RaTweenLambdaBase<ValueT> : RaTween, IRaTweenLambda
	{
		#region Events

		/// <summary>
		/// The Delegate which passes a value and normalized value to determine the setter logics
		/// > Note: normalizedValue is a value where 0 == Start and 1 == End. This is passed for extra info.
		/// </summary>
		/// <param name="value">The current evaluation value of the tween which should be set</param>
		/// <param name="normalizedValue">Value where 0 == Start and 1 == End. This is passed for extra info.</param>
		public delegate void SetterHandler(ValueT value, float normalizedValue);

		/// <summary>
		/// The Delegate which returns true if the tween is valid, and false if the tween should be terminated (Think of null reference prevention)
		/// </summary>
		public delegate bool IsValidHandler();

		#endregion

		#region Editor Variables

		[Header("Properties")]
		// Start
		[SerializeField]
		[Tooltip("The Value from which to tween from")]
		private ValueT _startValue = default;

		[SerializeField]
		[Tooltip("Dynamic Start will calculate the Start value at runtime duing the defined DynamicSetupStep")]
		private bool _dynamicStart = false;

		// End
		[SerializeField]
		[Tooltip("The Value from which to tween to")]
		private ValueT _endValue = default;

		[SerializeField]
		[Tooltip("End Is Delta will calculate the End value at runtime duing the defined DynamicSetupStep, relative to the Start Value")]
		private bool _endIsDelta = false;

		[SerializeField]
		[Tooltip("The Step when to calculate the dynamic values. When set to `Start`, it recalculates the values on each loop")]
		private RaTweenDynamicSetupStep _dynamicSetupStep = RaTweenDynamicSetupStep.Setup;

		#endregion

		#region Variables

		private SetterHandler _setter = null;
		private IsValidHandler _isValidChecker = null;

		private bool _setInitialStartEnd = false;
		private ValueT _initialStart;
		private ValueT _initialEnd;

		#endregion

		#region Properties

		/// <summary>
		/// The Value to start the Tween at (in which state it will begin)
		/// </summary>
		public ValueT Start
		{
			get; private set;
		}

		/// <summary>
		/// The value to end the Tween to (in which state it will result)
		/// </summary>
		public ValueT End
		{
			get; private set;
		}

		/// <summary>
		/// The value which is recorded by the evaluation of the tween (The current value of the imaginary target)
		/// </summary>
		public ValueT Value
		{
			get; private set;
		}

		public override bool IsValid => _isValidChecker.Invoke();

		#endregion

		public RaTweenLambdaBase()
			: this(default, default, default, default)
		{

		}

		public RaTweenLambdaBase(ValueT start, ValueT end, float duration, SetterHandler setter, IsValidHandler isValidHandler = null)
			: base(duration)
		{
			SetStartValue(start);
			SetEndValue(end);
			_setInitialStartEnd = false;
			_dynamicSetupStep = RaTweenDynamicSetupStep.Setup;

			Value = start;
			_setter = setter;
			_isValidChecker = isValidHandler ?? new IsValidHandler(() => true);
		}



		#region Public Methods

		// API
		/// <summary>
		/// This API call Sets the start value of the tween.
		/// </summary>
		/// <param name="start">The value to tween from</param>
		public RaTweenLambdaBase<ValueT> SetStartValue(ValueT start)
		{
			if(CanBeModified())
			{
				_startValue = start;
			}
			return this;
		}

		/// <summary>
		/// This API call Sets the end value of the tween.
		/// </summary>
		/// <param name="end">The value to tween to</param>
		public RaTweenLambdaBase<ValueT> SetEndValue(ValueT end)
		{
			if(CanBeModified())
			{
				_endValue = end;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the value set within the end value or reference is calculated as delta at runtime
		/// So an end value of 5 while the target is at value 20 would result in a final end value of 25. 
		/// <see cref="SetDynamicSetupStep(RaTweenDynamicSetupStep)"/> to see when this value is calculated
		/// </summary>
		/// <param name="enabled">true to enable this feature, false to disable it</param>
		public RaTweenLambdaBase<ValueT> SetEndIsDelta(bool enabled = true)
		{
			if(CanBeModified())
			{
				_endIsDelta = enabled;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the value set within the start value or reference is ignored and rather the value at which the object is at calculation time has is used as start value.
		/// <see cref="SetDynamicSetupStep(RaTweenDynamicSetupStep)"/> to see when this value is calculated
		/// </summary>
		/// <param name="enabled">true to enable this feature, false to disable it</param>
		public RaTweenLambdaBase<ValueT> SetStartDynamic(bool enabled = true)
		{
			if(CanBeModified())
			{
				_dynamicStart = enabled;
			}
			return this;
		}

		/// <summary>
		/// Determines when the <see cref="SetStartDynamic(bool)"/> and the <see cref="SetEndIsDelta(bool)"/> are calculated
		/// Read <see cref="RaTweenDynamicSetupStep"/> for the details on the calculation steps
		/// > Note: When set to <see cref="RaTweenDynamicSetupStep.Start"/>, it recalculates the values on each loop
		/// </summary>
		/// <param name="step">Step on which to calculate dynamic data</param>
		public RaTweenLambdaBase<ValueT> SetDynamicSetupStep(RaTweenDynamicSetupStep step)
		{
			if(CanBeModified())
			{
				_dynamicSetupStep = step;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void OnSetup()
		{
			base.OnSetup();

			Value = _startValue;

			if(_dynamicSetupStep == RaTweenDynamicSetupStep.Setup)
			{
				PerformDynamicSetup();
			}
		}

		protected override void OnStart()
		{
			base.OnStart();

			if(_dynamicSetupStep == RaTweenDynamicSetupStep.Start)
			{
				PerformDynamicSetup();
			}
		}

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Value);
			SetEndValue(default);
		}

		protected override void ApplyRewind(bool resumeAfterRewind)
		{
			Start = _initialStart;
			End = _initialEnd;
			Value = Start;

			base.ApplyRewind(resumeAfterRewind);
		}

		protected override void Evaluate(float normalizedValue)
		{
			LambdaEvaluation(normalizedValue, Start, End);
		}

		protected override RaTween RaTweenClone()
		{
			RaTweenLambdaBase<ValueT> clone = CloneLambdaTween();

			// Generic
			clone._dynamicSetupStep = _dynamicSetupStep;

			// Start
			clone._startValue = _startValue;
			clone._dynamicStart = _dynamicStart;

			// End
			clone._endValue = _endValue;
			clone._endIsDelta = _endIsDelta;

			// External Methods
			clone._setter = _setter;
			clone._isValidChecker = _isValidChecker;

			return clone;
		}

		protected override void Dispose()
		{
			_setter = null;
			_isValidChecker = null;
			Value = default;

			_endValue = default;
			_startValue = default;

			_initialStart = default;
			_initialEnd = default;

			Start = default;
			End = default;

			_setInitialStartEnd = false;
		}

		protected void WriteValue(ValueT value, float normalizedValue)
		{
			Value = value;
			_setter.Invoke(value, normalizedValue);
		}

		protected abstract RaTweenLambdaBase<ValueT> CloneLambdaTween();
		protected abstract ValueT GetEndByDelta(ValueT start, ValueT delta);
		protected abstract void LambdaEvaluation(float normalizedValue, ValueT start, ValueT end);

		#endregion

		#region Private Methods

		private void PerformDynamicSetup()
		{
			if(_dynamicStart)
			{
				Start = Value;
			}
			else
			{
				Start = _startValue;
			}

			End = _endIsDelta ? GetEndByDelta(Start, _endValue) : _endValue;

			if(!_setInitialStartEnd)
			{
				_initialStart = Start;
				_initialEnd = End;
				_setInitialStartEnd = true;
			}
		}

		#endregion
	}

	public interface IRaTweenLambda
	{
		
	}
}

namespace RaTweening
{
	/// <summary>
	/// This class contains all the <see cref="Lambda.RaTweenLambdaBase{ValueT}"/> methods to create / play Lambda tweens.
	/// > Note: Lambda Tweens are used to create a tween where you can define the setter logics of the tween yourself in code. (for when you try to do custom logics or the tween does not have a clear target)
	/// </summary>
	public static partial class RaTweenLambda
	{
	
	}
}