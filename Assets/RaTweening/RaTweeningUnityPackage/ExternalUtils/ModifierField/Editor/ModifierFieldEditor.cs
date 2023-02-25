#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace RaTweening.Tools
{
	/// <summary>
	/// Based on: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
	/// </summary>
	[CustomPropertyDrawer(typeof(ModifierFieldAttribute))]
	internal class ModifierFieldEditor : PropertyDrawer
	{
		#region Variables

		// Reference to the attribute on the property.
		private ModifierFieldAttribute _conditionalAttr;

		// Field that is being compared.
		private SerializedProperty _comparedField;

		#endregion

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if(ApplyModification(property) && _conditionalAttr.ModificationType == ModifierFieldAttribute.ModType.DontDraw)
			{
				return 0f;
			}

			return EditorGUI.GetPropertyHeight(property, label, true);
		}

		/// <summary>
		/// Errors default to showing the property.
		/// </summary>
		private bool ApplyModification(SerializedProperty property)
		{
			_conditionalAttr = attribute as ModifierFieldAttribute;

			int count = 0;
			for(int i = 0; i < _conditionalAttr.ComparedPropertyNames.Length; i++)
			{
				string comparedPropertyName = _conditionalAttr.ComparedPropertyNames[i];
				object comparedValue = _conditionalAttr.ComparedValues[i];

				// Replace propertyname to the value from the parameter
				string path = property.propertyPath.Contains(".") ? System.IO.Path.ChangeExtension(property.propertyPath, comparedPropertyName) : comparedPropertyName;

				_comparedField = property.serializedObject.FindProperty(path);

				if(_comparedField == null)
				{
					Debug.LogError("Cannot find property with name: " + path);
					return false;
				}

				bool condition = false;
				// get the value & compare based on types
				switch(_comparedField.type)
				{ // Possible extend cases to support your own type
					case "bool":
						condition = _comparedField.boolValue.Equals(comparedValue);
						break;
					case "Enum":
						condition = _comparedField.enumValueIndex.Equals((int)comparedValue);
						break;
					default:
						Debug.LogError("Error: " + _comparedField.type + " is not supported of " + path);
						break;
				}

				if(condition)
				{
					count++;
				}
			}

			switch(_conditionalAttr.ConditionType)
			{
				case ModifierFieldAttribute.RaConditionType.All:
					return count == _conditionalAttr.ComparedPropertyNames.Length;
				case ModifierFieldAttribute.RaConditionType.Any:
					return count > 0;
				case ModifierFieldAttribute.RaConditionType.None:
					return count == 0;
				default:
					return false;
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			// If the condition is met, simply draw the field.
			if(ApplyModification(property))
			{
				switch(_conditionalAttr.ModificationType)
				{
					case ModifierFieldAttribute.ModType.ReadOnly:
						GUI.enabled = false;
						EditorGUI.PropertyField(position, property, new GUIContent(property.displayName, property.tooltip));
						GUI.enabled = true;
						break;
					case ModifierFieldAttribute.ModType.Rename:
						EditorGUI.PropertyField(position, property, new GUIContent(_conditionalAttr.Rename, property.tooltip));
						break;
					case ModifierFieldAttribute.ModType.DontDraw:
						// Don't Draw
						break;
				}
			}
			else
			{
				EditorGUI.PropertyField(position, property, new GUIContent(property.displayName, property.tooltip));
			}
		}
	}
}
#endif