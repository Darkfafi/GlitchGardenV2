using RaTweening.Core;
using RaTweening.RaMaterial;
using System;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.RaMaterial
{
	/// <summary>
	/// A <see cref="RaTweenMaterialScaleBase{TargetT}"/> tween handles the logics of tweening the TextureScale of a Material
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenMaterialScale : RaTweenMaterialScaleBase<Material>
	{
		public RaTweenMaterialScale()
			: base()
		{

		}

		public RaTweenMaterialScale(Material target, Vector2 startScale, Vector2 endScale, float duration)
			: base(target, startScale, endScale, duration)
		{

		}

		public RaTweenMaterialScale(Material target, Vector2 endScale, float duration)
			: base(target, endScale, duration)
		{

		}

		#region Protected Methods

		protected override RaTweenMaterialScaleBase<Material> MaterialScaleClone()
		{
			return new RaTweenMaterialScale();
		}

		protected override Material GetMaterial(Material target)
		{
			return target;
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
		/// Tweens the Material's TextureScale's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scaleX">The X Axis textureScale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialScale TweenMaterialScaleX(this Material self, float scaleX, float duration)
		{
			RaTweenMaterialScale tween = new RaTweenMaterialScale(self, Vector2.one * scaleX, duration);
			tween.OnlyIncludeAxis(Axis.X).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's TextureScale's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scaleY">The Y Axis textureScale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialScale TweenMaterialScaleY(this Material self, float scaleY, float duration)
		{
			RaTweenMaterialScale tween = new RaTweenMaterialScale(self, Vector2.one * scaleY, duration);
			tween.OnlyIncludeAxis(Axis.Y).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's TextureScale to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scale">The textureScale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialScale TweenMaterialScale(this Material self, Vector2 scale, float duration)
		{
			return new RaTweenMaterialScale(self, scale, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's TextureScale to the given value
		/// </summary>
		/// <param name="startScale">The textureScale to tween from</param>
		/// <param name="endScale">The textureScale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialScale TweenMaterialScale(this Material self, Vector2 startScale, Vector2 endScale, float duration)
		{
			return new RaTweenMaterialScale(self, startScale, endScale, duration).Play();
		}
	}

	#endregion
}