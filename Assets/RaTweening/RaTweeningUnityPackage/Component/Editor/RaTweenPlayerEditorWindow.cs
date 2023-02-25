#if UNITY_EDITOR
using RaTweening.TimeScale;
using System;
using UnityEditor;
using UnityEngine;
using static RaTweening.Tools.RaEditorUtils;

namespace RaTweening.Core
{
	internal class RaTweenPlayerEditorWindow : EditorWindow
	{
		#region Editor Variables

		[SerializeField]
		private RaTweenerComponent _tweenElement = null;
		
		[SerializeField]
		private bool _isRunning = false;
		
		[SerializeField]
		private float _lastTime = 0f;

		[SerializeField]
		private bool _setLooping = false;

		[SerializeField]
		private Vector2 _position = default;

		[SerializeField]
		private float _timeScale = 1f;

		[SerializeField]
		private bool _previewTweenDataFoldout = true;

		[SerializeField]
		private bool _editorFoldout = true;

		#endregion

		#region Variables

		private RaTweenBase _tween = null;
		private Editor _editor;
		private RaTweeningProcessor _processor = new RaTweeningProcessor();

		#endregion

		#region Lifecycle

		protected void OnEnable()
		{
			if(_tweenElement != null)
			{
				Init(_tweenElement);
			}
		}

		protected void OnDisable()
		{
			Stop();
		}

		protected void Update()
		{
			if(_isRunning)
			{
				if(!_tweenElement)
				{
					Stop();
					return;
				}

				float deltaTime = (Time.realtimeSinceStartup - _lastTime) * _timeScale;
				_processor.Step(deltaTime);
				_lastTime = Time.realtimeSinceStartup;
				
				// Correctly Display Process
				Repaint();
				EditorUtility.SetDirty(_tweenElement);
			}
		}

		protected void OnDestroy()
		{
			Stop();
			_editor = null;
		}

		protected void OnGUI()
		{
			try
			{
				if(_tweenElement == null || _processor == null || _editor == null)
				{
					if(_tweenElement != null)
					{
						Init(_tweenElement);
					}
					else if(Selection.activeGameObject != null)
					{
						RaTweenerComponent comp = Selection.activeGameObject.GetComponent<RaTweenerComponent>();
						if(comp != null)
						{
							Init(comp);
						}
					}
					else
					{
						EditorGUILayout.LabelField("No TweenElement Target Selected");
					}
					return;
				}

				EditorGUILayout.BeginVertical("box");
				{
					EditorGUILayout.LabelField("Preview");

					EditorGUILayout.BeginHorizontal();
					{
						if(_tween == null)
						{
							if(IconButton("d_PlayButton On@2x", 30))
							{
								Play();
							}
						}
						else if(_tween.IsPaused)
						{
							if(IconButton("d_PlayButton On@2x", 30))
							{
								Resume();
							}
						}
						else
						{
							if(IconButton("d_PauseButton On@2x", 30, _tween.IsPlaying))
							{
								Pause();
							}
						}

						if(IconButton("d_PreMatQuad@2x", 30, _tween != null))
						{
							Stop();
						}

						EditorGUILayout.BeginVertical();
						{
							GUILayout.Space(4.5f);
							EditorGUILayout.Slider(_tween != null ? _tween.TotalProgress : 0f, 0f, 1f);
						}
						EditorGUILayout.EndVertical();
					}
					EditorGUILayout.EndHorizontal();

					_timeScale = EditorGUILayout.Slider("Time Scale: ", _timeScale, 0.1f, 1f);
					_setLooping = EditorGUILayout.Toggle("Loop: ", _setLooping);
				}
				EditorGUILayout.EndVertical();

				if(_tweenElement != null)
				{
					RaTweenBase liveTween = _tween;
					bool isLive = true;

					if(liveTween == null)
					{
						isLive = false;
						liveTween = _tweenElement.CreatePreviewTween();
						liveTween.SetStateInternal(RaTweenBase.State.Data);
					}

					EditorGUILayout.BeginVertical("box");
					{
						_previewTweenDataFoldout = EditorGUILayout.Foldout(_previewTweenDataFoldout, $"Tween Data ({(isLive ? "Preview" : "Data")})");

						if(_previewTweenDataFoldout)
						{
							DrawLabeledLabel("Delay: ", GetProgressString(liveTween.Delay * liveTween.DelayProgress, liveTween.Delay, false));
							DrawLabeledLabel("Time: ", GetProgressString(liveTween.Time, liveTween.Duration, liveTween.IsInfinite));
							DrawLabeledLabel("Total: ", GetProgressString(liveTween.TotalTime, liveTween.TotalDuration, liveTween.IsInfinite));
							DrawLabeledLabel($"Total ↬ [{(liveTween.IsInfiniteLoop ? "∞" : liveTween.Loops.ToString())}]: ", GetProgressString(liveTween.TotalLoopingTime, liveTween.TotalLoopingDuration, liveTween.IsInfinite || liveTween.IsInfiniteLoop));
							DrawLabeledLabel("Time Scale: ", liveTween.TimeScale.ToString());
						}
					}
					EditorGUILayout.EndVertical();
				}

				EditorGUILayout.BeginVertical("box");
				{
					_editorFoldout = EditorGUILayout.Foldout(_editorFoldout, "Editor: " + _tweenElement.name);
					if(_editorFoldout)
					{
						_position = EditorGUILayout.BeginScrollView(_position);
						{
							_editor.OnInspectorGUI();
						}
						EditorGUILayout.EndScrollView();
					}
				}
				EditorGUILayout.EndVertical();
			}
			catch(ArgumentException) 
			{
				Stop();
			};
		}

		#endregion

		#region Public Methods

		public static RaTweenPlayerEditorWindow Open(RaTweenerComponent component)
		{
			RaTweenPlayerEditorWindow window = GetWindow<RaTweenPlayerEditorWindow>("Tweener Editor", true);
			window.Init(component);
			return window;
		}

		public void Init(RaTweenerComponent component)
		{
			Stop();

			_tweenElement = component;
			_isRunning = false;
			_lastTime = Time.realtimeSinceStartup;
			_setLooping = false;
			_tween = null;
			_editor = Editor.CreateEditor(component);
			_processor = new RaTweeningProcessor();
			_position = default;
			_timeScale = 1f;
			_previewTweenDataFoldout = true;
			_editorFoldout = false;
		}

		#endregion

		#region Private Methods

		private string GetProgressString(float a, float b, bool isInfinite)
		{
			if(isInfinite)
			{
				return a.ToString("0.0") + "s / " + "Infinite";
			}
			else
			{
				return a.ToString("0.0") + "s / " + b.ToString("0.0") + "s";
			}
		}

		private RaTweenBase Play()
		{
			Stop();

			_isRunning = true;
			_lastTime = Time.realtimeSinceStartup;

			if(_tweenElement)
			{
				_tween = _tweenElement
					.CreatePreviewTween()
					.OnComplete(()=> 
					{
						Stop();
					});

				if(_setLooping)
				{
					_tween.SetInfiniteLooping();
				}
				else
				{
					_tween.DisableLooping();
				}

				_processor.RegisterTween(_tween);

				return _tween;
			}

			return null;
		}

		private bool Pause()
		{
			if(_tween != null)
			{
				return _tween.Pause();
			}
			return false;
		}

		private bool Resume()
		{
			if(_tween != null)
			{
				return _tween.Resume();
			}
			return false;
		}

		private bool Stop()
		{
			_isRunning = false;
			if(_tween != null)
			{
				_tween.Stop();
				_tween.Rewind();

				_tween = null;
				
				_processor.Step(0f);

				// Correctly Display Process
				Repaint();

				if(_tweenElement)
				{
					EditorUtility.SetDirty(_tweenElement);
				}

				return true;
			}
			return false;
		}

		#endregion
	}
}
#endif