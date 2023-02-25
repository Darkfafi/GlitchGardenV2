using RaTweening.Core;
using RaTweening.RaRenderer;
using UnityEngine;
using static RaTweening.RaColorOptions;

namespace RaTweening.RaRenderer
{
	/// <summary>
	/// A <see cref="RaTweenMaterialColorBase{TargetT}"/> tween handles the logics of tweening the Color of a Renderer's Material
	/// > Note: By default this targets the Instance Material. Use <see cref="SetSharedMaterial(bool)"/> to target the Shared Material instead \n
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenMaterialColor : RaTweenMaterialColorBase<Renderer>
	{
		#region Editor Variables

		[Header("RaTweenMaterialColor")]
		[SerializeField]
		private bool _targetSharedMaterial = false;

		#endregion

		public RaTweenMaterialColor()
			: base()
		{

		}

		public RaTweenMaterialColor(Renderer target, Color startColor, Color endColor, float duration)
			: base(target, startColor, endColor, duration)
		{

		}

		public RaTweenMaterialColor(Renderer target, Color endColor, float duration)
			: base(target, endColor, duration)
		{

		}

		#region Public Methods

		/// <summary>
		/// When Enabled, it targets the shared material of the renderer. Else it targets the instance material.
		/// </summary>
		/// <param name="enabled">When True, it targets the shared material, else the instance material</param>
		public RaTweenMaterialColor SetSharedMaterial(bool enabled = true)
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

		protected override RaTweenMaterialColorBase<Renderer> MaterialColorClone()
		{
			RaTweenMaterialColor tween = new RaTweenMaterialColor();
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
		/// Tweens the Material's Color's Red Channel
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="red">The red value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialColor TweenMaterialColorR(this Renderer self, float red, float duration)
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
		public static RaTweenMaterialColor TweenMaterialColorG(this Renderer self, float green, float duration)
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
		public static RaTweenMaterialColor TweenMaterialColorB(this Renderer self, float blue, float duration)
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
		public static RaTweenMaterialColor TweenMaterialColorA(this Renderer self, float alpha, float duration)
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
		public static RaTweenMaterialColor TweenMaterialColor(this Renderer self, Color color, float duration)
		{
			return new RaTweenMaterialColor(self, color, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's Color Property.
		/// </summary>
		/// <param name="startColor">The color value to tween from</param>
		/// <param name="endColor">The color value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialColor TweenMaterialColor(this Renderer self, Color startColor, Color endColor, float duration)
		{
			return new RaTweenMaterialColor(self, startColor, endColor, duration).Play();
		}
	}

	#endregion
}