using RaTweening.Tools;
using System;
using UnityEngine;
using static RaTweening.RaColorOptions;

namespace RaTweening.Core
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween that is the base class that handles the logics of tweening the Color of a Material
	/// </summary>
	[Serializable]
	public abstract class RaTweenMaterialColorBase<TargetT> : RaTweenDynamic<TargetT, Color>
		where TargetT : UnityEngine.Object
	{
		#region Consts

		public const string DefaultColorProperty = "_Color";

		#endregion

		#region Editor Variables

		[Header("RaTweenMaterialColorBase - Base")]
		[SerializeField]
		[Tooltip("Which Channels of the Color Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Channels")]
		private Channel _excludeChannels = Channel.None;

		[SerializeField]
		[Tooltip("The PropertyType by which to define the Material's Shader Property to affect.\n" +
			"Default: `_Color`")]
		private RaPropertyDefaultType _propertyType = RaPropertyDefaultType.Default;

		[SerializeField]
		[Tooltip("The Material's Shader PropertyName to affect")]
		[ModifierField(nameof(_propertyType), RaPropertyDefaultType.PropertyName, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.None)]
		private string _propertyName = DefaultColorProperty;

		[SerializeField]
		[Tooltip("The Material's Shader PropertyID to affect\n" +
			"Retrieved by calling `Shader.PropertyToID(propertyName)` on a PropertyName")]
		[ModifierField(nameof(_propertyType), RaPropertyDefaultType.PropertyID, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.None)]
		private int _propertyID = 0;

		#endregion

		public RaTweenMaterialColorBase()
			: base()
		{

		}

		public RaTweenMaterialColorBase(TargetT target, Color startColor, Color endColor, float duration)
			: base(target, startColor, endColor, duration)
		{

		}

		public RaTweenMaterialColorBase(TargetT target, Color endColor, float duration)
			: base(target, endColor, duration)
		{

		}

		#region Public Methods

		/// <summary>
		/// Makes it so the <see cref="DefaultColorProperty"/> of the Target Material is affected by the Tween
		/// </summary>
		public RaTweenMaterialColorBase<TargetT> SetTargetPropertyDefault()
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyDefaultType.Default;
				_propertyName = DefaultColorProperty;
				_propertyID = 0;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given property of the Target Material is affected by the Tween
		/// > Note: Doing it by ID (Calling <see cref="SetTargetProperty(int)"/>) is more efficient.
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		public RaTweenMaterialColorBase<TargetT> SetTargetProperty(string propertyName)
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyDefaultType.PropertyName;
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
		public RaTweenMaterialColorBase<TargetT> SetTargetProperty(int propertyID)
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyDefaultType.PropertyID;
				_propertyName = DefaultColorProperty;
				_propertyID = propertyID;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given color channels are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeChannels">The channels to exclude. This is a flagged value</param>
		public RaTweenMaterialColorBase<TargetT> SetExcludeChannels(Channel excludeChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = excludeChannels;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given color channels which are not specified by the input are excluded. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="inclChannels">The color channels to include. This is a flagged value</param>
		public RaTweenMaterialColorBase<TargetT> OnlyIncludeChannels(Channel inclChannels)
		{
			if(CanBeModified())
			{
				_excludeChannels = GetOnlyIncludeChannels(inclChannels);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			_propertyType = RaPropertyDefaultType.Default;
			_propertyName = DefaultColorProperty;
			_propertyID = 0;

			base.SetDefaultValues();

			SetStartValue(Target != null ? ReadValue(Target) : Color.white);
			SetEndValue(Color.white);
		}

		protected override void DynamicEvaluation(float normalizedValue, TargetT target, Color start, Color end)
		{
			Color delta = end - start;
			WriteValue(target, ApplyExcludeChannels(ReadValue(target), start + (delta * normalizedValue), _excludeChannels));
		}

		protected override RaTweenDynamic<TargetT, Color> DynamicClone()
		{
			RaTweenMaterialColorBase<TargetT> tween = MaterialColorClone();
			tween._propertyType = _propertyType;
			tween._propertyName = _propertyName;
			tween._propertyID = _propertyID;
			tween._excludeChannels = _excludeChannels;
			return tween;
		}

		protected override Color ReadValue(TargetT reference)
		{
			Material material = GetMaterial(reference);
			switch(_propertyType)
			{
				case RaPropertyDefaultType.Default:
					return material.color;
				case RaPropertyDefaultType.PropertyName:
					return material.GetColor(_propertyName);
				case RaPropertyDefaultType.PropertyID:
					return material.GetColor(_propertyID);
			}

			throw new NotImplementedException($"Type {_propertyType} does not have a {nameof(ReadValue)} implmenentation");
		}

		protected override Color GetEndByDelta(Color start, Color delta)
		{
			return start + delta;
		}

		protected abstract RaTweenMaterialColorBase<TargetT> MaterialColorClone();
		protected abstract Material GetMaterial(TargetT target);

		#endregion

		#region Private Methods

		private void WriteValue(TargetT reference, Color value)
		{
			Material material = GetMaterial(reference);
			switch(_propertyType)
			{
				case RaPropertyDefaultType.Default:
					material.color = value;
					break;
				case RaPropertyDefaultType.PropertyName:
					material.SetColor(_propertyName, value);
					break;
				case RaPropertyDefaultType.PropertyID:
					material.SetColor(_propertyID, value);
					break;
				default:
					throw new NotImplementedException($"Type {_propertyType} does not have a {nameof(WriteValue)} implmenentation");
			}
		}

		#endregion
	}
}