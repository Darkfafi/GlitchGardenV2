using UnityEngine;
using RaTweening.Core;
using RaTweening.Tools;
using RaTweening.Core.Elements;

namespace RaTweening
{
	/// <summary>
	/// This is the base tween class which is used to define all tweens using easings and easing modifiers
	/// </summary>
	[RaTweenerElement(typeof(RaTweenerSerializableElement))]
	public abstract class RaTween : RaTweenBase
	{
		#region Editor Variables

		[Header("Tween Settings")]
		[SerializeField]
		[Tooltip("The pre-calculated curve with which to evaluate from the start value to the end value")]
		[ModifierField(nameof(_useCurveEasing), true, ModifierFieldAttribute.ModType.DontDraw)]
		private RaEasingType _easingType = RaEasingType.Linear;
		
		[SerializeField]
		[Tooltip("The animation curve with which to evaluate from the start value to the end value")]
		[ModifierField(nameof(_useCurveEasing), false, ModifierFieldAttribute.ModType.DontDraw)]
		private AnimationCurve _curveEasing = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		[Tooltip("How many seconds to evaluate from the start to the end value")]
		[ModifierField(nameof(_useCurveEasingDuration), true, ModifierFieldAttribute.ModType.DontDraw)]
		private float _duration = 1f;

		[SerializeField]
		[Tooltip("Makes it so the duration of the tween is based on the duration of the `curveEasing`")]
		[ModifierField(nameof(_useCurveEasing), false, ModifierFieldAttribute.ModType.DontDraw)]
		private bool _useCurveEasingDuration = false;

		[SerializeField]
		[Tooltip("When true, it allows you to define your own evaluation curve instead of the pre-calculated `easingType`")]
		private bool _useCurveEasing = false;

		[Header("Modifier Settings")]
		[SerializeField]
		[Tooltip("The pre-calculated curve with which to evaluation order is specified. For example, evaluating a tween in reverse")]
		[ModifierField(nameof(_useCurveModifier), true, ModifierFieldAttribute.ModType.DontDraw)]
		private RaModifierType _modifierType = RaModifierType.None;

		[SerializeField]
		[Tooltip("The animation curve with which to evaluation order is specified. For example, evaluating a tween in reverse by setting time 0 to value 1 and time 1 to value 0")]
		[ModifierField(nameof(_useCurveModifier), false, ModifierFieldAttribute.ModType.DontDraw)]
		private AnimationCurve _curveModifier = AnimationCurve.Linear(0f, 0f, 1f, 1f);

		[SerializeField]
		[Tooltip("When true, it allows you to define your own modification curve instead of the pre-calculated `modifierType`")]
		private bool _useCurveModifier = false;

		#endregion

		public RaTween()
			: base(0f)
		{
			SetDuration(_duration);

			if(_useCurveEasing)
			{
				SetEasingAPIInternal(_curveEasing, _useCurveEasingDuration);
			}
			else
			{
				SetEasingAPIInternal(_easingType);
			}

			if(_useCurveModifier)
			{
				SetModifierAPIInternal(_curveModifier);
			}
			else
			{
				SetModifierAPIInternal(_modifierType);
			}
		}

		public RaTween(float duration)
			: base(duration)
		{
			_duration = duration;
			_easingType = RaEasingType.Linear;
			_useCurveEasing = false;
			_useCurveEasingDuration = false;
		}

		#region Internal Methods

		internal void SetEasingAPIInternal(RaEasingType easing)
		{
			if(CanBeModified())
			{
				_easingType = easing;
				_useCurveEasing = false;
				_useCurveEasingDuration = false;
			}
		}

		internal void SetEasingAPIInternal(AnimationCurve easing, bool inclDuration)
		{
			if(CanBeModified())
			{
				_curveEasing = easing ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);
				_useCurveEasing = true;

				if(_useCurveEasingDuration = inclDuration)
				{
					_duration = 0f;
					if(_curveEasing.keys.Length > 0)
					{
						_duration = _curveEasing.keys[_curveEasing.keys.Length - 1].time;
					}

					SetDuration(_duration);
				}
			}
		}

		internal void SetModifierAPIInternal(RaModifierType modifier)
		{
			if(CanBeModified())
			{
				_modifierType = modifier;
				_useCurveModifier = false;
			}
		}

		internal void SetModifierAPIInternal(AnimationCurve modifier)
		{
			if(CanBeModified())
			{
				modifier = modifier ?? AnimationCurve.Linear(0f, 0f, 0f, 0f);
				_curveModifier = modifier;
				_useCurveModifier = true;
			}
		}

		#endregion

		#region Protected Methods

		protected abstract RaTween RaTweenClone();

		protected override RaTweenBase CloneSelf()
		{
			RaTween tween = RaTweenClone();

			// Easing
			tween._easingType = _easingType;
			tween._curveEasing = _curveEasing;
			tween._useCurveEasing = _useCurveEasing;
			tween._useCurveEasingDuration = _useCurveEasingDuration;

			// Modifier
			tween._modifierType = _modifierType;
			tween._curveModifier = _curveModifier;
			tween._useCurveModifier = _useCurveModifier;

			// Generic
			tween.SetDuration(_duration);

			// Post Easing
			if(tween._useCurveEasing)
			{
				tween.SetEasingAPIInternal(_curveEasing, _useCurveEasingDuration);
			}

			return tween;
		}

		protected override void OnSetDuration(float duration)
		{
			_duration = duration;
		}

		protected override void SetDefaultValues()
		{
			_curveModifier = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			_easingType = RaEasingType.Linear;
			_duration = 1f;
		}

		protected override float CalculateEvaluation()
		{
			float modifiedProgress = ApplyEvaluationModifier(Progress);
			if(_useCurveEasing)
			{
				return _curveEasing.Evaluate(_useCurveEasingDuration ? modifiedProgress * Duration : modifiedProgress);
			}
			else
			{
				return RaTweenEasing.Evaluate(_easingType, modifiedProgress);
			}
		}

		protected float ApplyEvaluationModifier(float value)
		{
			if(_useCurveModifier)
			{
				return _curveModifier.Evaluate(value);
			}
			else
			{
				return RaTweenModifier.ApplyModifier(_modifierType, value);
			}
		}

		protected override void ApplyRewind(bool resumeAfterRewind)
		{
			Evaluate(0);
		}

		protected override void ApplyReset(bool inclDelay)
		{

		}

		#endregion
	}

	/// <summary>
	/// This is the class which represents all RaTweenUtilExtensions
	/// This class has been made so various utils can be used on a tween without losing the type reference of the original tween which is being modified.
	/// </summary>
	public static partial class RaTweenUtilExtensions
	{
		/// <summary>
		/// The curve with which to evaluate from the start value to the end value
		/// > Note: Specifies the <see cref="RaEasingType"/>. By default it is <see cref="RaEasingType.Linear"/>
		/// </summary>
		public static TweenT SetEasing<TweenT>(this TweenT self, RaEasingType easing)
			where TweenT : RaTween
		{
			self.SetEasingAPIInternal(easing);
			return self;
		}

		/// <summary>
		/// The curve with which to evaluate from the start value to the end value
		/// > Note: Makes it so the set <see cref="RaEasingType"/> is replaced with an animation curve
		/// </summary>
		/// <param name="inclDuration">when this is set to true, the duration of the animation curve will be used as the duration of the tween. Else the evaluation will only use the curve's 0 to 1 time range</param>
		public static TweenT SetEasing<TweenT>(this TweenT self, AnimationCurve easing, bool inclDuration = false)
			where TweenT : RaTween
		{
			self.SetEasingAPIInternal(easing, inclDuration);
			return self;
		}

		/// <summary>
		/// Sets an evaluation modifier which changes the way the tween is evaluated
		/// For example, the <see cref="RaModifierType.Reverse"/> makes it so the same tween is evaluated in reverse.
		/// </summary>
		public static TweenT SetModifier<TweenT>(this TweenT self, RaModifierType modifier)
			where TweenT : RaTween
		{
			self.SetModifierAPIInternal(modifier);
			return self;
		}

		/// <summary>
		/// Makes it so the set <see cref="RaModifierType"/> is replaced with an animation curve
		/// </summary>
		public static TweenT SetModifier<TweenT>(this TweenT self, AnimationCurve modifier)
			where TweenT : RaTween
		{
			self.SetModifierAPIInternal(modifier);
			return self;
		}
	}
}