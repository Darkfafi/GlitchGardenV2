using UnityEngine;
using RaTweening.Core.Elements;

namespace RaTweening
{
	/// <summary>
	/// The Component that allows for making Tweens within the editor.
	/// This component is used to create tweens without code or have visual designers create tweens which are triggered by code
	/// This system uses <see cref="RaTweening.Core.Elements.RaTweenerElementBase"/> inheritors to serialize the data of tweens
	/// </summary>
	[ExecuteAlways]
	public class RaTweenerComponent : MonoBehaviour
	{
		#region Editor Variables

		[SerializeReference, HideInInspector]
		private RaTweenerElementBase _tweenElement = null; 

		[Header("Options")]
		[SerializeField]
		[Tooltip("Based on `_optionProgressor`. \n" +
			"Default = Create New Tween.\n" +
			"ResumeOrDefault = Resume Tween if not ended, else Default Behaviour.\n" +
			"Resume = Resume Tween if not ended, else do nothing.\n" +
			"Reset = Reset Existing Tween.(Keep initial calculated Start and End Values)\n" +
			"ResumeOrReset = Resume Tween if not ended, else Reset Behaviour.")]
		private PlayOption _playOption = PlayOption.OnEnable;

		[SerializeField]
		[Tooltip("Based on `_optionProgressor`. \n"+
			"Default = Stop Tween.\n" +
			"ResumeOrDefault = Pause Tween if not ended, else Default Behaviour\n" +
			"Resume = Pause Tween if not ended, else do nothing\n" +
			"Reset = Pause Tween\n" +
			"ResumeOrReset = Stop: Pause Tween")]
		private StopOption _stopOption = StopOption.OnDisable;

		[SerializeField]
		[Tooltip("Defines what happens within `playOption` & `stopOption`. \n" +
			"Default = Play: Create New Tween. Stop: Stop Tween.\n" +
			"ResumeOrDefault = Play: Resume Tween if not ended, else Default Behaviour. Stop: Pause Tween if not ended, else Default Behaviour\n" +
			"Resume = Play: Resume Tween if not ended, else do nothing. Stop: Pause Tween if not ended, else do nothing\n" +
			"Reset = Play: Reset Existing Tween. Stop: Pause Tween (Reset will cause it to remain its initial calculated Start and End Values)\n" +
			"ResumeOrReset = Play: Resume Tween if not ended, else Reset Behaviour. Stop: Pause Tween")]
		private Progressor _optionProgressor = Progressor.Default;

		#endregion

		#region Variables

		private RaTweenBase _tween = null;

		#endregion

		#region Properties

		/// <summary>
		/// Returns true if the tween which was started using this component is still playing
		/// </summary>
		public bool IsPlaying => _tween != null && _tween.IsPlaying;

		/// <summary>
		/// Returns true if the tween which was started using this component is paused
		/// </summary>
		public bool IsPaused => _tween != null && _tween.IsPaused;

		/// <summary>
		/// Returns true if the tween which was started using this component has completed
		/// </summary>
		public bool IsCompleted => _tween != null && _tween.IsCompleted;

		#endregion

		#region Lifecycle

		protected void Awake()
		{
			TryPerformPlay(PlayOption.OnAwake);
		}

		protected void Start()
		{
			TryPerformPlay(PlayOption.OnStart);
		}

		protected void OnEnable()
		{
			TryPerformPlay(PlayOption.OnEnable);
		}

		protected void OnDisable()
		{
			TryPerformStop(StopOption.OnDisable);
		}

		protected void OnDestroy()
		{
			if(_tweenElement)
			{
#if UNITY_EDITOR
				if(Core.RaTweenerComponentEditor.TryRemoveTween(_tweenElement))
				{
					_tweenElement = null;
				}
#else
				if(Application.isPlaying)
				{
					Destroy(_tweenElement);
					_tweenElement = null;
				}
#endif
			}
		}

		#endregion

		#region Public Methods

		// Editor Specific

		/// <summary>
		/// Plays the tween of the component without a return value so it can be referenced in UnityEvents
		/// > Note: This method is designed to be used within UnityEvents \n
		/// > Note: This stops the previous tween of the component if active
		/// </summary>
		public void EditorPlay()
		{
			Play();
		}

		/// <summary>
		/// Pauses the tween of the component if it is playing without a return value so it can be referenced in UnityEvents
		/// > Note: This method is designed to be used within UnityEvents \n
		/// </summary>
		public void EditorPause()
		{
			Pause();
		}

		/// <summary>
		/// Resumes the currently paused tween without a return value so it can be referenced in UnityEvents
		/// If the tween is no longer active due to completion or destruction, it plays a new tween instance.
		/// > Note:  without a return value so it can be referenced in UnityEvents \n
		/// </summary>
		public void EditorResumeOrPlay()
		{
			ResumeOrPlay();
		}

		/// <summary>
		/// Attempts to resume to paused tween of this component if able, without a return value so it can be referenced in UnityEvents
		/// > Note:  without a return value so it can be referenced in UnityEvents \n
		/// </summary>
		public void EditorResume()
		{
			Resume();
		}

		/// <summary>
		/// Attempts to Stop the currently active tween if able, without a return value so it can be referenced in UnityEvents
		/// > Note:  without a return value so it can be referenced in UnityEvents \n
		/// </summary>
		/// <param name="rewind">When set to True, it also evaluates the tween to the first evaluation step</param>
		public void EditorStop(bool rewind)
		{
			Stop(rewind);
		}

		/// <summary>
		/// Attempts to Reset the currently active tween if able, without a return value so it can be referenced in UnityEvents
		/// > Note:  without a return value so it can be referenced in UnityEvents \n
		/// </summary>
		public void EditorReset()
		{
			Reset();
		}

		/// <summary>
		/// Attempts to Rewind the currently active tween if able, without a return value so it can be referenced in UnityEvents
		/// > Note: This resets all timers and performs the inital evaluation.ep
		/// > Note:  without a return value so it can be referenced in UnityEvents \n
		/// </summary>
		public void EditorRewind()
		{
			Rewind();
		}

		// Coding API

		/// <summary>
		/// Create an instance of the tween defined within the component
		/// </summary>
		/// <returns>The new tween instance</returns>
		public RaTweenBase CreateTweenInstance()
		{
			if(_tweenElement)
			{
				return _tweenElement.CreateTween();
			}
			return null;
		}

		/// <summary>
		/// Plays the tween of the component
		/// > Note: This stops the previous tween of the component if active
		/// </summary>
		public RaTweenBase Play()
		{
			Stop();

			if(_tweenElement)
			{
				_tween = _tweenElement
					.CreateTween()
					.Play();

				switch(_optionProgressor)
				{
					case Progressor.Reset:
					case Progressor.ResumeOrReset:
						_tween.SetPauseOnEnd();
						break;
				}

				return _tween;
			}

			return null;
		}

		/// <summary>
		/// Pauses the tween of the component if it is playing
		/// > Note: See <see cref="RaTweenBase.Pause()"/>
		/// </summary>
		/// <returns>True if the tween was successfully paused, false if no tween was active to pause</returns>
		public bool Pause()
		{
			if(_tween != null)
			{
				return _tween.Pause();
			}
			return false;
		}

		/// <summary>
		/// Resumes the currently paused tween.
		/// If the tween is no longer active due to completion or destruction, it plays a new tween instance.
		/// > Note: See <see cref="RaTweenBase.Resume(bool)"/>
		/// </summary>
		public RaTweenBase ResumeOrPlay()
		{
			if(_tween != null && _tween.CanUseAPI(false))
			{
				Resume();
			}
			else
			{
				Play();
			}

			return _tween;
		}

		/// <summary>
		/// Attempts to resume the paused tween of this component if able.
		/// > Note: See <see cref="RaTweenBase.Resume(bool)"/>
		/// </summary>
		/// <returns>True if the tween was resumed, else false</returns>
		public bool Resume()
		{
			if(_tween != null)
			{
				return _tween.Resume();
			}
			return false;
		}

		/// <summary>
		/// Attempts to rewind tween of this component if able.
		/// > Note: This resets all timers and performs the inital evaluation. \n
		/// > Note: See <see cref="RaTweenBase.Rewind(bool)"/>
		/// </summary>
		/// <returns>True if the tween was resumed, else false</returns>
		public bool Rewind(bool inclDelay = true)
		{
			if(_tween != null)
			{
				return _tween.Rewind(inclDelay);
			}
			return false;
		}

		/// <summary>
		/// Attempts to reset tween of this component if able.
		/// > Note: With the default parameters, it will act as if the tween was freshly submitted to the system \n
		/// > Note: See <see cref="RaTweenBase.Reset(bool)"/>
		/// </summary>
		/// <returns>True if the tween was resumed, else false</returns>
		public bool Reset(bool resumeAfterReset = false)
		{
			if(_tween != null)
			{
				return _tween.Reset(resumeAfterReset);
			}
			return false;
		}

		/// <summary>
		/// Attempts to Stop the currently active tween if able
		/// If reset is set to true, it also evaluates the tween to the first evaluation step
		/// </summary>
		/// <param name="rewind">To apply the first evaluation of the tween</param>
		/// <returns>If the stop was successful</returns>
		public bool Stop(bool rewind = false)
		{
			if(_tween != null)
			{
				_tween.Stop();
				
				if(rewind)
				{
					_tween.Rewind(true);
				}
				return true;
			}
			return false;
		}

		#endregion

		#region Internal Methods

		internal RaTweenBase CreatePreviewTween()
		{
			if(_tweenElement)
			{
				return _tweenElement
					.CreateCoreTweenInternal();
			}
			return null;
		}

		#endregion

		#region Private Methods

		private void TryPerformPlay(PlayOption option)
		{
			if(option != _playOption || !Application.isPlaying)
			{
				return;
			}

			switch(_optionProgressor)
			{
				case Progressor.Default:
					Play();
					break;
				case Progressor.ResumeOrDefault:
					ResumeOrPlay();
					break;
				case Progressor.Resume:
					if(_tween == null || !_tween.IsAlive(false))
					{
						Play();
					}
					else
					{
						Resume();
					}
					break;
				case Progressor.Reset:
					if(_tween == null || !_tween.IsAlive(true))
					{
						Play();
					}
					else
					{
						Reset(true);
					}
					break;
				case Progressor.ResumeOrReset:
					if(_tween == null || !_tween.IsAlive(true))
					{
						Play();
					}
					else if(_tween != null)
					{
						if(_tween.HasEnded)
						{
							Reset(true);
						}
						else
						{
							Resume();
						}
					}
					break;
			}
		}

		private void TryPerformStop(StopOption option)
		{
			if(option != _stopOption || !Application.isPlaying)
			{
				return;
			}

			switch(_optionProgressor)
			{
				case Progressor.Default:
					Stop();
					break;
				case Progressor.ResumeOrDefault:
					if(!Pause())
					{
						Stop();
					}
					break;
				case Progressor.Resume:
				case Progressor.Reset:
				case Progressor.ResumeOrReset:
					Pause();
					break;
			}
		}

		#endregion

		#region Nested

		/// <summary>
		/// These enum values represent the options on which trigger the tween of the component should play
		/// Based on the <see cref="Progressor"/> chosen, the Play option will override the current tween or not
		/// </summary>
		public enum PlayOption
		{
			/// <summary>
			/// The tween of the component does not start unless triggered by code
			/// </summary>
			None,

			/// <summary>
			/// The tween of the component starts on Awake
			/// </summary>
			OnAwake,

			/// <summary>
			/// The tween of the component starts on Start
			/// </summary>
			OnStart,

			/// <summary>
			/// The tween of the component starts on Enable
			/// </summary>
			OnEnable
		}


		/// <summary>
		/// These enum values represent the options on which trigger the tween of the component should stop
		/// </summary>
		public enum StopOption
		{
			/// <summary>
			/// The tween of the component does not stop unless it finishes by itself or is stopped by code
			/// </summary>
			None,

			/// <summary>
			/// The tween of the component stops on Disable
			/// </summary>
			OnDisable
		}

		/// <summary>
		/// These enum values represent what the <see cref="PlayOption"/> and <see cref="StopOption"/> will do on their triggers
		/// </summary>
		public enum Progressor
		{
			/// <summary>
			/// The tween will be Played on <see cref="PlayOption"/> and be Ended on <see cref="StopOption"/>. \n
			/// It will override the previous tween if active.
			/// > Note: Play: Create New Tween. Stop: Stop Tween.
			/// </summary>
			Default,
			/// <summary>
			/// The tween will be Resumed on <see cref="PlayOption"/> and be Paused on <see cref="StopOption"/>. \n
			/// However, if the tween has ended, it will play it once again when the trigger occurs
			/// > Note: ResumeOrDefault = Play: Resume Tween if not ended, else <see cref="Default"/> Behaviour. Stop: Pause Tween if not ended, else <see cref="Default"/> Behaviour
			/// </summary>
			ResumeOrDefault,
			/// <summary>
			/// The tween will be Resumed on <see cref="PlayOption"/> and be Paused on <see cref="StopOption"/>. \n
			/// If the tween has ended, it will remain in its current state unless manually triggered by code
			/// > Note: Play: Resume Tween if not ended, else do nothing. Stop: Pause Tween if not ended, else do nothing
			/// </summary>
			Resume,
			/// <summary>
			/// The tween will be Reset & Resumed on <see cref="PlayOption"/> and be Paused on <see cref="StopOption"/>. \n
			/// It will override the previous tween if active.
			/// > Note: Play: Reset Existing Tween. Stop: Pause Tween (Reset will cause it to remain its initial calculated Start and End Values)
			/// </summary>
			Reset,

			/// <summary>
			/// The tween will be Resumed on <see cref="PlayOption"/> and be Paused on <see cref="StopOption"/>. \n
			/// However, if the tween has ended, it will play it Reset the tween and play it when the trigger occurs
			/// > Note: Play: Resume Tween if not ended, else <see cref="Reset"/> Behaviour. Stop: Pause Tween
			/// </summary>
			ResumeOrReset,
		}

		#endregion
	}
}