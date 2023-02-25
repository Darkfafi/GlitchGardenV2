#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RaTweening.Tools
{
	internal static class RaEditorUtils
	{
		public static bool IconButton(Rect rect, string icon, bool isEnabled = true)
		{
			GUI.enabled = isEnabled;
			bool pressed = GUI.Button(rect, EditorGUIUtility.FindTexture(icon), new GUIStyle("label"));
			GUI.enabled = true;
			return pressed;
		}

		public static bool IconButton(string icon, float size, bool isEnabled = true)
		{
			GUI.enabled = isEnabled;
			bool pressed = GUILayout.Button(EditorGUIUtility.FindTexture(icon), new GUIStyle("label"), GUILayout.MaxWidth(size), GUILayout.MaxHeight(size));
			GUI.enabled = true;
			return pressed;
		}

		public static void DrawLabeledLabel(string labelA, string labelB)
		{
			EditorGUILayout.BeginHorizontal();
			{
				EditorGUILayout.LabelField(labelA);
				EditorGUILayout.LabelField(labelB);
			}
			EditorGUILayout.EndHorizontal();
		}
	}
}
#endif
