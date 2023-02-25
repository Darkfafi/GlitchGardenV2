#if UNITY_EDITOR
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using static RaTweening.Tools.RaEditorUtils;

namespace RaTweening.Core.Elements
{
	[CustomEditor(typeof(RaTweenerSequenceElement))]
	internal class RaTweenSequenceEditor : Editor
	{
		#region Variables

		private RaTweenElementEditorDrawer _drawer;
		private ReorderableList _orderableList;
		private SerializedProperty _tweensProperty;
		private bool _editMode = true;
		private Dictionary<Object, Editor> _cachedEditors = new Dictionary<Object, Editor>();
		private bool _isGlobalExpanded = false;

		#endregion

		protected void OnEnable()
		{
			try
			{
				_drawer = new RaTweenElementEditorDrawer(serializedObject);
				_tweensProperty = serializedObject.FindProperty("_sequenceEntries");
				_orderableList = new ReorderableList(serializedObject, _tweensProperty, true, true, false, false);
				_orderableList.drawElementCallback = OnDrawNestedItem;
				_orderableList.drawHeaderCallback = OnDrawHeader;
			}
			catch { }
		}

		#region Public Methods

		public override void OnInspectorGUI()
		{
			_drawer.Draw(()=> 
			{
				RaTweenerSequenceElement self = serializedObject.targetObject as RaTweenerSequenceElement;

				EditorGUILayout.BeginVertical("framebox");
				{
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField(self.GetName(), EditorStyles.boldLabel);
						if(_editMode)
						{
							if(IconButton(_isGlobalExpanded ? "scenevis_visible_hover@2x" : "scenevis_visible@2x", 20f))
							{
								_isGlobalExpanded = !_isGlobalExpanded;

								if(_tweensProperty != null && _tweensProperty.isArray)
								{
									for(int i = 0; i < _tweensProperty.arraySize; i++)
									{
										var element = _tweensProperty.GetArrayElementAtIndex(i);
										if(element != null)
										{
											element.isExpanded = _isGlobalExpanded;
										}
									}
								}
							}
						}
						if(IconButton("CollabCreate Icon", 20))
						{
							RaTweenerComponentEditor.CreateTweenSearchWindow(OnTweenSelectedToAdd);
						}
						if(IconButton(_editMode ? "CollabMoved Icon" : "CollabEdit Icon", 20))
						{
							_editMode = !_editMode;
						}
					}
					EditorGUILayout.EndHorizontal();
					GUILayout.Space(5);

					serializedObject.Update();

					if(_editMode)
					{
						if(_tweensProperty != null && _tweensProperty.isArray)
						{
							for(int i = 0; i < _tweensProperty.arraySize; i++)
							{
								DrawElementEntry(_tweensProperty.GetArrayElementAtIndex(i), i);
							}
						}
					}
					else if(_orderableList != null)
					{
						_orderableList.DoLayoutList();
					}
					serializedObject.ApplyModifiedProperties();
				}
				EditorGUILayout.EndVertical();
			});
		}

		#endregion

		#region Private Methods

		private void OnTweenSelectedToAdd(System.Type tweenType)
		{
			if(RaTweenerComponentEditor.TryAddTween(serializedObject, tweenType, out RaTweenerElementBase element))
			{
				RaTweenerSequenceElement self = serializedObject.targetObject as RaTweenerSequenceElement;
				if(self.RegisterTweenElement(element))
				{
					AssetDatabase.SaveAssets();
					EditorUtility.SetDirty(serializedObject.targetObject);
				}
				else
				{
					RaTweenerComponentEditor.TryRemoveTween(element, true);
				}
			}
		}

		private void DrawElementEntry(SerializedProperty serializedProp, int index)
		{
			var serializedEntry = serializedProp;

			if(serializedEntry == null)
			{
				EditorGUILayout.LabelField("Item Empty");
				return;
			}

			var serializedElement = serializedProp.FindPropertyRelative("TweenElement");
			var serializedStagger = serializedProp.FindPropertyRelative("Stagger");
			var serializedStaggerType = serializedProp.FindPropertyRelative("StaggerType");

			if(serializedElement != null && serializedElement.objectReferenceValue is RaTweenerElementBase element)
			{
				EditorGUILayout.BeginHorizontal("helpBox");
				{
					string oldName = element.GetName();
					EditorGUILayout.BeginHorizontal();
					{
						EditorGUILayout.LabelField(index + ":", GUILayout.Width(25));
						string newName = EditorGUILayout.TextField(oldName);
						if(oldName != newName)
						{
							element.SetName(newName);
						}
					}
					EditorGUILayout.EndHorizontal();
					if(IconButton(serializedProp.isExpanded ? "scenevis_visible_hover@2x" : "scenevis_visible@2x", 20f))
					{
						serializedProp.isExpanded = !serializedProp.isExpanded;
					}
					if(IconButton("CollabDeleted Icon", 20))
					{
						RaTweenerSequenceElement self = serializedObject.targetObject as RaTweenerSequenceElement;
						if(self.UnregisterTweenElement(element))
						{
							RaTweenerComponentEditor.TryRemoveTween(element, false);
							AssetDatabase.SaveAssets();
							EditorUtility.SetDirty(serializedObject.targetObject);
							return;
						}
					}

				}
				EditorGUILayout.EndHorizontal();
				if(serializedProp.isExpanded)
				{
					if(!_cachedEditors.TryGetValue(element, out Editor editor))
					{
						_cachedEditors[element] = editor = CreateEditor(element);
					}
					EditorGUILayout.BeginVertical("frameBox");
					{
						editor.OnInspectorGUI();
						EditorGUILayout.BeginVertical("box");
						{
							EditorGUILayout.LabelField(new GUIContent("Sequence Stagger", "How much % of this tween has to be finished in order to start the next tween in the sequence"), EditorStyles.boldLabel);
							EditorGUILayout.PropertyField(serializedStagger, new GUIContent("Stagger: "));
							EditorGUILayout.PropertyField(serializedStaggerType, new GUIContent("StaggerType: "));
						}
						EditorGUILayout.EndVertical();
						editor.serializedObject.ApplyModifiedProperties();
					}
					EditorGUILayout.EndVertical();
				}
			}
		}

		private void OnDrawHeader(Rect rect)
		{
			GUILayout.BeginHorizontal();
			{
				GUI.Label(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), string.Empty);
			}
			GUILayout.EndHorizontal();
		}

		private void OnDrawNestedItem(Rect rect, int index, bool isActive, bool isFocused)
		{
			SerializedProperty serializedSequenceItem = _orderableList.serializedProperty.GetArrayElementAtIndex(index);
			var elementProp = serializedSequenceItem.FindPropertyRelative("TweenElement");
			RaTweenerElementBase sequenceElement = elementProp.objectReferenceValue as RaTweenerElementBase;

			int currentWidth = 0;

			string oldName = sequenceElement.GetName();
			EditorGUI.LabelField(new Rect(rect.x, rect.y, currentWidth += 25, EditorGUIUtility.singleLineHeight), index + ":");
			string newName = EditorGUI.TextField(new Rect(currentWidth + rect.x, rect.y, 275 - currentWidth, EditorGUIUtility.singleLineHeight), oldName);

			if(oldName != newName)
			{
				sequenceElement.SetName(newName);
			}

			int bWidth = 20;
			if(IconButton(new Rect(rect.x + rect.width - bWidth, rect.y, bWidth, EditorGUIUtility.singleLineHeight), "CollabDeleted Icon"))
			{
				RaTweenerSequenceElement self = serializedObject.targetObject as RaTweenerSequenceElement;
				if(self.UnregisterTweenElement(sequenceElement))
				{
					RaTweenerComponentEditor.TryRemoveTween(sequenceElement, false);
					AssetDatabase.SaveAssets();
					EditorUtility.SetDirty(serializedObject.targetObject);
				}
			}
		}

		#endregion
	}
}
#endif