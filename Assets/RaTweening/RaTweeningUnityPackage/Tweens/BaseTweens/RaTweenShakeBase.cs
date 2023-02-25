using System;
using RaTweening.Tools;
using UnityEngine;

namespace RaTweening.Core
{
	/// <summary>
	/// This is the base class of the tweens that handle the logics of tweening a Target's Shake Value
	/// </summary>
	[Serializable]
	[RaTweenerElement(typeof(Elements.RaTweenerSerializableElement))]
	public abstract class RaTweenShakeBase<TargetT> : RaTweenBase, IRaTweenTarget
		where TargetT : UnityEngine.Object
	{
		#region Consts

		private const int MinIterations = 4;

		#endregion

		#region Editor Variables

		[SerializeField]
		[Tooltip("How many seconds to evaluate from the start to the end value")]
		private float _duration = 1f;

		[SerializeField]
		[Tooltip("The Target to Apply the Tween Evaluation To (modify the values of)")]
		private TargetT _target = default;

		[SerializeField]
		[Tooltip("The Direction and Strength in which the shake will occur.")]
		[ModifierField(nameof(_vectorBased), false, ModifierFieldAttribute.ModType.DontDraw)]
		private Vector3 _shake = Vector3.one;

		[SerializeField]
		[Tooltip("The Strength in which the shake will occur in all Axes" +
			"Recommended to set `ignoreZAxis` to True for cameras shaking.")]
		[ModifierField(nameof(_vectorBased), true, ModifierFieldAttribute.ModType.DontDraw)]
		private float _shakeValue = 1f;

		[SerializeField]
		[Tooltip("When true, only applies the shake on the X and Y axis\n" +
			"Recommended to set to True for cameras shaking.")]
		[ModifierField(nameof(_vectorBased), true, ModifierFieldAttribute.ModType.DontDraw)]
		private bool _ignoreZAxis = true;

		[SerializeField]
		[Tooltip("When true, the shake is based on the given Vector, else it will be based on the given Value and allows for `ignoreZAxis`.\n" +
			"The `ignoreZAxis` is recommended for Camera Shaking")]
		private bool _vectorBased = false;

		[SerializeField]
		[Tooltip("The amount the shake will vibrate. \n"+
			"This will cause a vibration within the shake, else it will be a linear shake")]
		private int _vibrato = 10;

		[SerializeField]
		[Tooltip("Adds a random offset to the shake (0 to 180). Setting it to 0 will shake along a single direction.\n" +
			"Recommended range is between 0 to 90")]
		private float _randomness = 90f;

		#endregion

		#region Variables

		private RaTweeningProcessor _processor = new RaTweeningProcessor();
		private float _time = 0f;

		#endregion

		#region Properties

		public override bool IsValid => _target != null;

		#endregion

		public RaTweenShakeBase()
			: base(0f)
		{
			SetShake(_shake);
			SetDuration(_duration);
			SetRandomness(_randomness);
		}

		public RaTweenShakeBase(TargetT target, Vector3 shake, float duration, int vibrato = 10, float randomness = 90f)
			: base(duration)
		{
			_target = target;
			_vibrato = vibrato;
			_duration = duration;

			SetShake(shake);
			SetDuration(duration);
			SetRandomness(randomness);
		}

		public RaTweenShakeBase(TargetT target, float shake, float duration, int vibrato = 10, float randomness = 90f, bool ignoreZAxis = true)
			: this(target, default, duration, vibrato, randomness)
		{
			SetShake(shake, ignoreZAxis);
		}

		#region Public Methods

		/// <summary>
		/// The Direction and Strength in which the shake will occur. 
		/// > Note: This overrides the Value Shake set within <see cref="SetShake(float)"/>
		/// </summary>
		/// <param name="shake">The Direction and Strength the shake will occur in</param>
		public RaTweenShakeBase<TargetT> SetShake(Vector3 shake)
		{
			if(CanBeModified())
			{
				_shake = shake;
				_vectorBased = true;
			}
			return this;
		}

		/// <summary>
		/// The Strength in which the shake will occur. 
		/// > Note: This overrides the Directional Shake set within <see cref="SetShake(Vector3)"/> \n
		/// > Note: Recommended to set ignoreZAxis to True for cameras shaking.
		/// </summary>
		/// <param name="shake">The Strength the shake will occur in</param>
		/// <param name="ignoreZAxis">When True, only applies the shake on the X and Y axis</param>
		public RaTweenShakeBase<TargetT> SetShake(float shake, bool ignoreZAxis)
		{
			if(CanBeModified())
			{
				_shakeValue = shake;
				_vectorBased = false;
				_ignoreZAxis = ignoreZAxis;
			}
			return this;
		}

		/// <summary>
		/// The amount the shake will vibrate. \n
		/// This will cause a vibration within the shake, else it will be a linear shake
		/// </summary>
		/// <param name="vibrato">Vibration of Shake, Default: 10</param>
		public RaTweenShakeBase<TargetT> SetVibrato(int vibrato)
		{
			if(CanBeModified())
			{
				_vibrato = vibrato;
			}
			return this;
		}

		/// <summary>
		/// Adds a random offset to the shake (0 to 180). Setting it to 0 will shake along a single direction.
		/// > Note: Recommended range is between 0 to 90
		/// </summary>
		/// <param name="randomness">Value between 0 and 180 representing randomness</param>
		public RaTweenShakeBase<TargetT> SetRandomness(float randomness)
		{
			if(CanBeModified())
			{
				_randomness = Mathf.Clamp(randomness, 0f, 180f);
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
			CalculateShake();
		}

		protected override void OnLoop()
		{
			base.OnLoop();
			ClearData();
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			_processor?.Complete(false);
		}

		protected override void ApplyReset(bool resumeAfterReset)
		{
			_processor?.Reset(resumeAfterReset);
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
			_shake = Vector3.one;
			_vibrato = 10;
			_randomness = 90f;
			_duration = 1f;
			SetShake(1f, true);
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
			RaTweenShakeBase<TargetT> tween = RaTweenShakeClone();

			tween._target = _target;
			tween._shake = _shake;
			tween._ignoreZAxis = _ignoreZAxis;
			tween._vectorBased = _vectorBased;
			tween._shakeValue = _shakeValue;
			tween._vibrato = _vibrato;
			tween._randomness = _randomness;

			tween.SetDuration(_duration);

			return tween;
		}

		protected abstract RaTween CreateSectionTween(TargetT target, Vector3 end, float duration);
		protected abstract Vector3 ReadValue(TargetT target);
		protected abstract RaTweenShakeBase<TargetT> RaTweenShakeClone();

		#endregion

		#region Private Methods

		private void CalculateShake()
		{
			// Setup
			Vector3 start = ReadValue(_target);
			float shakeMagnitude = _vectorBased ? _shake.magnitude : _shakeValue;
			int totalItarations = Mathf.Max((int)(_vibrato * Duration), MinIterations);
			float decay = shakeMagnitude / totalItarations;

			// Fast -> Slow Duration
			float[] tDurations = new float[totalItarations];
			float sum = 0;
			for(int i = 0; i < totalItarations; ++i)
			{
				float iterationPerc = (i + 1) / (float)totalItarations;
				float tDuration = Duration * iterationPerc;
				sum += tDuration;
				tDurations[i] = tDuration;
			}
			float durationScaler = Duration / sum;

			// Create the tween
			float randomAngle = UnityEngine.Random.Range(0f, 360f);
			RaTweenSequence.EntryData[] sequenceEntries = new RaTweenSequence.EntryData[totalItarations];
			for(int i = 0; i < totalItarations; ++i)
			{
				Vector3 tweenDelta;
				float tweenDuration = tDurations[i] * durationScaler;
				if(i < totalItarations - 1)
				{
					if(i > 0)
					{
						randomAngle = randomAngle - 180 + UnityEngine.Random.Range(-_randomness, _randomness);
					}

					if(_vectorBased)
					{
						Quaternion rndQuaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(-_randomness, _randomness), Vector3.up);
						tweenDelta = rndQuaternion * Vector3FromAngle(randomAngle, shakeMagnitude);
						tweenDelta.x = Vector3.ClampMagnitude(tweenDelta, _shake.x).x;
						tweenDelta.y = Vector3.ClampMagnitude(tweenDelta, _shake.y).y;
						tweenDelta.z = Vector3.ClampMagnitude(tweenDelta, _shake.z).z;
						shakeMagnitude -= decay;
						_shake = Vector3.ClampMagnitude(_shake, shakeMagnitude);
					}
					else
					{
						if(_ignoreZAxis)
						{
							tweenDelta = Vector3FromAngle(randomAngle, shakeMagnitude);
						}
						else
						{
							Quaternion rndQuaternion = Quaternion.AngleAxis(UnityEngine.Random.Range(-_randomness, _randomness), Vector3.up);
							tweenDelta = rndQuaternion * Vector3FromAngle(randomAngle, shakeMagnitude);
						}
						shakeMagnitude -= decay;
					}
				}
				else
				{
					tweenDelta = Vector3.zero;
				}

				sequenceEntries[i] = CreateSectionTween(_target, start + tweenDelta, tweenDuration)
										.SetEasing(RaEasingType.Linear)
										.ToSequenceEntry();
			}

			// Create Sequence for Shake
			_processor.RegisterTween(new RaTweenSequence(sequenceEntries));
			_processor.Step(0f);
		}

		private void ClearData()
		{
			_time = 0f;
		}

		private Vector3 Vector3FromAngle(float degrees, float magnitude)
		{
			float radians = degrees * Mathf.Deg2Rad;
			return new Vector3(magnitude * Mathf.Cos(radians), magnitude * Mathf.Sin(radians), 0);
		}

		#endregion
	}
}