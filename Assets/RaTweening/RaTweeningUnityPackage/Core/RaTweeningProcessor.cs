using System;
using System.Collections.Generic;

namespace RaTweening.Core
{
	/// <summary>
	/// The Processor can be used to manually handle the states of tweens which have been registered to it.
	/// It completely handles all the state related logics such as handling their delays, lifetime, evaluations and all other state related aspects.
	/// This system is used by <see cref="RaTweeningEngine"/> and <see cref="RaTweenSequence"/> to handle their internal tweens.
	/// > Note: By Default, a tween runs in RealTime. To change it to GameTime, call <see cref="RaTweenUtilExtensions.SetToGameTime{TweenT}(TweenT)"/> on that tween.
	/// </summary>
	public class RaTweeningProcessor : IDisposable
	{
		#region Variables

		private readonly List<RaTweenBase> _tweens = new List<RaTweenBase>();
		private readonly List<RaTweenBase> _endedTweens = new List<RaTweenBase>();
		private readonly List<RaTweenBase> _killedTweens = new List<RaTweenBase>();

		private readonly bool _isGroupSubmitter = false;

		#endregion

		#region Properties

		public int TweensCount => _tweens.Count;

		#endregion

		public RaTweeningProcessor()
			: this(false)
		{
		
		}

		internal RaTweeningProcessor(bool isGroupSubmitter)
		{
			_isGroupSubmitter = isGroupSubmitter;
		}

		#region Public Methods

		/// <summary>
		/// Ends all tweens within the processor
		/// > Note: See <see cref="RaTweenBase.Stop()"/>
		/// </summary>
		/// <param name="cleanEndedTweens"></param>
		public void Stop(bool cleanEndedTweens = true)
		{
			for(int i = _tweens.Count - 1; i >= 0; i--)
			{
				_tweens[i].Stop();
			}
			Step(0f, cleanEndedTweens);
		}

		/// <summary>
		/// Rewinds all tweens within the processor.
		/// > Note: See <see cref="RaTweenBase.Rewind(bool)"/>
		/// </summary>
		/// <param name="inclDelay">Also rewinds the Delay, else it only rewinds the evaluation time</param>
		public void Rewind(bool inclDelay = true)
		{
			for(int i = _tweens.Count - 1; i >= 0; i--)
			{
				_tweens[i].Rewind(inclDelay);
			}
		}

		/// <summary>
		/// Resets all tweens within the processor.
		/// > Note: See <see cref="RaTweenBase.Reset(bool)"/>
		/// </summary>
		/// <param name="resumeAfterReset">When True, this will unpause the tween immediately after Reset</param>
		public void Reset(bool resumeAfterReset = false)
		{
			for(int i = _tweens.Count - 1; i >= 0; i--)
			{
				_tweens[i].Reset(resumeAfterReset);
			}
		}

		/// <summary>
		/// Completes all tweens within the processor
		/// > Note: See <see cref="RaTweenBase.Complete()"/>
		/// </summary>
		public void Complete(bool cleanEndedTweens = true)
		{
			for(int i = _tweens.Count - 1; i >= 0; i--)
			{
				_tweens[i].Complete();
			}
			Step(0f, cleanEndedTweens);
		}

		/// <summary>
		/// This method progresses the tweens registered through their various states.
		/// Handling their delays, lifetime, evaluations and all other state related aspects.
		/// </summary>
		/// <param name="deltaTime">Time to progres the registered tweens with</param>
		/// <param name="cleanEndedTweens">on True, calls <see cref="CleanEndedTweens"/> at end of step</param>
		public void Step(float deltaTime, bool cleanEndedTweens = true)
		{
			for(int i = 0, c = _tweens.Count; i < c; i++)
			{
				RaTweenBase tween = _tweens[i];

				if(tween.IsValid)
				{
					if(!tween.IsSetup)
					{
						tween.SetupInternal();
					}

					// Stepping
					switch(tween.TweenState)
					{
						case RaTweenBase.State.ToStart:
						case RaTweenBase.State.ToResume:
							if(tween.HasEnded)
							{
								tween.SetStateInternal(RaTweenBase.State.Ended);
							}
							else if(!tween.HasStarted)
							{
								tween.SetStateInternal(RaTweenBase.State.InDelay);
							}
							else if(!tween.IsCompleted)
							{
								tween.SetStateInternal(RaTweenBase.State.InProgress);
							}
							else
							{
								tween.SetStateInternal(RaTweenBase.State.Ended);
							}

							break;
					}
				}
				else
				{
					tween.SetStateInternal(RaTweenBase.State.Ended);
				}

				float tweenDeltaTime = deltaTime * tween.TimeScale;
				do
				{
					// Stepping
					switch(tween.TweenState)
					{
						case RaTweenBase.State.InDelay:
							tweenDeltaTime = tween.StepDelayInternal(tweenDeltaTime);
							if(tween.HasNoDelayRemaining)
							{
								tween.StartInternal();
								if(tween.IsTotalCompleted)
								{
									tween.SetStateInternal(RaTweenBase.State.Completed);
								}
								else
								{
									tween.SetStateInternal(RaTweenBase.State.InProgress);
								}
							}
							break;

						case RaTweenBase.State.InProgress:
							tweenDeltaTime = tween.StepTweenInternal(tweenDeltaTime);
							if(tween.IsTotalCompleted)
							{
								if(tween.HasReachedLoopEnd)
								{
									tween.SetStateInternal(RaTweenBase.State.Completed);
								}
								else
								{
									tween.SetStateInternal(RaTweenBase.State.InDelay);
									tween.LoopInternal();
								}
							}
							break;
						default:
							tweenDeltaTime = 0f;
							break;
					}
				}
				while(tweenDeltaTime > 0f);

				// State Switching
				switch(tween.TweenState)
				{
					case RaTweenBase.State.Completed:
						if(!tween.HasStarted)
						{
							tween.StartInternal();
						}
						tween.CompleteInternal();
						tween.SetStateInternal(RaTweenBase.State.Ended);
						break;
				}

				if(!tween.HasEnded && tween.TweenState == RaTweenBase.State.Ended)
				{
					tween.EndInternal();
					_endedTweens.Add(tween);
				}
			}

			if(cleanEndedTweens)
			{
				CleanEndedTweens();
			}
		}

		/// <summary>
		/// Kills all Tweens which <see cref="RaTweenBase.HasEnded"/>
		/// </summary>
		public void CleanEndedTweens()
		{
			for(int i = _endedTweens.Count - 1; i >= 0; i--)
			{
				RaTweenBase tween = _endedTweens[i];

				if(tween.TweenState == RaTweenBase.State.Ended)
				{
					tween.SetStateInternal(RaTweenBase.State.Dead);
					tween.KillInternal();
					_killedTweens.Add(tween);
					_endedTweens.RemoveAt(i);
				}
			}

			for(int i = _killedTweens.Count - 1; i >= 0; i--)
			{
				UnregisterTween(_killedTweens[i]);
			}

			_killedTweens.Clear();
		}

		public void Dispose()
		{
			for(int i = _killedTweens.Count - 1; i >= 0; i--)
			{
				_killedTweens[i].DisposeInternal();
			}

			for(int i = _tweens.Count - 1; i >= 0; i--)
			{
				_tweens[i].DisposeInternal();
			}

			_killedTweens.Clear();
			_tweens.Clear();
		}

		/// <summary>
		/// This method attempts to register the given tween in the system, setting the tween state to <see cref="RaTweenBase.State.ToStart"/>
		/// After Registration, the tween will go through the processing flow by calling the <see cref="Step"/> method.
		/// > Note: Registration is only allowed if the state of the tween is <see cref="RaTweenBase.State.None"/>, else it will ignore it.
		/// </summary>
		public TweenT RegisterTween<TweenT>(TweenT tween)
			where TweenT : RaTweenBase
		{
			if(tween.TweenState == RaTweenBase.State.None)
			{
				_tweens.Add(tween);
				tween.SetStateInternal(RaTweenBase.State.ToStart);
				
				if(_isGroupSubmitter)
				{
					tween.SubmitGroupInternal();
				}
			}
			else
			{
				UnityEngine.Debug.LogError($"Tween {tween} can't be registered because it is not in the state {nameof(RaTweenBase.State.None)}");
			}

			return tween;
		}

		/// <summary>
		/// This method attempts to unregister the given tween from the system.
		/// After Unregistration, the tween will no longer go through the processing flow by calling the <see cref="Step"/> method.
		/// > Note: Unregistration is only allowed if the state of the tween is NOT <see cref="RaTweenBase.State.None"/>, else it will ignore it.
		/// </summary>
		public TweenT UnregisterTween<TweenT>(TweenT tween)
			where TweenT : RaTweenBase
		{
			if(tween.TweenState != RaTweenBase.State.None)
			{
				if(_tweens.Remove(tween))
				{
					if(_isGroupSubmitter)
					{
						tween.RevertGroupInternal();
					}
				}
			}

			return tween;
		}

		#endregion
	}
}