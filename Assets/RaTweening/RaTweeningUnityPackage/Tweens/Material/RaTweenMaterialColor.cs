using RaTweening.Core;
using RaTweening.RaMaterial;
using UnityEngine;
using static RaTweening.RaColorOptions;

namespace RaTweening.RaMaterial
{
	/// <summary>
	/// A <see cref="RaTweenMaterialColorBase{TargetT}"/> tween handles the logics of tweening the Color of a Material
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenMaterialColor : RaTweenMaterialColorBase<Material>
	{
		public RaTweenMaterialColor()
			: base()
		{

		}

		public RaTweenMaterialColor(Material target, Color startColor, Color endColor, float duration)
			: base(target, startColor, endColor, duration)
		{

		}

		public RaTweenMaterialColor(Material target, Color endColor, float duration)
			: base(target, endColor, duration)
		{

		}

		#region Protected Methods

		protected override Material GetMaterial(Material target)
		{
			return target;
		}

		protected override RaTweenMaterialColorBase<Material> MaterialColorClone()
		{
			return new RaTweenMaterialColor();
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
		/// Tweens the Material's Color's Red Channel
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="red">The red value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialColor TweenMaterialColorR(this Material self, float red, float duration)
		{
			RaTweenMaterialColor color = new RaTweenMaterialColor(self, Color.white * red, duration);
			color.OnlyIncludeChannels(Channel.R).Play();
			return color;
		}

		/// <summary>
		/// Tweens the Material's Color's Green Channel
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="green">The green value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialColor TweenMaterialColorG(this Material self, float green, float duration)
		{
			RaTweenMaterialColor color = new RaTweenMaterialColor(self, Color.white * green, duration);
			color.OnlyIncludeChannels(Channel.G).Play();
			return color;
		}

		/// <summary>
		/// Tweens the Material's Color's Blue Channel
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="blue">The blue value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialColor TweenMaterialColorB(this Material self, float blue, float duration)
		{
			RaTweenMaterialColor color = new RaTweenMaterialColor(self, Color.white * blue, duration);
			color.OnlyIncludeChannels(Channel.B).Play();
			return color;
		}

		/// <summary>
		/// Tweens the Material's Color's Alpha Channel. 
		/// > Note: This can be used for Fading. \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="alpha">The alpha value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialColor TweenMaterialColorA(this Material self, float alpha, float duration)
		{
			RaTweenMaterialColor color = new RaTweenMaterialColor(self, Color.white * alpha, duration);
			color.OnlyIncludeChannels(Channel.A).Play();
			return color;
		}

		/// <summary>
		/// Tweens the Material's Color Property.
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="color">The color value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialColor TweenMaterialColor(this Material self, Color color, float duration)
		{
			return new RaTweenMaterialColor(self, color, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Color Property.
		/// </summary>
		/// <param name="startColor">The color value to tween from</param>
		/// <param name="endColor">The color value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialColor TweenMaterialColor(this Material self, Color startColor, Color endColor, float duration)
		{
			return new RaTweenMaterialColor(self, startColor, endColor, duration).Play();
		}
	}

	#endregion
}