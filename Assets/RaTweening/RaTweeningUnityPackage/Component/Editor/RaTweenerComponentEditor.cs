#if UNITY_EDITOR
using RaTweening.Core.Elements;
using RaTweening.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using RaTweening.Lambda;
using static RaTweening.Tools.RaEditorUtils;

namespace RaTweening.Core
{
	[CustomEditor(typeof(RaTweenerComponent))]
	internal class RaTweenerComponentEditor : Editor
	{
		#region Consts

		private static readonly string[] ExclProps = new string[] { "m_Script" };

		#endregion

		#region Variables

		private static List<RaTweenerElementBase> _elementsToClear = new List<RaTweenerElementBase>();
		private SerializedProperty _tweenElementProperty;
		private Editor _editor;
		private string _name;
		private RaSearchWindow _searchWindow;

		#endregion

		#region Lifecycle

		protected void OnEnable()
		{
			try
			{
				_tweenElementProperty = serializedObject.FindProperty("_tweenElement");
			}
			catch { };
		}

		protected void OnDisable()
		{
			if(_searchWindow != null)
			{
				_searchWindow.Close();
			}
		}

		public override void OnInspectorGUI()
		{
			// Auto Fill Tweener
			if(_tweenElementProperty != null && _tweenElementProperty.objectReferenceValue == null)
			{
				var tweens = GetAllTweenTypes();
				if(tweens.Length > 0)
				{
					var tween = tweens.FirstOrDefault(x => TryGetRaTweenerElementAttribute(x, out _, out _));
					if(tween != null)
					{
						SelectTween(tween);
					}
				}
			}

			EditorGUILayout.BeginHorizontal("box");
			{
				// Select Tweener Button
				if(GUILayout.Button(string.IsNullOrEmpty(_name) ? "Select Tweener" : _name))
				{
					if(_searchWindow != null)
					{
						_searchWindow.Close();
					}

					_searchWindow = CreateTweenSearchWindow((tweenType) =>
					{
						SelectTween(tweenType);
					});
				}
				if(IconButton("CollabEdit Icon", 22))
				{
					RaTweenPlayerEditorWindow.Open(target as RaTweenerComponent);
				}
			}
			EditorGUILayout.EndHorizontal();

			// Draw Editor
			if(_tweenElementProperty != null)
			{
				if(_tweenElementProperty.objectReferenceValue != null)
				{
					if(_editor == null)
					{
						_editor = CreateEditor(_tweenElementProperty.objectReferenceValue);

						RaTweenerElementBase element = _tweenElementProperty.GetValue<RaTweenerElementBase>();

						if(element)
						{
							_name = element.GetName();
						}
					}
				}
				else
				{
					_editor = null;
				}

				if(_editor != null && _editor.serializedObject != null && _editor.serializedObject.targetObject != null)
				{
					_editor.OnInspectorGUI();
					_editor.serializedObject.ApplyModifiedProperties();
				}

				DrawDefaultInspectorWithoutScript(serializedObject);
			}
		}

		#endregion

		#region Public Nethods

		public static void DrawDefaultInspectorWithoutScript(SerializedObject serializedObject)
		{
			if(serializedObject != null && serializedObject.targetObject != null)
			{
				serializedObject.Update();
				DrawPropertiesExcluding(serializedObject, ExclProps);
				serializedObject.ApplyModifiedProperties();
			}
		}

		public static bool TryGetRaTweenerElementAttribute(Type tweenType, out RaTweenerElementAttribute attribute, out string error)
		{
			if(tweenType.GetCustomAttributes(typeof(RaTweenerElementAttribute), true).FirstOrDefault()
				is RaTweenerElementAttribute extractedAttribute)
			{
				if(typeof(RaTweenerElementBase).IsAssignableFrom(extractedAttribute.ElementType))
				{
					if(!extractedAttribute.ElementType.IsAbstract)
					{
						attribute = extractedAttribute;
						error = string.Empty;
						return true;
					}
					else
					{
						error = $"Type {extractedAttribute.ElementType.Name} defined within {nameof(RaTweenerElementAttribute)}, found above {tweenType.Name}, can't be an Abstract";
					}
				}
				else
				{
					error = $"Type {extractedAttribute.ElementType.Name} defined within {nameof(RaTweenerElementAttribute)}, found above {tweenType.Name}, does not derive from {nameof(RaTweenerElementBase)}";
				}
			}
			else
			{
				error = $"No {nameof(RaTweenerElementAttribute)} found above {tweenType.Name}";
			}

			attribute = null;
			return false;
		}

		public static RaSearchWindow CreateTweenSearchWindow(Action<Type> onSelectTween)
		{
			Type[] tweenTypes = GetAllTweenTypes();

			string[] searchTypes = tweenTypes.Select(CreateEntryName).ToArray();

			RaSearchWindow window = null;

			window = RaSearchWindow.OpenWindow((index) =>
			{
				if(index >= 0)
				{
					Type tweenType = tweenTypes[index];
					onSelectTween?.Invoke(tweenType);
					window.Close();
				}
				window = null;
			}, searchTypes);
			return window;

			string CreateEntryName(Type x)
			{
				string newNameSpace = x.Namespace;
				newNameSpace = newNameSpace.Replace(".Ra", ".");
				newNameSpace = newNameSpace.Replace(nameof(RaTweening) + ".", "");
				newNameSpace = newNameSpace.Replace("RaTweening", "");

				string name = x.Name;
				name = name.Replace("RaTween", "");
				name = name.Replace("RaTweening", "");

				return string.Concat(newNameSpace, string.IsNullOrEmpty(newNameSpace) ? "" : " → ", name);
			}
		}

		public static Type[] GetAllTweenTypes()
		{
			return AppDomain.CurrentDomain
					.GetAssemblies()
					.SelectMany(x => x.GetTypes())
					.Where(x => typeof(RaTweenBase).IsAssignableFrom(x) && !x.IsAbstract && !typeof(IRaTweenLambda).IsAssignableFrom(x))
					.ToArray();
		}

		public static bool TryAddTween(SerializedObject obj, Type tweenType, out RaTweenerElementBase elementAdded)
		{
			if(TryGetRaTweenerElementAttribute(tweenType, out RaTweenerElementAttribute attribute, out string error))
			{
				if(obj.targetObject is MonoBehaviour parent)
				{
					try
					{
						RaTweenerElementBase value = parent.gameObject.AddComponent(attribute.ElementType) as RaTweenerElementBase;
						value.Initialize(tweenType);
						elementAdded = value;
						EditorUtility.SetDirty(parent);
					}
					catch(Exception e)
					{
						elementAdded = null;
						EditorUtility.DisplayDialog("Error", e.Message, "Ok");
					}

					obj.ApplyModifiedProperties();
					return elementAdded != null;
				}
				else
				{
					elementAdded = null;
				}
			}
			else
			{
				elementAdded = null;
				EditorUtility.DisplayDialog("Error", error, "Ok");
			}

			return elementAdded != null;
		}

		public static bool TryRemoveTween(RaTweenerElementBase tween, bool inclDelay = true)
		{
			if(tween != null && !_elementsToClear.Contains(tween))
			{
				_elementsToClear.Add(tween);
				if(inclDelay)
				{
					EditorApplication.delayCall += ClearElement;
				}
				else
				{
					ClearElement();
				}
				return true;
			}
			return false;
		}

		#endregion

		#region Private Methods

		private static void ClearElement()
		{
			EditorApplication.delayCall -= ClearElement;

			for(int i = _elementsToClear.Count - 1; i >= 0; i--)
			{
				var element = _elementsToClear[i];
				if(element)
				{
					if(Application.isPlaying)
					{
						Destroy(element);
					}
					else
					{
						DestroyImmediate(element);
					}
				}
			}
			_elementsToClear.Clear();
		}

		private void SelectTween(Type tweenType)
		{
			if(_tweenElementProperty != null)
			{
				if(TryAddTween(serializedObject, tweenType, out RaTweenerElementBase element))
				{
					TryRemoveTween(_tweenElementProperty.GetValue<RaTweenerElementBase>(), false);
					_tweenElementProperty.SetValue(element);
				}
			}

			serializedObject.Update();
			_editor = null;
		}

		#endregion
	}
}
#endif