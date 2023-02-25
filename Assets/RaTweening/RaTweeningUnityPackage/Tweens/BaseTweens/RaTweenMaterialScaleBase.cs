using RaTweening.Tools;
using System;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.Core
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween that is the base class that handles the logics of tweening the TextureScale of a Material
	/// </summary>
	[Serializable]
	public abstract class RaTweenMaterialScaleBase<TargetT> : RaTweenDynamic<TargetT, Vector2>
		where TargetT : UnityEngine.Object
	{
		#region Consts

		public const string DefaultVector2Property = "_MainTex";

		#endregion

		#region Editor Variables

		[Header("RaTweenMaterialScale - Base")]
		[Tooltip("The PropertyType by which to define the Material's Shader Property to affect.\n" +
			"Default: `_Color`")]
		[SerializeField]
		private RaPropertyDefaultType _propertyType = RaPropertyDefaultType.Default;

		[SerializeField]
		[Tooltip("The Material's Shader PropertyName to affect")]
		[ModifierField(nameof(_propertyType), RaPropertyDefaultType.PropertyName, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.None)]
		private string _propertyName = DefaultVector2Property;

		[SerializeField]
		[ModifierField(nameof(_propertyType), RaPropertyDefaultType.PropertyID, ModifierFieldAttribute.ModType.DontDraw, ModifierFieldAttribute.RaConditionType.None)]
		[Tooltip("The Material's Shader PropertyID to affect\n" +
			"Retrieved by calling `Shader.PropertyToID(propertyName)` on a PropertyName")]
		private int _propertyID = 0;

		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenMaterialScaleBase()
			: base()
		{

		}

		public RaTweenMaterialScaleBase(TargetT target, Vector2 startValue, Vector2 endValue, float duration)
			: base(target, startValue, endValue, duration)
		{

		}

		public RaTweenMaterialScaleBase(TargetT target, Vector2 endValue, float duration)
			: base(target, endValue, duration)
		{
		}

		#region Public Methods

		/// <summary>
		/// Makes it so the <see cref="DefaultVector2Property"/> of the Target Material is affected by the Tween
		/// </summary>
		public RaTweenMaterialScaleBase<TargetT> SetTargetPropertyDefault()
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyDefaultType.Default;
				_propertyName = DefaultVector2Property;
				_propertyID = 0;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given property of the Target Material is affected by the Tween
		/// > Note: Doing it by ID (Calling <see cref="SetTargetProperty(int)"/>) is more efficient.
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		public RaTweenMaterialScaleBase<TargetT> SetTargetProperty(string propertyName)
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
		public RaTweenMaterialScaleBase<TargetT> SetTargetProperty(int propertyID)
		{
			if(CanBeModified())
			{
				_propertyType = RaPropertyDefaultType.PropertyID;
				_propertyName = DefaultVector2Property;
				_propertyID = propertyID;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenMaterialScaleBase<TargetT> SetExcludeAxis(Axis excludeAxes)
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
		public RaTweenMaterialScaleBase<TargetT> OnlyIncludeAxis(Axis inclAxes)
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
			_propertyType = RaPropertyDefaultType.Default;
			_propertyName = DefaultVector2Property;
			_propertyID = 0;

			base.SetDefaultValues();
		}

		protected override void DynamicEvaluation(float normalizedValue, TargetT target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			WriteValue(target, ApplyExcludeAxes(ReadValue(target), start + (delta * normalizedValue), _excludeAxes));
		}

		protected override RaTweenDynamic<TargetT, Vector2> DynamicClone()
		{
			RaTweenMaterialScaleBase<TargetT> tween = MaterialScaleClone();
			tween._propertyType = _propertyType;
			tween._propertyName = _propertyName;
			tween._propertyID = _propertyID;
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override Vector2 ReadValue(TargetT reference)
		{
			Material material = GetMaterial(reference);
			switch(_propertyType)
			{
				case RaPropertyDefaultType.Default:
					return material.mainTextureScale;
				case RaPropertyDefaultType.PropertyName:
					return material.GetTextureScale(_propertyName);
				case RaPropertyDefaultType.PropertyID:
					return material.GetTextureScale(_propertyID);
			}

			throw new NotImplementedException($"Type {_propertyType} does not have a {nameof(ReadValue)} implmenentation");
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected abstract RaTweenMaterialScaleBase<TargetT> MaterialScaleClone();
		protected abstract Material GetMaterial(TargetT target);

		#endregion

		#region Private Methods

		private void WriteValue(TargetT reference, Vector2 value)
		{
			Material material = GetMaterial(reference);
			switch(_propertyType)
			{
				case RaPropertyDefaultType.Default:
					material.mainTextureScale = value;
					break;
				case RaPropertyDefaultType.PropertyName:
					material.SetTextureScale(_propertyName, value);
					break;
				case RaPropertyDefaultType.PropertyID:
					material.SetTextureScale(_propertyID, value);
					break;
				default:
					throw new NotImplementedException($"Type {_propertyType} does not have a {nameof(WriteValue)} implmenentation");
			}
		}

		#endregion
	}
}