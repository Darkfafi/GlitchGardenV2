using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ConfigBase), true)]
public class ConfigEditor : Editor
{
    public override void OnInspectorGUI()
	{
		EditorGUI.indentLevel--;
		SerializedProperty dataProperty = serializedObject.FindProperty("_data");
		dataProperty.isExpanded = true;
		base.OnInspectorGUI();

		EditorGUI.indentLevel++;

	}
}
