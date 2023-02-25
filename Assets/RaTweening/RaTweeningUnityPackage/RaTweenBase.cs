using RaTweening.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using RaTweening.TimeScale;

namespace RaTweening
{
	/// <summary>
	/// The base RaTween class from which all tweens inherit from.
	/// > Note: By Default, a tween runs in RealTime. To change it to GameTime, call <see cref="RaTweenUtilExtensions.SetToGameTime{TweenT}(TweenT)"/>
	/// </summary>
	public abstract class RaTweenBase
	{
		#region Consts

		/// <summary>
		/// The value used to assign and represent Infinity
		/// </summary>
		public const int InfiniteLoopingValue = -1;

		#endregion

		#region Events

		public delegate void CallbackHandler();
		public delegate void LoopCallbackHandler(int loopCount);

		private CallbackHandler _onSetupEvent;
		private CallbackHandler _onUpdateEvent;
		private CallbackHandler _onStartEvent;
		private CallbackHandler _onCompletedEvent;
		private CallbackHandler _onEndedEvent;
		private CallbackHandler _onKillEvent;
		private LoopCallbackHandler _onLoopEvent;

		#endregion

		#region Editor Variables

		[SerializeField, HideInInspector]
		private int _loopsSerialized = 0;

		[SerializeField, HideInInspector]
		private float _delaySerialized = 0f;

		[SerializeField, HideInInspector]
		private bool _setPauseOnEndSerialized = false;

		[SerializeField, HideInInspector]
		private bool _includeDelayInLoopsSerialized = false;

		[SerializeField, HideInInspector]
		private float _timeScaleSerialized = 1f;

		[SerializeReference, HideInInspector]
		private ITimeScaleChannel _timeScaleChannelSerialized = null;

		#endregion

		#region Variables

		// Group Map
		private static Dictionary<object, List<RaTweenBase>> _tweensToGroupMap = new Dictionary<object, List<RaTweenBase>>();

		private bool _submittedGroupSetting = false;

		private RaTweenProgressor _process;
		private RaTweenProgressor _delay;

		// Tracker
		private int _loopCount = 0;

		#endregion

		#region Properties

		/// <summary>
		/// A Scaler which represents at what relative speed the tween evaluates. (1 being normal speed, 2 being double speed etc) \n
		/// > Note: This affects both the Delay as Evaluation speed \n 
		/// > Note: This returns the <see cref="ITimeScaleChannel"/>.<see cref="ITimeScaleChannel.TimeScale"/> * `Tween.TimeScaleValue` (passed with <see cref="RaTweenUtilExtensions.SetTimeScale{TweenT}(TweenT, float)"/>) \n
		/// > Note: If there is no <see cref="ITimeScaleChannel"/> passed to the tween, this will return Tween.TimeScaleValue instead. \n
		/// > Note: This value will never go below 0.
		/// </summary>
		public float TimeScale
		{
			get
			{
				return Mathf.Max((_timeScaleChannelSerialized != null ? _timeScaleChannelSerialized.TimeScale : 1f) * _timeScaleSerialized, 0f);
			}
		}

		/// <summary>
		/// The GroupID Represents the Group in which the Tween was placed within the <see cref="RaTweeningProcessor"/>
		/// </summary>
		public object GroupID
		{
			get; private set;
		}

		/// <summary>
		/// IsValid will return true when the tween is able to apply its changes to the target.
		/// If IsValid returns false, it will be killed automatically when registered to a RaTweenProcessor
		/// </summary>
		public abstract bool IsValid
		{
			get;
		}

		// Delay
		/// <summary>
		/// This value represent the duration of the delay of the tween in seconds.
		/// > Note: No evaluation is applied to the target while the delay is running
		/// </summary>
		public float Delay => _delay.Duration;

		/// <summary>
		/// The Normalized Progress of the delay. 
		/// 0 being at the start
		/// 1 being at the end
		/// </summary>
		public float DelayProgress => _delay.Progress;

		/// <summary>
		/// Returns true when the delay process has been completed (or was empty)
		/// </summary>
		public bool HasNoDelayRemaining => _delay.IsCompleted;

		// Process
		/// <summary>
		/// Returns whether the tween itself is infinite. 
		/// Not to be confused with <see cref="IsInfiniteLoop"/>, which returns if a finite loop is looping infinitely
		/// </summary>
		public bool IsInfinite => _process.IsInfinite;

		/// <summary>
		/// Represents the duration of the actual tween after the delay in seconds
		/// </summary>
		public float Duration => _process.Duration;

		/// <summary>
		/// The Normalized Progress of the delay. 
		/// 0 being at the start
		/// 1 being at the end
		/// </summary>
		public float Progress => _process.Progress;

		/// <summary>
		/// Represents the time in seconds that the tween has progressed.
		/// </summary>
		public float Time => _process.Time;

		// Progress And Delay
		/// <summary>
		/// The Time that has passed in seconds in delay + the actual tween
		/// Delay Time + <see cref="Time"/>
		/// </summary>
		public float TotalTime => _delay.Time + Time;

		/// <summary>
		/// Represents the duration of the delay plus the actual tween in seconds
		/// <see cref="Delay"/> + <see cref="Duration"/>
		/// </summary>
		public float TotalDuration => Delay + Duration;

		/// <summary>
		/// The Normalized Progress of the delay + the actual tween. 
		/// 0 being at the start
		/// 1 being at the end
		/// </summary>
		public float TotalProgress => IsEmpty ? 0f : TotalTime / TotalDuration;

		/// <summary>
		/// Returns true if the delay + actual tween has reached their end
		/// </summary>
		public bool IsTotalCompleted => _process.IsCompleted && HasNoDelayRemaining;

		// Looping
		/// <summary>
		/// Returns true if the tween is set to loop
		/// </summary>
		public bool IsLoop => Loops != 0;

		/// <summary>
		/// Returns true if the tween loops an infinite amount of times (unless manually stopped)
		/// </summary>
		public bool IsInfiniteLoop => Loops == InfiniteLoopingValue;

		/// <summary>
		/// Returns the amount of loops the tween desires to make before marking itself as completed.
		/// > Note: this will return <see cref="InfiniteLoopingValue"/> if <see cref="IsInfiniteLoop"/> is set to true
		/// </summary>
		public int Loops => _loopsSerialized;

		public bool IncludeDelayInLoops => _includeDelayInLoopsSerialized;

		/// <summary>
		/// The Time of <see cref="TotalTime"/> in total of all its Passed Loops. 
		/// Delay Time + <see cref="Time"/> & <see cref="Loops"/>
		/// </summary>
		public float TotalLoopingTime
		{
			get
			{
				if(IncludeDelayInLoops)
				{
					return TotalTime + (_loopCount * TotalDuration);
				}
				else
				{
					return TotalTime + (_loopCount * Duration);
				}
			}
		}

		/// <summary>
		/// Represents the duration of <see cref="TotalDuration"/> in total of Total Loops. 
		/// <see cref="Delay"/> + <see cref="Duration"/> & <see cref="Loops"/>
		/// > Note: this will return <see cref="InfiniteLoopingValue"/> if <see cref="IsInfiniteLoop"/> is set to true
		/// </summary>
		public float TotalLoopingDuration
		{
			get
			{
				if(IsInfiniteLoop)
				{
					return InfiniteLoopingValue;
				}

				if(IncludeDelayInLoops)
				{
					return TotalDuration + (Loops * TotalDuration);
				}
				else
				{
					return TotalDuration + (Loops * Duration);
				}
			}
		}

		/// <summary>
		/// The Normalized value of <see cref="TotalProgress"/> in total of Total Loops.
		/// 0 being at the start
		/// 1 being at the end
		/// <see cref="DelayProgress"/> + <see cref="Progress"/> & <see cref="Loops"/>
		/// > Note: this will return <see cref="InfiniteLoopingValue"/> if <see cref="IsInfiniteLoop"/> is set to true
		/// </summary>
		public float TotalLoopingProgress
		{
			get
			{
				if(IsInfiniteLoop)
				{
					return InfiniteLoopingValue;
				}

				return IsEmpty ? 0f : TotalLoopingTime / TotalLoopingDuration;
			}
		}

		/// <summary>
		/// Returns true if the the current running tween is the last of the loop cycle
		/// Returns true on the first tween when it has no loops
		/// > Note: this will always return false if <see cref="IsInfiniteLoop"/> is set to true
		/// </summary>
		public bool HasReachedLoopEnd
		{
			get
			{
				if(Loops >= 0)
				{
					return _loopCount >= Loops;
				}

				return false;
			}
		}

		// Core
		/// <summary>
		/// When True, this pauses the Tween when it Ends
		/// This prevents it from being Killed by the <see cref="RaTweeningProcessor"/>
		/// </summary>
		public bool PauseOnEnd => _setPauseOnEndSerialized;

		/// <summary>
		///  Returns True if the tween has ended. By going through the natural flow or by calling <see cref="Stop"/>
		/// </summary>
		public bool HasEnded
		{
			get; private set;
		}

		/// <summary>
		/// Returns True if the tween has been Setup. Meaning it triggered the <see cref="OnSetup"/> callback of the tween.
		/// > Note: This happens when a tween is registered to a <see cref="RaTweeningProcessor"/> and the <see cref="State.ToStart"/> state is processed.
		/// </summary>
		public bool IsSetup
		{
			get; private set;
		}

		/// <summary>
		/// Returns True if the tween has transitioned from the <see cref="State.InDelay"/> state to the <see cref="State.InProgress"/> state.
		/// > Note: This resets to False every time the tween is set back in the <see cref="State.InDelay"/> state.
		/// </summary>
		public bool HasStarted
		{
			get; private set;
		}

		/// <summary>
		/// Returns true if <see cref="Delay"/> and <see cref="Duration"/> are both 0
		/// </summary>
		public bool IsEmpty => _delay.IsEmpty && _process.IsEmpty;

		/// <summary>
		/// The current state of the tween.
		/// > Note: A tween can only be manipulated when <see cref="CanBeModified"/> returns true. \n
		/// > Note: API calls on a tween can only be made when <see cref="CanUseAPI"/> returns true. \n
		/// > Note: To check if a tween is playing, please use <see cref="IsPlaying"/> \n
		/// > Note: To check if a tween is paused, please use <see cref="IsPaused"/> \n
		/// > Note: To check if a tween is completed, please use <see cref="IsCompleted"/>
		/// </summary>
		public State TweenState
		{
			get; private set;
		}

		/// <summary>
		/// Returns true if the tween is in its delay or in its active tween evaluation
		/// </summary>
		public bool IsPlaying
		{
			get
			{
				switch(TweenState)
				{
					case State.InDelay:
					case State.InProgress:
						return true;
					default:
						return false;
				}
			}
		}

		/// <summary>
		/// Returns true when a tween is in its paused state
		/// </summary>
		public bool IsPaused
		{
			get
			{
				return TweenState == State.IsPaused;
			}
		}

		/// <summary>
		/// Returns true when a tween has been completed
		/// Meaning fully played or force completed using <see cref="Complete"/>
		/// </summary>
		public bool IsCompleted
		{
			get; private set;
		}

		#endregion

		public RaTweenBase(float duration)
		{
			_delay = new RaTweenProgressor(0f);
			_process = new RaTweenProgressor(0f);

			TweenState = State.None;

			SetDuration(duration);
		}

		#region Public Methods

		/// <summary>
		/// Returns all <see cref="IsAlive(bool)"/> Tweens under a given GroupID set by <see cref="RaTweenUtilExtensions.SetGroup{TweenT}(TweenT, object)"/>
		/// > Note: When the tween is Submitted to a Processor, only then does the tween become part of the GroupID it is assigned to. \n
		/// > Note: A Tween is Submitted to a Processor when it is Registered to the <see cref="RaTweeningEngine"/> (When <see cref="Play{TweenT}(TweenT)"/> is called on it)
		/// </summary>
		/// <param name="inclEnded">When False, this method will NOT return tweens which have ended</param>
		public static RaTweenBase[] GetGroup(object groupID, bool inclEnded)
		{
			if(_tweensToGroupMap.TryGetValue(groupID, out var group))
			{
				return group.Where(x => x.IsAlive(inclEnded)).ToArray();
			}
			return new RaTweenBase[] { };
		}

		/// <summary>
		/// Calls <see cref="Stop"/> on all Tweens under a given GroupID set by <see cref="RaTweenUtilExtensions.SetGroup{TweenT}(TweenT, object)"/>
		/// > Note: When the tween is Submitted to a Processor, only then does the tween become part of the GroupID it is assigned to. \n
		/// > Note: A Tween is Submitted to a Processor when it is Registered to the <see cref="RaTweeningEngine"/> (When <see cref="Play{TweenT}(TweenT)"/> is called on it)
		/// </summary>
		public static void StopGroup(object groupID)
		{
			if(_tweensToGroupMap.TryGetValue(groupID, out var group))
			{
				for(int i = 0, c = group.Count; i < c; i++)
				{
					group[i].Stop();
				}
			}
		}

		/// <summary>
		/// Calls <see cref="Complete"/> on all Tweens under a given GroupID set by <see cref="RaTweenUtilExtensions.SetGroup{TweenT}(TweenT, object)"/>
		/// > Note: When the tween is Submitted to a Processor, only then does the tween become part of the GroupID it is assigned to. \n
		/// > Note: A Tween is Submitted to a Processor when it is Registered to the <see cref="RaTweeningEngine"/> (When <see cref="Play{TweenT}(TweenT)"/> is called on it)
		/// </summary>
		public static void CompleteGroup(object groupID)
		{
			if(_tweensToGroupMap.TryGetValue(groupID, out var group))
			{
				for(int i = 0, c = group.Count; i < c; i++)
				{
					group[i].Complete();
				}
			}
		}

		/// <summary>
		/// Calls <see cref="Pause"/> on all Tweens under a given GroupID set by <see cref="RaTweenUtilExtensions.SetGroup{TweenT}(TweenT, object)"/>
		/// > Note: When the tween is Submitted to a Processor, only then does the tween become part of the GroupID it is assigned to. \n
		/// > Note: A Tween is Submitted to a Processor when it is Registered to the <see cref="RaTweeningEngine"/> (When <see cref="Play{TweenT}(TweenT)"/> is called on it)
		/// </summary>
		public static void PauseGroup(object groupID)
		{
			if(_tweensToGroupMap.TryGetValue(groupID, out var group))
			{
				for(int i = 0, c = group.Count; i < c; i++)
				{
					group[i].Pause();
				}
			}
		}

		/// <summary>
		/// Calls <see cref="Resume"/> on all Tweens under a given GroupID set by <see cref="RaTweenUtilExtensions.SetGroup{TweenT}(TweenT, object)"/>
		/// > Note: When the tween is Submitted to a Processor, only then does the tween become part of the GroupID it is assigned to. \n
		/// > Note: A Tween is Submitted to a Processor when it is Registered to the <see cref="RaTweeningEngine"/> (When <see cref="Play{TweenT}(TweenT)"/> is called on it)
		/// </summary>
		public static void ResumeGroup(object groupID)
		{
			if(_tweensToGroupMap.TryGetValue(groupID, out var group))
			{
				for(int i = 0, c = group.Count; i < c; i++)
				{
					group[i].Resume();
				}
			}
		}

		/// <summary>
		/// Returns true if the tween is a living tween
		/// > Note: A living tween is one which is not <see cref="State.None"/>, <see cref="State.Dead"/> or <see cref="State.Data"/> \n
		/// > Note: A living tween is one which is being processed, to be processed or ended processing by a <see cref="RaTweenSequence"/> or the <see cref="RaTweeningEngine"/>
		/// </summary>
		/// <param name="inclEnded">When False, this method will NOT return tweens which have ended</param>
		public bool IsAlive(bool inclEnded)
		{
			switch(TweenState)
			{
				case State.None:
				case State.Dead:
				case State.Data:
					return false;
			}

			if(!inclEnded && HasEnded)
			{
				return false;
			}

			return true;
		}

		/// <summary>
		/// An API Call which pauses the tween on the next processor step
		/// </summary>
		/// <returns>True if the request to pause the tween was successfully submitted</returns>
		public bool Pause()
		{
			if(CanUseAPI(true))
			{
				SetStateInternal(State.IsPaused);
				return true;
			}
			return false;
		}

		/// <summary>
		/// An API Call which resumes the tween on the next processor step
		/// </summary>
		/// <returns>True if the request to resume the tween was successfully submitted</returns>
		public bool Resume()
		{
			if(CanUseAPI(true) && IsPaused)
			{
				SetStateInternal(State.ToResume);
				return true;
			}
			return false;
		}

		/// <summary>
		/// An API Call which Ends a tween by stopping it abruptly
		/// </summary>
		/// <returns>True if the request to end the tween was successfully submitted</returns>
		public bool Stop()
		{
			if(CanUseAPI(true))
			{
				SetStateInternal(State.Ended);
				return true;
			}
			return false;
		}

		/// <summary>
		/// An API Call which Rewinds the tween and evaluates its first frame
		/// > Note: This will Reset all timers and Refresh internal data \n
		/// > Note: This does not change the <see cref="TweenState"/> 
		/// </summary>
		/// <returns>True if the request to rewind the tween was successfully applied</returns>
		/// <param name="inclDelay">Also rewinds the Delay, else it only rewinds the evaluation time</param>
		public bool Rewind(bool inclDelay = true)
		{
			if(CanUseAPI(true))
			{
				if(inclDelay)
				{
					ResetDelay();
				}

				_process.Reset();
				ResetData();

				ApplyRewind(inclDelay);
				return true;
			}
			return false;
		}


		/// <summary>
		/// An API Call which Resets the Tween
		/// > Note: This will Reset all timers and Refresh internal data \n
		/// > Note: This will automatically put the tween in the <see cref="IsPaused"/> state.
		/// </summary>
		/// <returns>True if the request to reset the tween was successfully applied</returns>
		/// <param name="resumeAfterReset">When True, this will unpause the tween immediately after Reset</param>
		public bool Reset(bool resumeAfterReset = false)
		{
			if(CanUseAPI(true))
			{
				ResetData();

				ResetDelay();
				_process.Reset();

				ApplyReset(resumeAfterReset);

				SetStateInternal(State.IsPaused);

				if(resumeAfterReset)
				{
					Resume();
				}

				return true;
			}
			return false;
		}

		/// <summary>
		/// An API Call which force completes the tween on the next processor step
		/// > Note: It will skip over any callbacks which would happen between the current state of the tween and complete. And only call the Completion callback and all after. \n
		/// > Note: Does not work on a tween that <see cref="HasEnded"/>
		/// </summary>
		/// <returns>True if the request to complete the tween was successfully submitted</returns>
		public bool Complete()
		{
			if(CanUseAPI(false))
			{
				SetStateInternal(State.Completed);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Creates a clone of the tween. Which is a completely separate instance with cleared event hooks.
		/// All other settings such as delay, duration, target and more will be copied over to the new instance
		/// </summary>
		/// <param name="inclEvents">If True, Adds <see cref="RaTweenBase"/> events listeners to the events of the new tween instance</param>
		/// <returns>A new instance this tween</returns>
		public RaTweenBase Clone(bool inclEvents)
		{
			RaTweenBase tween = CloneSelf();
			tween.SetDelayAPIInternal(_delaySerialized);
			tween.SetLoopingAPIInternal(_loopsSerialized);
			tween.SetLoopingIncludesDelayAPIInternal(_includeDelayInLoopsSerialized);
			tween.SetPotentialGroupInternal(GroupID);
			tween.SetPauseOnEndAPIInternal(_setPauseOnEndSerialized);
			if(inclEvents)
			{
				tween._onSetupEvent += _onSetupEvent;
				tween._onStartEvent += _onStartEvent;
				tween._onUpdateEvent += _onUpdateEvent;
				tween._onLoopEvent += _onLoopEvent;
				tween._onCompletedEvent += _onCompletedEvent;
				tween._onEndedEvent += _onEndedEvent;
				tween._onKillEvent += _onKillEvent;
			}
			return tween;
		}

		/// <summary>
		/// Represents when modification calls will be applied to the tween or not.
		/// Modification Calls are methods such as, but not limited to; setting the Delay, Duration, Target, Loop Count, Listen to Events of a tween.
		/// > Note: All modification calls can only be made when the state is <see cref="State.None"/> or <see cref="State.ToStart"/>
		/// </summary>
		/// <returns>When this returns true, modification calls will be applied to the tween</returns>
		public bool CanBeModified()
		{
			switch(TweenState)
			{
				case State.None:
				case State.ToStart:
					return true;
				default:
					return false;
			}
		}

		/// <summary>
		/// Represents when API calls will be made on the tween or not.
		/// API Calls are methods which make request to the tween, such as <see cref="Pause"/>, <see cref="Resume"/>, <see cref="Stop"/>, <see cref="Complete"/>
		/// > Note: All API calls can only be made when the tween <see cref="IsAlive(bool)"/>
		/// </summary>
		/// <returns>When this returns true, API calls will be made on the tween</returns>
		/// <param name="inclEnded">When False, this method will NOT return tweens which have ended</param>
		public bool CanUseAPI(bool inclEnded)
		{
			return IsAlive(inclEnded);
		}



		#endregion

		#region Internal Methods

		internal void SetTimeScaleInternal(float value)
		{
			_timeScaleSerialized = value;
		}

		internal void SetTimeScaleChannelInternal(ITimeScaleChannel channel)
		{
			_timeScaleChannelSerialized = channel;
		}

		internal void SubmitGroupInternal()
		{
			if(!_submittedGroupSetting)
			{
				_submittedGroupSetting = true;

				if(GroupID != null)
				{
					if(!_tweensToGroupMap.TryGetValue(GroupID, out var group))
					{
						_tweensToGroupMap[GroupID] = group = new List<RaTweenBase>();
					}
					group.Add(this);
				}
			}
		}

		internal void RevertGroupInternal()
		{
			if(_submittedGroupSetting)
			{
				_submittedGroupSetting = false;

				if(GroupID != null)
				{
					if(_tweensToGroupMap.TryGetValue(GroupID, out var group))
					{
						group.Remove(this);
						if(group.Count == 0)
						{
							_tweensToGroupMap.Remove(GroupID);
						}
					}
				}
			}
		}

		internal void SetPotentialGroupInternal(object groupID)
		{
			if(CanBeModified())
			{
				if(_submittedGroupSetting)
				{
					RevertGroupInternal();
					GroupID = groupID;
					SubmitGroupInternal();
				}
				else
				{
					GroupID = groupID;
				}
			}
		}

		internal void ClearPotentialGroupInternal()
		{
			if(CanBeModified())
			{
				if(_submittedGroupSetting)
				{
					RevertGroupInternal();
					GroupID = null;
					SubmitGroupInternal();
				}
				else
				{
					GroupID = null;
				}
			}
		}

		internal void SetDefaultValuesInternal()
		{
			SetDefaultValues();
		}

		internal void SetupInternal()
		{
			IsCompleted = false;
			IsSetup = true;
			
			_loopCount = 0;

			ResetDelay();
			_process.Reset();

			OnSetup();
			_onSetupEvent?.Invoke();
		}

		internal void StartInternal()
		{
			HasStarted = true;

			_process.Reset();

			OnStart();
			_onStartEvent?.Invoke();

			PerformEvaluation();
		}

		internal float StepDelayInternal(float deltaTime)
		{
			return _delay.Step(deltaTime);
		}

		internal float StepTweenInternal(float deltaTime)
		{
			float remaining = _process.Step(deltaTime);
			PerformEvaluation();
			return remaining;
		}

		internal void LoopInternal()
		{
			_loopCount++;

			if(IncludeDelayInLoops)
			{
				ResetDelay();
			}

			_process.Reset();
			
			OnLoop();
			_onLoopEvent?.Invoke(_loopCount);
		}

		internal void EndInternal()
		{
			HasEnded = true;
			OnEnd();
			_onEndedEvent?.Invoke();

			if(PauseOnEnd)
			{
				Pause();
			}
		}

		internal void KillInternal()
		{
			OnKill();
			_onKillEvent?.Invoke();

			DisposeInternal();
		}

		internal void DisposeInternal()
		{
			Dispose();

			// Group
			RevertGroupInternal();
			GroupID = null;

			// Events
			_onKillEvent = null;
			_onEndedEvent = null;
			_onCompletedEvent = null;
			_onStartEvent = null;
			_onUpdateEvent = null;
			_onSetupEvent = null;
			_onLoopEvent = null;

			// TimeScale
			_timeScaleChannelSerialized = null;
			_timeScaleSerialized = default;
		}

		internal void CompleteInternal()
		{
			IsCompleted = true;

			_delay.Complete();
			_process.Complete();
			
			OnComplete();

			_onCompletedEvent?.Invoke();
			PerformEvaluation();
		}

		internal void SetStateInternal(State state)
		{
			TweenState = state;
		}

		internal void SetPauseOnEndAPIInternal(bool pauseOnEnd = true)
		{
			if(CanBeModified())
			{
				_setPauseOnEndSerialized = pauseOnEnd;
			}
		}

		internal void SetDelayAPIInternal(float delay)
		{
			if(CanBeModified())
			{
				_delaySerialized = delay;
				_delay.SetDuration(delay);
			}
		}

		internal void SetLoopingAPIInternal(int loopAmount)
		{
			if(CanBeModified())
			{
				_loopsSerialized = loopAmount;
			}
		}

		internal void SetLoopingIncludesDelayAPIInternal(bool enabled)
		{
			if(CanBeModified())
			{
				_includeDelayInLoopsSerialized = enabled;
			}
		}

		internal void OnSetupAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onSetupEvent += callback;
			}
		}

		internal void OnStartAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onStartEvent += callback;
			}
		}

		internal void OnLoopAPIInternal(LoopCallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onLoopEvent += callback;
			}
		}

		internal void OnUpdateAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onUpdateEvent += callback;
			}
		}

		internal void OnCompleteAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onCompletedEvent += callback;
			}
		}

		internal void OnEndAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onEndedEvent += callback;
			}
		}

		internal void OnKillAPIInternal(CallbackHandler callback)
		{
			if(CanBeModified())
			{
				_onKillEvent += callback;
			}
		}

		#endregion

		#region Protected Methods

		protected abstract void SetDefaultValues();

		protected void SetInfiniteDuration()
		{
			if(CanBeModified())
			{
				_process.SetInfiniteDuration();
			}
		}

		protected void SetDuration(float duration)
		{
			if(CanBeModified())
			{
				_process.SetDuration(duration);
				OnSetDuration(duration);
			}
		}

		protected virtual void OnSetup()
		{

		}

		protected virtual void OnStart()
		{

		}

		protected virtual void OnLoop()
		{

		}

		protected virtual void ResetData()
		{
			HasEnded = false;
			IsCompleted = false;
			_loopCount = 0;
		}

		protected virtual void OnSetDuration(float duration)
		{
		}

		protected virtual void OnComplete()
		{

		}

		protected virtual void OnEnd()
		{
		
		}

		/// <summary>
		/// For Clean-up, use Dispose instead. This is a notification before disposal
		/// </summary>
		protected virtual void OnKill()
		{

		}

		protected abstract RaTweenBase CloneSelf();
		protected abstract void Evaluate(float normalizedValue);
		protected abstract void ApplyRewind(bool inclDelay);
		protected abstract void ApplyReset(bool resumeAfterReset);
		protected abstract void Dispose();
		protected abstract float CalculateEvaluation();

		#endregion

		#region Private Methods

		private void ResetDelay()
		{
			_delay.Reset();
			HasStarted = false; 
		}

		private void PerformEvaluation()
		{
			Evaluate(CalculateEvaluation());
			_onUpdateEvent?.Invoke();
		}

		#endregion

		#region Internal

		/// <summary>
		/// The state in which a tween can be found
		/// </summary>
		public enum State
		{
			// Idle
			/// <summary>
			/// No specific state. This is a tween which was made but not yet used
			/// </summary>
			None		= 0,

			// Intent
			/// <summary>
			/// A tween which is registered to start (Play was called on it)
			/// A tween passing its <see cref="State.InDelay"/> state and will go to <see cref="State.InProgress"/> or <see cref="State.Completed"/> state based on if there is an evaluation step to be processed or not
			/// </summary>
			ToStart		= 101,
			/// <summary>
			/// A tween was requested to be resumed after being paused
			/// </summary>
			ToResume = 102,
			
			// Lifecycle
			/// <summary>
			/// A tween is currently progressing through its delay
			/// </summary>
			InDelay		= 201,
			/// <summary>
			/// A tween is currently progressing through its active tween state (evaluating and applying the actual tween)
			/// </summary>
			InProgress	= 203,
			/// <summary>
			/// A tween is paused while in <see cref="InDelay"/> or <see cref="InProgress"/>
			/// </summary>
			IsPaused	= 204,

			/// <summary>
			/// A tween is requested to be completed
			/// </summary>
			Completed	= 205,

			/// <summary>
			/// A tween that has ended by going thorugh the natural flow or being stopped by <see cref="Stop"/>
			/// Making it so it can still be targeted by certain API calls
			/// </summary>
			Ended		= 206,

			/// <summary>
			/// A tween is requested to be killed, this is the clean-up phase of a tween
			/// </summary>
			Dead		= 207,

			// Type Lock
			/// <summary>
			/// A tween is marked as Data, which disables its API and modification calls
			/// This state is used for <see cref="RaTweenSequence"/> entries
			/// </summary>
			Data		= 301,
		}

		#endregion
	}

	public static partial class RaTweenUtilExtensions
	{
		/// <summary>
		/// This registers a tween to the gobal tween evaluation system (<see cref="RaTweeningEngine"/>) in order to play it 
		/// </summary>
		public static TweenT Play<TweenT>(this TweenT self)
			where TweenT : RaTweenBase
		{
			return RaTweeningEngine.Instance.RegisterTween(self);
		}

		/// <summary>
		/// This API call listens to the Setup event of the tween. And will make it so the callback is fired when the Setup occurs
		/// The Setup is triggered when a tween is registered into the system. This happens once in a tween's lifetime.
		/// > Note: All callbacks are automatically cleaned when a tween is killed
		/// </summary>
		public static TweenT OnSetup<TweenT>(this TweenT self, RaTweenBase.CallbackHandler callback)
			where TweenT : RaTweenBase
		{
			self.OnSetupAPIInternal(callback);
			return self;
		}

		/// <summary>
		/// This API call listens to the Start event of the tween. And will make it so the callback is fired when the Start occurs
		/// The Start is triggered when a tween is started. This happens after the Delay has finished of a tween
		/// > Note: In a loop this will thus happen after every Delay in each loop \n
		/// > Note: All callbacks are automatically cleaned when a tween is killed
		/// </summary>
		public static TweenT OnStart<TweenT>(this TweenT self, RaTweenBase.CallbackHandler callback)
			where TweenT : RaTweenBase
		{
			self.OnStartAPIInternal(callback);
			return self;
		}

		/// <summary>
		/// This API call listens to the Update event of the tween. And will make it so the callback is fired when the Update occurs
		/// The Update is triggered whenever the Tween is Evaluated
		/// </summary>
		public static TweenT OnUpdate<TweenT>(this TweenT self, RaTweenBase.CallbackHandler callback)
			where TweenT : RaTweenBase
		{
			self.OnUpdateAPIInternal(callback);
			return self;
		}

		/// <summary>
		/// This API call listens to the Loop event of the tween. And will make it so the callback is fired when the Loop occurs
		/// The Loop is triggered when a tween is entering a new loop cycle.
		/// > Note: The first play of a tween does NOT trigger a Loop callback, only after the first cycle (and each following cycle) has been finished \n
		/// > Note: All callbacks are automatically cleaned when a tween is killed
		/// </summary>
		public static TweenT OnLoop<TweenT>(this TweenT self, RaTweenBase.LoopCallbackHandler callback)
			where TweenT : RaTweenBase
		{
			self.OnLoopAPIInternal(callback);
			return self;
		}

		/// <summary>
		/// This API call listens to the Complete event of the tween. And will make it so the callback is fired when the Complete occurs
		/// The Complete is triggered when a tween has completed all its cycles, or is manually completed through <see cref="RaTweenBase.Complete"/>
		/// > Note: Complete does not trigger when a tween is pre-maturely ended / never finished \n
		/// > Note: Complete is not reached through the normal flow when <see cref="RaTweenBase.IsInfinite"/> or <see cref="RaTweenBase.IsInfiniteLoop"/> are true \n
		/// > Note: All callbacks are automatically cleaned when a tween is killed
		/// </summary>
		public static TweenT OnComplete<TweenT>(this TweenT self, RaTweenBase.CallbackHandler callback)
			where TweenT : RaTweenBase
		{
			self.OnCompleteAPIInternal(callback);
			return self;
		}

		/// <summary>
		/// This API call listens to the End event of the tween. And will make it so the callback is fired when the End occurs
		/// The End is triggered after Completion, or is manually ended through <see cref="RaTweenBase.Stop"/>
		/// > Note: End is not reached through the normal flow when <see cref="RaTweenBase.IsInfinite"/> or <see cref="RaTweenBase.IsInfiniteLoop"/> are true \n
		/// > Note: All callbacks are automatically cleaned when a tween is killed
		/// </summary>
		public static TweenT OnEnd<TweenT>(this TweenT self, RaTweenBase.CallbackHandler callback)
			where TweenT : RaTweenBase
		{
			self.OnEndAPIInternal(callback);
			return self;
		}

		/// <summary>
		/// This API call listens to the Kill event of the tween. And will make it so the callback is fired when the Kill occurs
		/// The Kill is triggered after a tween was completed or when it was manually ended through <see cref="RaTweenBase.Stop"/>
		/// > Note: All callbacks are automatically cleaned when a tween is killed
		/// </summary>
		public static TweenT OnKill<TweenT>(this TweenT self, RaTweenBase.CallbackHandler callback)
			where TweenT : RaTweenBase
		{
			self.OnKillAPIInternal(callback);
			return self;
		}

		/// <summary>
		/// This API call Sets the amount of times the tween should loop.
		/// 0 == No Loops, only play once
		/// -1 == Infinite Looping (<see cref="RaTweenBase.InfiniteLoopingValue"/>)
		/// 1 or greater == Loop Amount
		/// > Note: The loops are excluding the delay, to include the delay for each loop, call <see cref="SetLoopingIncludesDelay{TweenT}(TweenT, bool)"/>
		/// </summary>
		public static TweenT SetLooping<TweenT>(this TweenT self, int loopAmount)
			where TweenT : RaTweenBase
		{
			self.SetLoopingAPIInternal(loopAmount);
			return self;
		}

		/// <summary>
		/// This API call Sets whether or not the loop should include the delay 
		/// When False is passed, the delay will only be applied at the start of the tween. And excluded from the looping process.
		/// > Note: By default it is set to `false`
		/// </summary>
		public static TweenT SetLoopingIncludesDelay<TweenT>(this TweenT self, bool loopingIncludesDelay = true)
			where TweenT : RaTweenBase
		{
			self.SetLoopingIncludesDelayAPIInternal(loopingIncludesDelay);
			return self;
		}

		/// <summary>
		/// This API call Sets the tween to loop infinitely
		/// -1 == Infinite Looping (<see cref="RaTweenBase.InfiniteLoopingValue"/>)
		/// </summary>
		public static TweenT SetInfiniteLooping<TweenT>(this TweenT self)
			where TweenT : RaTweenBase
		{
			self.SetLoopingAPIInternal(RaTweenBase.InfiniteLoopingValue);
			return self;
		}

		/// <summary>
		/// This API call Sets the tween to loop infinitely
		/// 0 == No Loops, only play once
		/// </summary>
		public static TweenT DisableLooping<TweenT>(this TweenT self)
			where TweenT : RaTweenBase
		{
			self.SetLoopingAPIInternal(0);
			return self;
		}

		/// <summary>
		/// This API call Sets the delay until the tween starts playing in seconds
		/// > Note: A Delay causes the tween to not evaluate its content until the delay has passed
		/// </summary>
		public static TweenT SetDelay<TweenT>(this TweenT self, float delay)
			where TweenT : RaTweenBase
		{
			self.SetDelayAPIInternal(delay);
			return self;
		}


		/// <summary>
		/// This API call sets the <see cref="RaTweenBase.PauseOnEnd"/>, which, when true, Pauses the Tween on the End of it
		/// This prevents it from being Killed by the <see cref="RaTweeningProcessor"/>
		/// </summary>
		public static TweenT SetPauseOnEnd<TweenT>(this TweenT self, bool pauseOnEnd = true)
			where TweenT : RaTweenBase
		{
			self.SetPauseOnEndAPIInternal(pauseOnEnd);
			return self;
		}

		/// <summary>
		/// Sets the GroupID of the Tween.
		/// > Note: When the tween is Submitted to a Processor, only then does the tween become part of the GroupID it is assigned to. \n
		/// > Note: A Tween is Submitted to a Processor when it is Registered to the <see cref="RaTweeningEngine"/> (When <see cref="Play{TweenT}(TweenT)"/> is called on it)
		/// </summary>
		public static TweenT SetGroup<TweenT>(this TweenT self, object groupID)
			where TweenT : RaTweenBase
		{
			self.SetPotentialGroupInternal(groupID);
			return self;
		}

		/// <summary>
		/// Clears the GroupID of the Tween set using <see cref="SetGroup{TweenT}(TweenT, object)"/>
		/// > Note: When the tween is Submitted to a Processor, only then does the tween become part of the GroupID it is assigned to. \n
		/// > Note: A Tween is Submitted to a Processor when it is Registered to the <see cref="RaTweeningEngine"/> (When <see cref="Play{TweenT}(TweenT)"/> is called on it)
		/// </summary>
		public static TweenT ClearGroup<TweenT>(this TweenT self)
			where TweenT : RaTweenBase
		{
			self.ClearPotentialGroupInternal();
			return self;
		}

		/// <summary>
		/// Sets the TimeScale of the Tween. Which changes the Delay and Evaluation speed.
		/// > Note: This will multiply the value of the set <see cref="ITimeScaleChannel"/>. If none is set, this will be the leading TimeScale value. \n
		/// > Note: This can be called at any state of the tween
		/// </summary>
		public static TweenT SetTimeScale<TweenT>(this TweenT self, float timeScale)
			where TweenT : RaTweenBase
		{
			self.SetTimeScaleInternal(timeScale);
			return self;
		}

		/// <summary>
		/// Sets the <see cref="ITimeScaleChannel"/> of the Tween to `Null`. Which changes the Delay and Evaluation speed.
		/// > Note: This can be called at any state of the tween \n
		/// > Note: This makes it so the tweens TimeScaleValue is multiplied by 1 (so running on Real Time)
		/// </summary>
		public static TweenT SetToRealTime<TweenT>(this TweenT self)
			where TweenT : RaTweenBase
		{
			self.SetTimeScaleChannelInternal(null);
			return self;
		}

		/// <summary>
		/// Sets the <see cref="ITimeScaleChannel"/> of the Tween to <see cref="GameTimeScaleChannel"/>. Which changes the Delay and Evaluation speed.
		/// > Note: This can be called at any state of the tween \n
		/// > Note: This makes it so the tweens TimeScaleValue is multiplied by `Time.timeScale` from Unity
		/// </summary>
		public static TweenT SetToGameTime<TweenT>(this TweenT self)
			where TweenT : RaTweenBase
		{
			self.SetTimeScaleChannelInternal(GameTimeScaleChannel.Instance);
			return self;
		}

		/// <summary>
		/// Sets the <see cref="ITimeScaleChannel"/> of the Tween. Which changes the Delay and Evaluation speed.
		/// > Note: This can be called at any state of the tween
		/// </summary>
		public static TweenT SetTimeScaleChannel<TweenT>(this TweenT self, ITimeScaleChannel timeScaleChannel)
			where TweenT : RaTweenBase
		{
			self.SetTimeScaleChannelInternal(timeScaleChannel);
			return self;
		}
	}
}