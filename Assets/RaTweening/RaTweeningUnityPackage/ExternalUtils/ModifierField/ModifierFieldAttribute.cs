using UnityEngine;
using System;

namespace RaTweening.Tools
{
	/// <summary>
	/// Draws the field/property ONLY if the compared property compared by the comparison type with the value of comparedValue returns true.
	/// Based on: https://forum.unity.com/threads/draw-a-field-only-if-a-condition-is-met.448855/
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
	internal class ModifierFieldAttribute : PropertyAttribute
	{
		#region Properties

		public string[] ComparedPropertyNames
		{
			get; private set;
		}

		public object[] ComparedValues
		{
			get; private set;
		}

		public ModType ModificationType
		{
			get; private set;
		}

		public string Rename
		{
			get; private set;
		}

		public RaConditionType ConditionType
		{
			get; private set;
		}

		/// <summary>
		/// Types of comperisons.
		/// </summary>
		public enum ModType
		{
			Rename = 1,
			ReadOnly = 10,
			DontDraw = 100,
		}

		#endregion

		public ModifierFieldAttribute(string[] comparedPropertyNames, object[] comparedValues, ModType disablingType = ModType.DontDraw, RaConditionType conditionType = RaConditionType.Any)
		{
			ComparedPropertyNames = comparedPropertyNames;
			ComparedValues = comparedValues;
			ModificationType = disablingType;
			ConditionType = conditionType;
		}

		public ModifierFieldAttribute(string comparedPropertyName, object comparedValue, ModType disablingType = ModType.DontDraw, RaConditionType conditionType = RaConditionType.Any)
		{
			ComparedPropertyNames	= new string[] { comparedPropertyName };
			ComparedValues			= new object[] { comparedValue };
			ModificationType		= disablingType;
			ConditionType			= conditionType;
		}

		public ModifierFieldAttribute(string comparedPropertyName, object comparedValue, string rename, RaConditionType conditionType = RaConditionType.Any)
		{
			ComparedPropertyNames	= new string[] { comparedPropertyName };
			ComparedValues			= new object[] { comparedValue };
			ModificationType		= ModType.Rename;
			Rename					= rename;
			ConditionType			= conditionType;
		}

		public enum RaConditionType
		{
			Any,
			All,
			None
		}
	}
}