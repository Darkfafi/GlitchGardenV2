using RaTweening.Core;
using RaTweening.RaRenderer;
using System;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.RaRenderer
{
	/// <summary>
	/// A <see cref="RaTweenMaterialOffsetBase{TargetT}"/> tween handles the logics of tweening the TextureOffset of a Renderer's Material
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenMaterialOffset : RaTweenMaterialOffsetBase<Renderer>
	{
		#region Editor Variables

		[Header("RaTweenMaterialOffset")]
		[SerializeField]
		private bool _targetSharedMaterial = false;

		#endregion

		public RaTweenMaterialOffset()
			: base()
		{

		}

		public RaTweenMaterialOffset(Renderer target, Vector2 startOffset, Vector2 endOffset, float duration)
			: base(target, startOffset, endOffset, duration)
		{

		}

		public RaTweenMaterialOffset(Renderer target, Vector2 endOffset, float duration)
			: base(target, endOffset, duration)
		{

		}

		#region Public Methods

		/// <summary>
		/// When Enabled, it targets the shared material of the renderer. Else it targets the instance material.
		/// </summary>
		/// <param name="enabled">When True, it targets the shared material, else the instance material</param>
		public RaTweenMaterialOffset SetSharedMaterial(bool enabled = true)
		{
			if(CanBeModified())
			{
				_targetSharedMaterial = enabled;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenMaterialOffsetBase<Renderer> MaterialOffsetClone()
		{
			RaTweenMaterialOffset tween = new RaTweenMaterialOffset();
			tween._targetSharedMaterial = _targetSharedMaterial;
			return tween;
		}

		protected override Material GetMaterial(Renderer target)
		{
			return Tools.MaterialUtils.GetRendererMaterial(target, _targetSharedMaterial);
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
		/// Tweens the Renderer's TextureOffset's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="offsetX">The X Axis textureOffset to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialOffset TweenRendererOffsetX(this Renderer self, float offsetX, float duration)
		{
			RaTweenMaterialOffset tween = new RaTweenMaterialOffset(self, Vector2.one * offsetX, duration);
			tween.OnlyIncludeAxis(Axis.X).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Renderer's TextureOffset's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="offsetY">The Y Axis textureOffset to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialOffset TweenRendererOffsetY(this Renderer self, float offsetY, float duration)
		{
			RaTweenMaterialOffset tween = new RaTweenMaterialOffset(self, Vector2.one * offsetY, duration);
			tween.OnlyIncludeAxis(Axis.Y).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Renderer's TextureOffset to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="offset">The textureOffset to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialOffset TweenRendererOffset(this Renderer self, Vector2 offset, float duration)
		{
			return new RaTweenMaterialOffset(self, offset, duration).Play();
		}

		/// <summary>
		/// Tweens the Renderer's TextureOffset to the given value
		/// </summary>
		/// <param name="startOffset">The textureOffset to tween from</param>
		/// <param name="endOffset">The textureOffset to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialOffset TweenRendererOffset(this Renderer self, Vector2 startOffset, Vector2 endOffset, float duration)
		{
			return new RaTweenMaterialOffset(self, startOffset, endOffset, duration).Play();
		}
	}

	#endregion
}