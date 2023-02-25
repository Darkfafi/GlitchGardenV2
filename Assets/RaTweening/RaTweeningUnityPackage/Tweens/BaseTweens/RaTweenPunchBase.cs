using System;
using UnityEngine;

namespace RaTweening.Core
{
	/// <summary>
	/// This is the base class of the tweens that handle the logics of tweening a Target's Punch Value
	/// </summary>
	[Serializable]
	[RaTweenerElement(typeof(Elements.RaTweenerSerializableElement))]
	public abstract class RaTweenPunchBase<TargetT> : RaTweenBase, IRaTweenTarget
		where TargetT : UnityEngine.Object
	{
		#region Consts

		private const int MinIterations = 2;

		#endregion

		#region Editor Variables

		[SerializeField]
		[Tooltip("How many seconds to evaluate from the start to the end value")]
		private float _duration = 1f;

		[SerializeField]
		[Tooltip("The Target to Apply the Tween Evaluation To (modify the values of)")]
		private TargetT _target = default;

		[SerializeField]
		[Tooltip("The Direction and Strength in which the punch will occur.")]
		private Vector3 _punch = Vector3.up;

		[SerializeField]
		[Tooltip("The amount the punch will vibrate.\n" +
			"This will cause a shake within the punch, else it will be a smooth sine wave punch.")]
		private int _vibrato = 10;

		[SerializeField]
		[Range(0f, 1f)]
		[Tooltip("Represents how much (0 to 1) the vector will go beyond the starting value when bouncing backwards. \n" +
			"1 creates a full oscillation between the punch direction and the opposite direction, while 0 oscillates only between the punch and the start value.")]
		private float _elasticity = 1f;

		#endregion

		#region Variables

		private RaTweeningProcessor _processor = new RaTweeningProcessor();
		private float _time = 0f;

		#endregion

		#region Properties

		public override bool IsValid => _target != null;

		#endregion

		public RaTweenPunchBase()
			: this(default, default, default, default, default)
		{

		}

		public RaTweenPunchBase(TargetT target, Vector3 punch, float duration, int vibrato = 10, float elasticity = 1f)
			: base(duration)
		{
			_target = target;
			_punch = punch;
			_vibrato = vibrato;
			_duration = duration;

			SetDuration(duration);
			SetElasticity(elasticity);
		}

		#region Public Methods

		/// <summary>
		/// The Direction and Strength in which the punch will occur. 
		/// </summary>
		/// <param name="punch">The Direction and Strength the punch will occur in</param>
		public RaTweenPunchBase<TargetT> SetPunch(Vector3 punch)
		{
			if(CanBeModified())
			{
				_punch = punch;
			}
			return this;
		}

		/// <summary>
		/// The amount the punch will vibrate.
		/// This will cause a shake within the punch, else it will be a smooth sine wave punch.
		/// </summary>
		/// <param name="vibrato">Vibration of Punch, Default: 10</param>
		public RaTweenPunchBase<TargetT> SetVibrato(int vibrato)
		{
			if(CanBeModified())
			{
				_vibrato = vibrato;
			}
			return this;
		}

		/// <summary>
		/// Represents how much (0 to 1) the vector will go beyond the starting value when bouncing backwards. \n
		/// 1 creates a full oscillation between the punch direction and the opposite direction, while 0 oscillates only between the punch and the start value.
		/// </summary>
		/// <param name="elasticity">Value between 0 and 1 representing elasticity</param>
		public RaTweenPunchBase<TargetT> SetElasticity(float elasticity)
		{
			if(CanBeModified())
			{
				_elasticity = Mathf.Clamp01(elasticity);
			}
			return this;
		}

		public Type GetTargetTypeRaw()
		{
			return typeof(TargetT);
		}

		public void SetTargetRaw(object value)
		{
			if(value is TargetT casted)
			{
				_target = casted;
			}
		}

		#endregion

		#region Protected Methods

		protected override void OnSetup()
		{
			base.OnSetup();
			ClearData();
		}

		protected override void OnStart()
		{
			base.OnStart();
			CalculatePunch();
		}

		protected override void OnLoop()
		{
			base.OnLoop();
			ClearData();
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			_processor?.Complete();
		}

		protected override void ApplyReset(bool resumeAfterReset)
		{
			_processor?.Reset();
		}

		protected override void ApplyRewind(bool inclDelay)
		{
			_processor?.Rewind(inclDelay);
		}

		protected override void ResetData()
		{
			base.ResetData();
			ClearData();
		}

		protected override void OnSetDuration(float duration)
		{
			base.OnSetDuration(duration);
			_duration = duration;
		}

		protected override void SetDefaultValues()
		{
			_punch = Vector3.one;
			_vibrato = 10;
			_elasticity = 1f;
			_duration = 1f;
		}

		protected override void Dispose()
		{
			_target = default;

			_processor.Dispose();
			ClearData();
		}

		protected override void Evaluate(float normalizedValue)
		{
			float newTime = Time;
			float deltaTime = newTime - _time;
			_time = newTime;
			_processor.Step(deltaTime, false);
		}

		protected override float CalculateEvaluation()
		{
			return Progress;
		}

		protected override RaTweenBase CloneSelf()
		{
			RaTweenPunchBase<TargetT> tween = RaTweenPunchClone();

			tween._punch = _punch;
			tween._vibrato = _vibrato;
			tween._target = _target;
			tween._elasticity = _elasticity;

			tween.SetDuration(_duration);

			return tween;
		}


		protected abstract RaTween CreateSectionTween(TargetT target, Vector3 end, float duration);
		protected abstract Vector3 ReadValue(TargetT target);
		protected abstract RaTweenPunchBase<TargetT> RaTweenPunchClone();

		#endregion

		#region Private Methods

		private void CalculatePunch()
		{
			// Setup
			Vector3 start = ReadValue(_target);
			float strength = _punch.magnitude;
			int totalItarations = Mathf.Max((int)(_vibrato * Duration), MinIterations);
			float decayAmount = strength / totalItarations;

			// Fast -> Slow Durations
			float[] tDurations = new float[totalItarations];
			float sum = 0f;
			for(int i = 0; i < totalItarations; i++)
			{
				float iterationPerc = (i + 1) / (float)totalItarations;
				float tDuration = Duration * iterationPerc;
				sum += tDuration;
				tDurations[i] = tDuration;
			}
			float durationScaler = Duration / sum;

			// Create the tween
			RaTweenSequence.EntryData[] sequenceEntries = new RaTweenSequence.EntryData[totalItarations];
			for(int i = 0; i < totalItarations; i++)
			{
				Vector3 tweenDelta;
				float tweenDuration = tDurations[i] * durationScaler;
				if(i < totalItarations - 1)
				{
					if(i == 0)
					{
						tweenDelta = _punch;
					}
					else if(i % 2 != 0)
					{
						tweenDelta = -Vector3.ClampMagnitude(_punch, strength * _elasticity);
					}
					else
					{
						tweenDelta = Vector3.ClampMagnitude(_punch, strength);
					}
					strength -= decayAmount;
				}
				else
				{
					tweenDelta = Vector3.zero;
				}

				sequenceEntries[i] = CreateSectionTween(_target, start + tweenDelta, tweenDuration)
										.SetEasing(RaEasingType.OutSine)
										.ToSequenceEntry();
			}

			// Create Sequence for Punch
			_processor.RegisterTween(new RaTweenSequence(sequenceEntries));
			_processor.Step(0f);
		}

		private void ClearData()
		{
			_time = 0f;
		}

		#endregion
	}
}