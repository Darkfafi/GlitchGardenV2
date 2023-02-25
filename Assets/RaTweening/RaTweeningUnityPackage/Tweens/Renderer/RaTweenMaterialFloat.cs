using RaTweening.Core;
using RaTweening.RaRenderer;
using UnityEngine;

namespace RaTweening.RaRenderer
{
	/// <summary>
	/// A <see cref="RaTweenMaterialFloatBase{TargetT}"/> tween handles the logics of tweening the Float of a Renderer's Material
	/// > Note: By default this targets the Instance Material. Use <see cref="SetSharedMaterial(bool)"/> to target the Shared Material instead \n
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenMaterialFloat : RaTweenMaterialFloatBase<Renderer>
	{
		#region Editor Variables

		[Header("RaTweenMaterialFloat")]
		[SerializeField]
		private bool _targetSharedMaterial = false;

		#endregion

		public RaTweenMaterialFloat()
			: base()
		{

		}

		public RaTweenMaterialFloat(Renderer target, string propertyName, float startValue, float endValue, float duration)
			: base(target, propertyName, startValue, endValue, duration)
		{

		}

		public RaTweenMaterialFloat(Renderer target, string propertyName, float endValue, float duration)
			: base(target, propertyName, endValue, duration)
		{

		}

		public RaTweenMaterialFloat(Renderer target, int propertyID, float startValue, float endValue, float duration)
			: base(target, propertyID, startValue, endValue, duration)
		{

		}

		public RaTweenMaterialFloat(Renderer target, int propertyID, float endValue, float duration)
			: base(target, propertyID, endValue, duration)
		{

		}

		#region Public Methods

		/// <summary>
		/// When Enabled, it targets the shared material of the renderer. Else it targets the instance material.
		/// </summary>
		/// <param name="enabled">When True, it targets the shared material, else the instance material</param>
		public RaTweenMaterialFloat SetSharedMaterial(bool enabled = true)
		{
			if(CanBeModified())
			{
				_targetSharedMaterial = enabled;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override Material GetMaterial(Renderer target)
		{
			return Tools.MaterialUtils.GetRendererMaterial(target, _targetSharedMaterial);
		}

		protected override RaTweenMaterialFloatBase<Renderer> MaterialFloatClone()
		{
			RaTweenMaterialFloat tween = new RaTweenMaterialFloat();
			tween._targetSharedMaterial = _targetSharedMaterial;
			return tween;
		}

		#endregion
	}
}

namespace RaTweening
{
	#region Extensions

	public static partial class RaTweenUtilExtensions
	{
		/// <summary>
		/// Tweens the Material's Float Property.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialFloat(Renderer, int, float, float)"/>) is more efficient. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialFloat TweenMaterialFloat(this Renderer self, string propertyName, float value, float duration)
		{
			return new RaTweenMaterialFloat(self, propertyName, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Float Property.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialFloat TweenMaterialFloat(this Renderer self, int propertyID, float value, float duration)
		{
			return new RaTweenMaterialFloat(self, propertyID, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Float Property.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialFloat(Renderer, int, float, float, float)"/>) is more efficient. \n
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="startValue">The value to tween from</param>
		/// <param name="endValue">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialFloat TweenMaterialFloat(this Renderer self, string propertyName, float startValue, float endValue, float duration)
		{
			return new RaTweenMaterialFloat(self, propertyName, startValue, endValue, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Float Property.
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="startValue">The value to tween from</param>
		/// <param name="endValue">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialFloat TweenMaterialFloat(this Renderer self, int propertyID, float startValue, float endValue, float duration)
		{
			return new RaTweenMaterialFloat(self, propertyID, startValue, endValue, duration).Play();
		}
	}

	#endregion
}