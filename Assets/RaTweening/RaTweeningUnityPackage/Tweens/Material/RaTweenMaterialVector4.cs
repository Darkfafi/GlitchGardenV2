using RaTweening.Core;
using RaTweening.RaMaterial;
using UnityEngine;

namespace RaTweening.RaMaterial
{
	/// <summary>
	/// A <see cref="RaTweenMaterialVector4Base{TargetT}"/> tween handles the logics of tweening the Vector4 Property of a Material
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenMaterialVector4 : RaTweenMaterialVector4Base<Material>
	{
		public RaTweenMaterialVector4()
			: base()
		{

		}

		public RaTweenMaterialVector4(Material target, string propertyName, Vector4 startValue, Vector4 endValue, float duration)
			: base(target, propertyName, startValue, endValue, duration)
		{

		}

		public RaTweenMaterialVector4(Material target, string propertyName, Vector4 endValue, float duration)
			: base(target, propertyName, endValue, duration)
		{

		}

		public RaTweenMaterialVector4(Material target, int propertyID, Vector4 startValue, Vector4 endValue, float duration)
			: base(target, propertyID, startValue, endValue, duration)
		{

		}

		public RaTweenMaterialVector4(Material target, int propertyID, Vector4 endValue, float duration)
			: base(target, propertyID, endValue, duration)
		{

		}

		#region Protected Methods

		protected override Material GetMaterial(Material target)
		{
			return target;
		}

		protected override RaTweenMaterialVector4Base<Material> MaterialVector4Clone()
		{
			return new RaTweenMaterialVector4();
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
		/// Tweens the Material's Vector4 Property's X Axis.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialVector4X(Material, int, float, float)"/>) is more efficient. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="value">The X Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4X(this Material self, string propertyName, float value, float duration)
		{
			RaTweenMaterialVector4 tween = new RaTweenMaterialVector4(self, propertyName, Vector4.one * value, duration);
			tween.OnlyIncludeAxis(RaVector4Options.Axis.X).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property's Y Axis.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialVector4Y(Material, int, float, float)"/>) is more efficient. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="value">The Y Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4Y(this Material self, string propertyName, float value, float duration)
		{
			RaTweenMaterialVector4 tween = new RaTweenMaterialVector4(self, propertyName, Vector4.one * value, duration);
			tween.OnlyIncludeAxis(RaVector4Options.Axis.Y).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property's Z Axis.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialVector4Z(Material, int, float, float)"/>) is more efficient. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="value">The Z Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4Z(this Material self, string propertyName, float value, float duration)
		{
			RaTweenMaterialVector4 tween = new RaTweenMaterialVector4(self, propertyName, Vector4.one * value, duration);
			tween.OnlyIncludeAxis(RaVector4Options.Axis.Z).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property's W Axis.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialVector4W(Material, int, float, float)"/>) is more efficient. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="value">The W Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4W(this Material self, string propertyName, float value, float duration)
		{
			RaTweenMaterialVector4 tween = new RaTweenMaterialVector4(self, propertyName, Vector4.one * value, duration);
			tween.OnlyIncludeAxis(RaVector4Options.Axis.W).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property's X Axis.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="value">The X Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4X(this Material self, int propertyID, float value, float duration)
		{
			RaTweenMaterialVector4 tween = new RaTweenMaterialVector4(self, propertyID, Vector4.one * value, duration);
			tween.OnlyIncludeAxis(RaVector4Options.Axis.X).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property's Y Axis.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="value">The Y Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4Y(this Material self, int propertyID, float value, float duration)
		{
			RaTweenMaterialVector4 tween = new RaTweenMaterialVector4(self, propertyID, Vector4.one * value, duration);
			tween.OnlyIncludeAxis(RaVector4Options.Axis.Y).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property's Z Axis.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="value">The Z Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4Z(this Material self, int propertyID, float value, float duration)
		{
			RaTweenMaterialVector4 tween = new RaTweenMaterialVector4(self, propertyID, Vector4.one * value, duration);
			tween.OnlyIncludeAxis(RaVector4Options.Axis.Z).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property's W Axis.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="value">The W Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4W(this Material self, int propertyID, float value, float duration)
		{
			RaTweenMaterialVector4 tween = new RaTweenMaterialVector4(self, propertyID, Vector4.one * value, duration);
			tween.OnlyIncludeAxis(RaVector4Options.Axis.W).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialVector4(Material, int, Vector4, float)"/>) is more efficient. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4(this Material self, string propertyName, Vector4 value, float duration)
		{
			return new RaTweenMaterialVector4(self, propertyName, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="value">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4(this Material self, int propertyID, Vector4 value, float duration)
		{
			return new RaTweenMaterialVector4(self, propertyID, value, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property.
		/// > Note: Doing it by ID (Calling <see cref="TweenMaterialVector4(Material, int, Vector4, Vector4, float)"/>) is more efficient. \n
		/// </summary>
		/// <param name="propertyName">The name of the property to affect with the Tween</param>
		/// <param name="startValue">The value to tween from</param>
		/// <param name="endValue">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4(this Material self, string propertyName, Vector4 startValue, Vector4 endValue, float duration)
		{
			return new RaTweenMaterialVector4(self, propertyName, startValue, endValue, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Vector4 Property.
		/// </summary>
		/// <param name="propertyID">The ID of the property to affect with the Tween</param>
		/// <param name="startValue">The value to tween from</param>
		/// <param name="endValue">The value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialVector4 TweenMaterialVector4(this Material self, int propertyID, Vector4 startValue, Vector4 endValue, float duration)
		{
			return new RaTweenMaterialVector4(self, propertyID, startValue, endValue, duration).Play();
		}
	}

	#endregion
}