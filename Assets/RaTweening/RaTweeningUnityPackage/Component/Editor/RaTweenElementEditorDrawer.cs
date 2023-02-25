#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static RaTweening.Core.Elements.RaTweenerElementBase;

namespace RaTweening.Core.Elements
{
	internal class RaTweenElementEditorDrawer
	{
		#region Consts

		private static readonly string LoopsLabel = "Loops: ";
		private static readonly string InclDelayLoopsLabel = "Include Delay: ";

		private static readonly string[] Events = new string[]
		{
			"_onSetup",
			"_onStart",
			"_onUpdate",
			"_onLoop",
			"_onComplete",
			"_onEnd",
			"_onKill",
		};

		#endregion

		#region Variables

		private SerializedObject _elementSerializedObject = null;
		private RaTweenerElementBase _element = null;

		private bool _foldedOutEventProps = false;
		private SerializedProperty[] _eventProps = null;

		private bool _foldedOutPropertiesProps = false;
		private SerializedProperty _delayProperty = null;

		private SerializedProperty _loopsProperty = null;
		private SerializedProperty _loopsInclDelayProperty = null;

		private bool _foldedOutTimeScaleProps = false;
		private SerializedProperty[] _timeScaleProps = null;
		private SerializedProperty _timeScaleValueProperty = null;
		private SerializedProperty _timeScaleChannelProperty = null;

		#endregion

		public RaTweenElementEditorDrawer(SerializedObject elementObject)
		{
			_elementSerializedObject = elementObject;
			_element = elementObject.targetObject as RaTweenerElementBase;

			// Events
			_eventProps = new SerializedProperty[Events.Length];
			for(int i = 0; i < Events.Length; i++)
			{
				_eventProps[i] = elementObject.FindProperty(Events[i]);
			}

			// Properties
			_delayProperty = elementObject.FindProperty("_delay");

			// Loops
			_loopsProperty = elementObject.FindProperty("_loops");
			_loopsInclDelayProperty = elementObject.FindProperty("_includeDelayInLoop");

			// TimeScale
			_timeScaleProps = new SerializedProperty[] 
			{
				_timeScaleValueProperty = elementObject.FindProperty("_timeScale"),
				_timeScaleChannelProperty = elementObject.FindProperty("_timeScaleChannel")
			};
		}

		#region Public Methods

		public void Draw(Action inspectorDraw)
		{
			if(_elementSerializedObject != null && _elementSerializedObject.targetObject != null)
			{
				_elementSerializedObject.Update();

				EditorGUILayout.BeginVertical("box");
				{
					string loopLabel = _loopsProperty.intValue == RaTweenBase.InfiniteLoopingValue ? "∞" : _loopsProperty.intValue.ToString();
					string inclDelayLabel = _loopsInclDelayProperty.boolValue ? "→" : "|";

					EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
					_foldedOutPropertiesProps = DrawFoldout
					(
						_foldedOutPropertiesProps,
						$"Delay [{_delayProperty.floatValue}s] {inclDelayLabel} Looping [{loopLabel}]",
						null, DrawLoopingProperties
					);


					TimeScale.TimeScaleChannelSO channelSO = _timeScaleChannelProperty.objectReferenceValue as TimeScale.TimeScaleChannelSO;
					string channelName = channelSO != null ? channelSO.name : "RealTimeScale";
					float channelTimeScale = channelSO != null ? channelSO.TimeScale : 1f;

					_foldedOutTimeScaleProps = DrawFoldout
					(
						_foldedOutTimeScaleProps,
						$"TimeScale [{_timeScaleValueProperty.floatValue}] | {channelName} [{channelTimeScale}]",
						_timeScaleProps, 
						null
					);

					_foldedOutEventProps = DrawFoldout(_foldedOutEventProps, "Events", _eventProps, null);
				}
				EditorGUILayout.EndVertical();

				_elementSerializedObject.ApplyModifiedProperties();

				inspectorDraw?.Invoke();

				GUILayout.Space(5f);

				if(_element is RaTweenerSerializableElement serElement && serElement.GetTweenInternal() is RaTween tween && tween is IDynamicTween)
				{
					GUILayout.BeginHorizontal();
					GUILayout.Space(12);
					if(GUILayout.Button(new GUIContent("Refresh Values", "Sets the Start and End Value to the current values of the Target for Dynamic Tweens.\n" +
						"Ideal for when you changed a setting in the tween which bases the tween from global to local positon or from units to pixels.\n"), GUILayout.Width(100), GUILayout.Height(20)))
					{
						_element.CalculateDefaultValuesInternal();
					}
					GUILayout.EndHorizontal();
				}

				_elementSerializedObject.ApplyModifiedProperties();
			}
		}

		#endregion

		#region Private Methods

		private void DrawLoopingProperties()
		{
			EditorGUILayout.BeginVertical("box");
			{
				EditorGUILayout.LabelField("Looping", EditorStyles.boldLabel);
				switch(_element.GetLoopAllowStage())
				{
					case LoopAllowStage.None:
						EditorGUILayout.LabelField(new GUIContent("Disabled", "This Tween does not allow for looping"), EditorStyles.boldLabel);
						break;
					case LoopAllowStage.ToFinite:
						_loopsProperty.intValue = Mathf.Max(EditorGUILayout.IntField(new GUIContent(LoopsLabel, _loopsProperty.tooltip), _loopsProperty.intValue), 0);
						_loopsInclDelayProperty.boolValue = EditorGUILayout.Toggle(new GUIContent(InclDelayLoopsLabel, _loopsInclDelayProperty.tooltip), _loopsInclDelayProperty.boolValue);
						break;
					case LoopAllowStage.ToInfinity:
						bool isInfinite = _loopsProperty.intValue == RaTweenBase.InfiniteLoopingValue;

						if(!isInfinite)
						{
							_loopsProperty.intValue = Mathf.Max(EditorGUILayout.IntField(new GUIContent(LoopsLabel, _loopsProperty.tooltip), _loopsProperty.intValue), 0);
						}

						bool isInfiniteNew = EditorGUILayout.Toggle(new GUIContent("IsInfiniteLoop: ", "Makes it so the Tween loops Infinitely.\n It can still be Paused, Stopped and Reset."), isInfinite);

						if(isInfinite != isInfiniteNew)
						{
							if(isInfiniteNew)
							{
								_loopsProperty.intValue = RaTweenBase.InfiniteLoopingValue;
							}
							else
							{
								_loopsProperty.intValue = 0;
							}
						}
						_loopsInclDelayProperty.boolValue = EditorGUILayout.Toggle(new GUIContent(InclDelayLoopsLabel, _loopsInclDelayProperty.tooltip), _loopsInclDelayProperty.boolValue);
						break;
				}
			}
			EditorGUILayout.EndVertical();


			_delayProperty.floatValue = Mathf.Max(0f, EditorGUILayout.FloatField(new GUIContent(_delayProperty.displayName, _delayProperty.tooltip), _delayProperty.floatValue));
		}

		private bool DrawFoldout(bool foldout, string name, IList<SerializedProperty> props, Action customEditorCallback)
		{
			foldout = EditorGUILayout.Foldout(foldout, name);
			if(foldout)
			{
				if(props != null)
				{
					for(int i = 0; i < props.Count; i++)
					{
						SerializedProperty property = props[i];
						EditorGUILayout.PropertyField(property, new GUIContent(property.displayName, property.tooltip));
					}
				}

				customEditorCallback?.Invoke();
			}
			return foldout;
		}

		#endregion
	}
}
#endif