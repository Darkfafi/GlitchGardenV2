using RaTweening.Tools;
using System;
using UnityEngine;
using static RaTweening.RaVector4Options;

namespace RaTweening.Core
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween that is the base class that handles the logics of tweening the Vector4 Property of a Material
	/// </summary>
	[Serializable]
	public abstract class RaTweenMaterialVector4Base<TargetT> : RaTweenDynamic<TargetT, Vector4>
		where TargetT : UnityEngine.Object
	{
		#region Consts

		public const string DefaultVector4Property = "_Value";

		#endregion

		#region Editor Variables

		[Header("RaTweenMaterialVector4 - Base")]
		[Tooltip("The PropertyType by which to define the Material's Shader Property to affect.\n" +
			"Default: `_Color`")]
		[SerializeField]
		private RaPropertyType _propertyType = RaPropertyType.PropertyName;

		[SerializeField]
		[Tooltip("The Material's Shader PropertyName to affect")]
		[ModifierField(nameof(_propertyType), RaPropertyType.PropertyName, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.None)]
		private string _propertyName = DefaultVector4Property;

		[SerializeField]
		[ModifierField(nameof(_propertyType), RaPropertyType.PropertyID, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.None)]
		[Tooltip("The Material's Shader PropertyID to affect\n" +
			"Retrieved by calling `Shader.PropertyToID(propertyName)` on a PropertyName")]
		private int _propertyID = 0;

		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenMaterialVector4Base()
			: base()
		{

		}

		public RaTweenMaterialVector4Base(TargetT target, string propertyName, Vector4 startValue, Vector4 endValue, float duration)
			: base(target, startValue, endValue, duration)
		{
			SetTargetProperty(propertyName);
		}

		public RaTweenMaterialVector4Base(TargetT target, string propertyName, Vector4 endValue, float duration)
			: base(target, endValue, duration)
		{
			SetTargetProperty(propertyName);
		}

		public RaTweenMaterialVector4Base(TargetT target, int propertyID, Vector4 startValue, Vector4 endValue, float duration)
			: base(target, startValue, endValue, duration)
		{
			SetTargetProperty(propertyID);
		}

		public RaTweenMaterialVector4Base(TargetT target, int propertyID, Vector4 endValue, float duration)
			: base(target, endValue, duration)
		{
			SetTargetProperty(propertyID);
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the <see cref="DefaultVector4Property"/> of the Target Material is affected by the Tween
		/// </summary>
		public RaTweenMaterialVector4Base<TargetT> SetTargetPropertyDefault()
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyType.PropertyName;
				_propertyName = DefaultVector4Property;
				_propertyID = 0;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given property of the Target Material is affected by the Tween
		/// > Note: Doing it by ID (Calling <see cref="SetTargetProperty(int)"/>) is more efficient.
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		public RaTweenMaterialVector4Base<TargetT> SetTargetProperty(string propertyName)
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
		public RaTweenMaterialVector4Base<TargetT> SetTargetProperty(int propertyID)
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyType.PropertyID;
				_propertyName = DefaultVector4Property;
				_propertyID = propertyID;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenMaterialVector4Base<TargetT> SetExcludeAxis(Axis excludeAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = excludeAxes;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given axes which are not specified by the input are excluded. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="inclAxes">The axes to include. This is a flagged value</param>
		public RaTweenMaterialVector4Base<TargetT> OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			_propertyType = RaPropertyType.PropertyName;
			_propertyName = DefaultVector4Property;
			_propertyID = 0;

			base.SetDefaultValues();
		}

		protected override void DynamicEvaluation(float normalizedValue, TargetT target, Vector4 start, Vector4 end)
		{
			Vector4 delta = end - start;
			WriteValue(target, ApplyExcludeAxes(ReadValue(target), start + (delta * normalizedValue), _excludeAxes));
		}

		protected override RaTweenDynamic<TargetT, Vector4> DynamicClone()
		{
			RaTweenMaterialVector4Base<TargetT> tween = MaterialVector4Clone();
			tween._propertyType = _propertyType;
			tween._propertyName = _propertyName;
			tween._propertyID = _propertyID;
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override Vector4 ReadValue(TargetT reference)
		{
			Material material = GetMaterial(reference);
			switch(_propertyType)
			{
				case RaPropertyType.PropertyName:
					return material.GetVector(_propertyName);
				case RaPropertyType.PropertyID:
					return material.GetVector(_propertyID);
			}

			throw new NotImplementedException($"Type {_propertyType} does not have a {nameof(ReadValue)} implmenentation");
		}

		protected override Vector4 GetEndByDelta(Vector4 start, Vector4 delta)
		{
			return start + delta;
		}

		protected abstract RaTweenMaterialVector4Base<TargetT> MaterialVector4Clone();
		protected abstract Material GetMaterial(TargetT target);

		#endregion

		#region Private Methods

		private void WriteValue(TargetT reference, Vector4 value)
		{
			Material material = GetMaterial(reference);
			switch(_propertyType)
			{
				case RaPropertyType.PropertyName:
					material.SetVector(_propertyName, value);
					break;
				case RaPropertyType.PropertyID:
					material.SetVector(_propertyID, value);
					break;
				default:
					throw new NotImplementedException($"Type {_propertyType} does not have a {nameof(WriteValue)} implmenentation");
			}
		}

		#endregion
	}
}