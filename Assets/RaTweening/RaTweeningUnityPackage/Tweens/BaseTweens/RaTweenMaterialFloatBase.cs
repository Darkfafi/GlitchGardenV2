using RaTweening.Tools;
using System;
using UnityEngine;

namespace RaTweening.Core
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween that is the base class that handles the logics of tweening the Float of a Material
	/// </summary>
	[Serializable]
	public abstract class RaTweenMaterialFloatBase<TargetT> : RaTweenDynamic<TargetT, float>
		where TargetT : UnityEngine.Object
	{
		#region Consts

		public const string DefaultFloatProperty = "_Glossiness";

		#endregion

		#region Editor Variables

		[Header("RaTweenMaterialFloat - Base")]
		[Tooltip("The PropertyType by which to define the Material's Shader Property to affect.\n" +
			"Default: `_Color`")]
		[SerializeField]
		private RaPropertyType _propertyType = RaPropertyType.PropertyName;

		[SerializeField]
		[Tooltip("The Material's Shader PropertyName to affect")]
		[ModifierField(nameof(_propertyType), RaPropertyType.PropertyName, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.None)]
		private string _propertyName = DefaultFloatProperty;

		[SerializeField]
		[Tooltip("The Material's Shader PropertyID to affect\n" +
			"Retrieved by calling `Shader.PropertyToID(propertyName)` on a PropertyName")]
		[ModifierField(nameof(_propertyType), RaPropertyType.PropertyID, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.None)]
		private int _propertyID = 0;

		#endregion

		public RaTweenMaterialFloatBase()
			: base()
		{

		}

		public RaTweenMaterialFloatBase(TargetT target, string propertyName, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{
			SetTargetProperty(propertyName);
		}

		public RaTweenMaterialFloatBase(TargetT target, string propertyName, float endValue, float duration)
			: base(target, endValue, duration)
		{
			SetTargetProperty(propertyName);
		}

		public RaTweenMaterialFloatBase(TargetT target, int propertyID, float startValue, float endValue, float duration)
			: base(target, startValue, endValue, duration)
		{
			SetTargetProperty(propertyID);
		}

		public RaTweenMaterialFloatBase(TargetT target, int propertyID, float endValue, float duration)
			: base(target, endValue, duration)
		{
			SetTargetProperty(propertyID);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the given property of the Target Material is affected by the Tween
		/// > Note: Doing it by ID (Calling <see cref="SetTargetProperty(int)"/>) is more efficient.
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		public RaTweenMaterialFloatBase<TargetT> SetTargetProperty(string propertyName)
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyType.PropertyName;
				_propertyName = propertyName;
				_propertyID = 0;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given property of the Target Material is affected by the Tween
		/// > Note: PropertyID is retrieved by calling `Shader.PropertyToID(propertyName)`
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		public RaTweenMaterialFloatBase<TargetT> SetTargetProperty(int propertyID)
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyType.PropertyID;
				_propertyName = DefaultFloatProperty;
				_propertyID = propertyID;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			_propertyType = RaPropertyType.PropertyName;
			_propertyName = DefaultFloatProperty;
			_propertyID = 0;

			base.SetDefaultValues();
		}

		protected override void DynamicEvaluation(float normalizedValue, TargetT target, float start, float end)
		{
			float delta = end - start;
			WriteValue(target, start + (delta * normalizedValue));
		}

		protected override RaTweenDynamic<TargetT, float> DynamicClone()
		{
			RaTweenMaterialFloatBase<TargetT> tween = MaterialFloatClone();
			tween._propertyType = _propertyType;
			tween._propertyName = _propertyName;
			tween._propertyID = _propertyID;
			return tween;
		}

		protected override float ReadValue(TargetT reference)
		{
			Material material = GetMaterial(reference);
			switch(_propertyType)
			{
				case RaPropertyType.PropertyName:
					return material.GetFloat(_propertyName);
				case RaPropertyType.PropertyID:
					return material.GetFloat(_propertyID);
			}

			throw new NotImplementedException($"Type {_propertyType} does not have a {nameof(ReadValue)} implmenentation");
		}

		protected override float GetEndByDelta(float start, float delta)
		{
			return start + delta;
		}

		protected abstract RaTweenMaterialFloatBase<TargetT> MaterialFloatClone();
		protected abstract Material GetMaterial(TargetT target);

		#endregion

		#region Private Methods

		private void WriteValue(TargetT reference, float value)
		{
			Material material = GetMaterial(reference);
			switch(_propertyType)
			{
				case RaPropertyType.PropertyName:
					material.SetFloat(_propertyName, value);
					break;
				case RaPropertyType.PropertyID:
					material.SetFloat(_propertyID, value);
					break;
				default:
					throw new NotImplementedException($"Type {_propertyType} does not have a {nameof(WriteValue)} implmenentation");
			}
		}

		#endregion
	}
}