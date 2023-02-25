using System;
using UnityEngine;
using UnityEngine.Events;
using RaTweening.TimeScale;

namespace RaTweening.Core.Elements
{
	/// <summary>
	/// This is the base class for Elements.
	/// Elements are used to represent serialized tween data for the <see cref="RaTweenerComponent"/>
	/// </summary>
	public abstract class RaTweenerElementBase : MonoBehaviour
	{
		#region Editor Variables

		[SerializeField, HideInInspector]
		private LoopAllowStage _loopingAllowStage = LoopAllowStage.ToInfinity;

		[SerializeField, HideInInspector]
		[Tooltip("The amount of times to repeat the tween.\n A value of 0 makes it play only once without causing a loop.")]
		private int _loops = 0;

		[SerializeField, HideInInspector]
		[Tooltip("When true, this causes the loop to wait for the set Delay value again. Else it will only use the Delay for the initial play.")]
		private bool _includeDelayInLoop = false;

		[SerializeField, HideInInspector]
		[Tooltip("The amount of Seconds to wait until the Tween Starts")]
		private float _delay = 0f;

		[SerializeField, HideInInspector]
		[Tooltip("Triggered when this tween is registered to the Engine (Before Delay)")]
		private UnityEvent _onSetup = null;

		[SerializeField, HideInInspector]
		[Tooltip("Triggered when this tween finishes its Delay")]
		private UnityEvent _onStart = null;

		[SerializeField, HideInInspector]
		[Tooltip("Triggered every time this tween is evaluated")]
		private UnityEvent _onUpdate = null;

		[SerializeField, HideInInspector]
		[Tooltip("Triggered when this tween is Looped (Only when Loops > 0 or Infinite)")]
		private UnityEvent _onLoop = null;

		[SerializeField, HideInInspector]
		[Tooltip("Triggered when this tween is Completed. Meaning it is fully Evaluated and reached its End.")]
		private UnityEvent _onComplete = null;

		[SerializeField, HideInInspector]
		[Tooltip("Triggered when this tween is Ended. This can happen after the Complete trigger or at any time `Stop` is called on this Tween")]
		private UnityEvent _onEnd = null;

		[SerializeField, HideInInspector]
		[Tooltip("Triggered when this tween is Disposed.\n" +
			"This is where garbage of the tween should be cleaned such as objects created during the Setup or other moments of the Tween.")]
		private UnityEvent _onKill = null;

		[SerializeField, HideInInspector]
		private string _overrideName = null;

		// TimeScale
		[SerializeField, HideInInspector]
		private float _timeScale = 1f;

		[SerializeReference, HideInInspector]
		private TimeScaleChannelSO _timeScaleChannel = null;

		#endregion

#if UNITY_EDITOR
		#region Lifecycle

		protected void OnValidate()
		{
			hideFlags = HideFlags.HideInInspector | HideFlags.HideInHierarchy;
		}

		#endregion
#endif
		#region Public Methods

		/// <summary>
		/// Creates a tween with all the tween settings attached set within the element
		/// </summary>
		/// <returns></returns>
		public RaTweenBase CreateTween()
		{
			return CreateCoreTweenInternal()
				.OnSetup(() => _onSetup?.Invoke())
				.OnStart(() => _onStart?.Invoke())
				.OnUpdate(()=> _onUpdate?.Invoke())
				.OnLoop((loopCount) => _onLoop?.Invoke())
				.OnComplete(() => _onComplete?.Invoke())
				.OnEnd(() => _onEnd?.Invoke())
				.OnKill(() => _onKill?.Invoke());
		}

		#endregion

		#region Internal Methods

		internal void CalculateDefaultValuesInternal()
		{
			CalculateDefaultValues();
		}

		internal RaTweenBase CreateCoreTweenInternal()
		{
			return CreateTweenCore()
				.SetDelay(_delay)
				.SetLooping(_loops)
				.SetLoopingIncludesDelay(_includeDelayInLoop)
				.SetTimeScale(_timeScale)
				.SetTimeScaleChannel(_timeScaleChannel);
		}

		internal LoopAllowStage GetLoopAllowStage()
		{
			return _loopingAllowStage;
		}

		internal void SetLoopingAllowStage(LoopAllowStage stage)
		{
			_loopingAllowStage = stage;
		}

		internal void Initialize(Type tweenType)
		{
			Init(tweenType);
		}

		internal string GetName()
		{
			return string.IsNullOrEmpty(_overrideName) ? GetElementName() : _overrideName;
		}

		internal void SetName(string name)
		{
			_overrideName = name;
		}

		#endregion

		#region Protected Methods

		protected abstract void Init(Type tweenType);
		protected abstract string GetElementName();
		protected abstract RaTweenBase CreateTweenCore();
		protected abstract void CalculateDefaultValues();

		#endregion

		#region Enum

		/// <summary>
		/// The loop types to allow within the element
		/// </summary>
		public enum LoopAllowStage
		{
			/// <summary>
			/// Allows for Infinite loops, Finite loops or no loops
			/// </summary>
			ToInfinity,
			/// <summary>
			/// Allows for Finite Loops or no loops
			/// </summary>
			ToFinite,
			/// <summary>
			/// Only allows no loops
			/// </summary>
			None,
		}

		#endregion
	}
}