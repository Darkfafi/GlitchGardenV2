using RaTweening.Tools;
using System;
using UnityEngine;

namespace RaTweening
{
	/// <summary>
	/// This is the base tween class which is used to define all tweens which tween from one value to another affecting a target \n
	/// This tween type can use a value or reference as start and end value \n
	/// This tween type can <see cref="SetStartDynamic(bool)"/> to live calculate its start value \n
	/// This tween type can use <see cref="SetEndIsDelta(bool)"/> to live calculate its end value \n
	/// This tween type can use <see cref="SetDynamicSetupStep(RaTweenDynamicSetupStep)"/> to determine when to calculate the <see cref="SetStartDynamic(bool)"/> and <see cref="SetEndIsDelta(bool)"/> values \n
	/// This is the main used base class for most of the tweening functionalities \n
	/// > Note: Dynamic values don't work well together with modified tween evaluations for they rely heavily on the start and end position being in sequence. \n
	/// </summary>
	/// <typeparam name="TargetT">The target type to affect and use as reference</typeparam>
	/// <typeparam name="ValueT">The value type to apply to the target</typeparam>
	[Serializable]
	public abstract class RaTweenDynamic<TargetT, ValueT> : RaTween, IRaTweenTarget, IDynamicTween
		where TargetT : UnityEngine.Object
	{
		#region Editor Variables

		[Header("Properties")]
		[Tooltip("The Target to Apply the Tween Evaluation To (modify the values of)")]
		[SerializeField]
		private TargetT _target = default;

		// Start
		[SerializeField]
		[Tooltip("The Target from which to copy its value from as the tweens Start Value at the `dynamicSetupStep`")]
		[ModifierField(new string[] { nameof(_useStartRef), nameof(_dynamicStart) }, new object[] { false, true }, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.Any)]
		private TargetT _startRef = default;

		[SerializeField]
		[Tooltip("The Value from which to tween from")]
		[ModifierField(new string[] { nameof(_useStartRef), nameof(_dynamicStart) }, new object[] { true, true }, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.Any)]
		private ValueT _startValue = default;

		[SerializeField]
		[Tooltip("When true, the Start Value can be based on a target Reference")]
		[ModifierField(nameof(_dynamicStart), true, ModifierFieldAttribute.ModType.DontDraw)]
		private bool _useStartRef = false;

		[SerializeField]
		[Tooltip("Dynamic Start will calculate the Start value at runtime duing the defined DynamicSetupStep")]
		private bool _dynamicStart = false;

		// End
		[SerializeField]
		[Tooltip("The Target from which to copy its value from as the tweens End Value at the `dynamicSetupStep`")]
		[ModifierField(nameof(_useEndRef), false, ModifierFieldAttribute.ModType.DontDraw)]
		private TargetT _endRef = default;
		
		[SerializeField]
		[Tooltip("The Value from which to tween to")]
		[ModifierField(nameof(_useEndRef), true, ModifierFieldAttribute.ModType.DontDraw)]
		private ValueT _endValue = default;

		[SerializeField]
		[Tooltip("When true, the End Value can be based on a target Reference")]
		private bool _useEndRef = false;

		[SerializeField]
		[Tooltip("End Is Delta will calculate the End value at runtime duing the defined DynamicSetupStep, relative to the Start Value")]
		private bool _endIsDelta = false;

		[SerializeField]
		[Tooltip("The Step when to calculate the dynamic values. When set to `Start`, it recalculates the values on each loop")]
		private RaTweenDynamicSetupStep _dynamicSetupStep = RaTweenDynamicSetupStep.Setup;

		#endregion

		#region Properties

		/// <summary>
		/// The Target to apply the value evaluation on
		/// </summary>
		public TargetT Target => _target;

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

		public override bool IsValid => _target != null;

		#endregion

		#region Variables

		private bool _setInitialStartEnd = false;
		private ValueT _initialStart;
		private ValueT _initialEnd;

		#endregion

		public RaTweenDynamic()
			: base()
		{

		}

		public RaTweenDynamic(TargetT target, ValueT start, ValueT end, float duration)
			: base(duration)
		{
			_target = target;
			SetStartValue(start);
			SetEndValue(end);
			_setInitialStartEnd = false;
			_dynamicSetupStep = RaTweenDynamicSetupStep.Setup;
		}

		public RaTweenDynamic(TargetT target, ValueT end, float duration)
			: this(target, default, end, duration)
		{
			SetStartDynamic(true);
		}

		#region Public Methods

		// Raw
		Type IRaTweenTarget.GetTargetTypeRaw()
		{
			return typeof(TargetT);
		}

		void IRaTweenTarget.SetTargetRaw(object value)
		{
			if(value is TargetT target)
			{
				_target = target;
			}
		}

		// API

		/// <summary>
		/// Makes it so the End value set within the tween becomes the start value
		/// And the start value becomes the value currently set within the target
		/// > Note: This method Disables <see cref="SetStartDynamic(bool)"/>
		/// </summary>
		/// <returns></returns>
		public RaTweenDynamic<TargetT, ValueT> From()
		{
			if(CanBeModified())
			{
				_startRef = _endRef;
				_useStartRef = _useEndRef;
				_startValue = _endValue;
				SetEndValue(ReadValue(Target));
				SetStartDynamic(false);
			}
			return this;
		}

		/// <summary>
		/// This API call Sets the start value of the tween.
		/// > Note: This makes it so the <see cref="SetStartRef(TargetT)"/> is cleared. \n
		/// > Note: This makes it so the <see cref="SetStartDynamic(bool)"/> is set to false
		/// </summary>
		/// <param name="start">The value to tween from</param>
		public RaTweenDynamic<TargetT, ValueT> SetStartValue(ValueT start)
		{
			if(CanBeModified())
			{
				_useStartRef = false;
				_startValue = start;
				_dynamicStart = false;
			}
			return this;
		}

		/// <summary>
		/// This API call Sets the start reference of the tween.
		/// The tween will copy the value from the reference to determine its start value at runtime.
		/// <see cref="SetDynamicSetupStep(RaTweenDynamicSetupStep)"/> to see when this value is calculated
		/// > Note: this makes it so the <see cref="SetStartValue(ValueT)"/> is cleared. \n
		/// > Note: This makes it so the <see cref="SetStartDynamic(bool)"/> is set to false
		/// </summary>
		/// <param name="start">The reference to copy the start value from</param>
		public RaTweenDynamic<TargetT, ValueT> SetStartRef(TargetT start)
		{
			if(CanBeModified())
			{
				_useStartRef = true;
				_startRef = start;
				_dynamicStart = false;
			}
			return this;
		}

		/// <summary>
		/// This API call Sets the end value of the tween.
		/// > Note: this makes it so the <see cref="SetEndRef(TargetT)"/> is cleared.
		/// </summary>
		/// <param name="end">The value to tween to</param>
		public RaTweenDynamic<TargetT, ValueT> SetEndValue(ValueT end)
		{
			if(CanBeModified())
			{
				_useEndRef = false;
				_endValue = end;
			}
			return this;
		}

		/// <summary>
		/// This API call Sets the end reference of the tween.
		/// The tween will copy the value from the reference to determine its end value at runtime.
		/// <see cref="SetDynamicSetupStep(RaTweenDynamicSetupStep)"/> to see when this value is calculated
		/// > Note: this makes it so the <see cref="SetEndValue(ValueT)"/> is cleared.
		/// </summary>
		/// <param name="end">The reference to copy the end value from</param>
		public RaTweenDynamic<TargetT, ValueT> SetEndRef(TargetT end)
		{
			if(CanBeModified())
			{
				_useEndRef = true;
				_endRef = end;
			}
			return this;
		}

		/// <summary>
		/// This API call Sets target on which to apply the tween
		/// This target will be tweening from the start value to the end value
		/// </summary>
		/// <param name="target">The target to tween</param>
		/// <returns></returns>
		public RaTweenDynamic<TargetT, ValueT> SetTarget(TargetT target)
		{
			if(CanBeModified())
			{
				_target = target;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the value set within the end value or reference is calculated as delta at runtime
		/// So an end value of 5 while the target is at value 20 would result in a final end value of 25. 
		/// <see cref="SetDynamicSetupStep(RaTweenDynamicSetupStep)"/> to see when this value is calculated
		/// </summary>
		/// <param name="enabled">true to enable this feature, false to disable it</param>
		public RaTweenDynamic<TargetT, ValueT> SetEndIsDelta(bool enabled = true)
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
		public RaTweenDynamic<TargetT, ValueT> SetStartDynamic(bool enabled = true)
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
		public RaTweenDynamic<TargetT, ValueT> SetDynamicSetupStep(RaTweenDynamicSetupStep step)
		{
			if(CanBeModified())
			{
				_dynamicSetupStep = step;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected abstract RaTweenDynamic<TargetT, ValueT> DynamicClone();

		protected abstract ValueT GetEndByDelta(ValueT start, ValueT delta);

		protected abstract ValueT ReadValue(TargetT reference);

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Target != null ? ReadValue(Target) : default);
			SetEndValue(default);
		}

		protected override void ApplyRewind(bool resumeAfterRewind)
		{
			Start = _initialStart;
			End = _initialEnd;
			
			base.ApplyRewind(resumeAfterRewind);
		}

		protected override RaTween RaTweenClone()
		{
			RaTweenDynamic<TargetT, ValueT> clone = DynamicClone();

			// Generic
			clone._target = _target;
			clone._dynamicSetupStep = _dynamicSetupStep;

			// Start
			clone._startValue = _startValue;
			clone._startRef = _startRef;
			clone._useStartRef = _useStartRef;
			clone._dynamicStart = _dynamicStart;

			// End
			clone._endValue = _endValue;
			clone._endRef = _endRef;
			clone._useEndRef = _useEndRef;
			clone._endIsDelta = _endIsDelta;

			return clone;
		}

		protected override void OnSetup()
		{
			base.OnSetup();

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

		protected override void Evaluate(float normalizedValue)
		{
			if(_target != null)
			{
				DynamicEvaluation(normalizedValue, _target, Start, End);
			}
		}

		protected override void Dispose()
		{
			_target = default;

			_endRef = default;
			_endValue = default;

			_startRef = default;
			_startValue = default;

			_initialStart = default;
			_initialEnd = default;

			Start = default;
			End = default;

			_setInitialStartEnd = false;
		}

		protected abstract void DynamicEvaluation(float normalizedValue, TargetT target, ValueT start, ValueT end);

		#endregion

		#region Private Methods

		private void PerformDynamicSetup()
		{
			if(_dynamicStart)
			{
				Start = ReadValue(Target);
			}
			else
			{
				Start = _useStartRef ? ReadValue(_startRef) : _startValue;
			}

			if(_useEndRef)
			{
				End = _endIsDelta ? GetEndByDelta(Start, ReadValue(_endRef)) : ReadValue(_endRef);
			}
			else
			{
				End = _endIsDelta ? GetEndByDelta(Start, _endValue) : _endValue;
			}

			if(!_setInitialStartEnd)
			{
				_initialStart = Start;
				_initialEnd = End;
				_setInitialStartEnd = true;
			}
		}

		#endregion
	}

	/// <summary>
	/// The Calculation Step of Dynamic values
	/// </summary>
	public enum RaTweenDynamicSetupStep
	{
		/// <summary>
		/// Causes the calculation to occur when the Tween is registered. This only happens once.
		/// </summary>
		Setup,
		/// <summary>
		/// Causes the calculation to occur at the start of the Tween (after the Delay). This happens in each cycle of the tween.
		/// > Note: This can be used together with <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> + <see cref="RaTweenDynamic{TargetT, ValueT}.SetEndIsDelta(bool)"/> + <see cref="RaTweenUtilExtensions.SetInfiniteLooping{TweenT}(TweenT)"/> to make a target value go indefinitely upwards in value.
		/// </summary>
		Start
	}

	public interface IDynamicTween
	{
	
	}
}