using RaTweening.Core;
using RaTweening.RaMaterial;
using System;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.RaMaterial
{
	/// <summary>
	/// A <see cref="RaTweenMaterialOffsetBase{TargetT}"/> tween handles the logics of tweening the TextureOffset of a Material
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenMaterialOffset : RaTweenMaterialOffsetBase<Material>
	{
		public RaTweenMaterialOffset()
			: base()
		{

		}

		public RaTweenMaterialOffset(Material target, Vector2 startOffset, Vector2 endOffset, float duration)
			: base(target, startOffset, endOffset, duration)
		{

		}

		public RaTweenMaterialOffset(Material target, Vector2 endOffset, float duration)
			: base(target, endOffset, duration)
		{

		}

		#region Protected Methods

		protected override RaTweenMaterialOffsetBase<Material> MaterialOffsetClone()
		{
			return new RaTweenMaterialOffset();
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
		/// Tweens the Material's TextureOffset's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="offsetX">The X Axis textureOffset to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialOffset TweenMaterialOffsetX(this Material self, float offsetX, float duration)
		{
			RaTweenMaterialOffset tween = new RaTweenMaterialOffset(self, Vector2.one * offsetX, duration);
			tween.OnlyIncludeAxis(Axis.X).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's TextureOffset's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="offsetY">The Y Axis textureOffset to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialOffset TweenMaterialOffsetY(this Material self, float offsetY, float duration)
		{
			RaTweenMaterialOffset tween = new RaTweenMaterialOffset(self, Vector2.one * offsetY, duration);
			tween.OnlyIncludeAxis(Axis.Y).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Material's TextureOffset to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="offset">The textureOffset to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialOffset TweenMaterialOffset(this Material self, Vector2 offset, float duration)
		{
			return new RaTweenMaterialOffset(self, offset, duration).Play();
		}

		/// <summary>
		/// Tweens the Material's TextureOffset to the given value
		/// </summary>
		/// <param name="startOffset">The textureOffset to tween from</param>
		/// <param name="endOffset">The textureOffset to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialOffset TweenMaterialOffset(this Material self, Vector2 startOffset, Vector2 endOffset, float duration)
		{
			return new RaTweenMaterialOffset(self, startOffset, endOffset, duration).Play();
		}
	}

	#endregion
}