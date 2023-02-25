using RaTweening.Core;
using RaTweening.RaMaterial;
using UnityEngine;

namespace RaTweening.RaMaterial
{
	/// <summary>
	/// A <see cref="RaTweenMaterialFloatBase{TargetT}"/> tween handles the logics of tweening the Float of a Material
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenMaterialFloat : RaTweenMaterialFloatBase<Material>
	{
		public RaTweenMaterialFloat()
			: base()
		{

		}

		public RaTweenMaterialFloat(Material target, string propertyName, float startValue, float endValue, float duration)
			: base(target, propertyName, startValue, endValue, duration)
		{

		}

		public RaTweenMaterialFloat(Material target, string propertyName, float endValue, float duration)
			: base(target, propertyName, endValue, duration)
		{

		}

		public RaTweenMaterialFloat(Material target, int propertyID, float startValue, float endValue, float duration)
			: base(target, propertyID, startValue, endValue, duration)
		{

		}

		public RaTweenMaterialFloat(Material target, int propertyID, float endValue, float duration)
			: base(target, propertyID, endValue, duration)
		{

		}

		#region Protected Methods

		protected override Material GetMaterial(Material target)
		{
			return target;
		}

		protected override RaTweenMaterialFloatBase<Material> MaterialFloatClone()
		{
			return new RaTweenMaterialFloat();
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
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialFloat(Material, int, float, float)"/>) is more efficient. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialFloat TweenMaterialFloat(this Material self, string propertyName, float value, float duration)
		{
			return new RaTweenMaterialFloat(self,propertyName, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Float Property.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialFloat TweenMaterialFloat(this Material self, int propertyID, float value, float duration)
		{
			return new RaTweenMaterialFloat(self, propertyID, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Float Property.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialFloat(Material, int, float, float, float)"/>) is more efficient. \n
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="startValue">The value to tween from</param>
		/// <param name="endValue">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialFloat TweenMaterialFloat(this Material self, string propertyName, float startValue, float endValue, float duration)
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
		public static RaTweenMaterialFloat TweenMaterialFloat(this Material self, int propertyID, float startValue, float endValue, float duration)
		{
			return new RaTweenMaterialFloat(self, propertyID, startValue, endValue, duration).Play();
		}
	}

	#endregion
}