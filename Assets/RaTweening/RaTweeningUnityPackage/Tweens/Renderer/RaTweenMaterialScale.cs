using RaTweening.Core;
using RaTweening.RaRenderer;
using System;
using UnityEngine;
using static RaTweening.RaVector2Options;

namespace RaTweening.RaRenderer
{
	/// <summary>
	/// A <see cref="RaTweenMaterialScaleBase{TargetT}"/> tween handles the logics of tweening the TextureScale of a Renderer's Material
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenMaterialScale : RaTweenMaterialScaleBase<Renderer>
	{
		#region Editor Variables

		[Header("RaTweenMaterialScale")]
		[SerializeField]
		private bool _targetSharedMaterial = false;

		#endregion

		public RaTweenMaterialScale()
			: base()
		{

		}

		public RaTweenMaterialScale(Renderer target, Vector2 startScale, Vector2 endScale, float duration)
			: base(target, startScale, endScale, duration)
		{

		}

		public RaTweenMaterialScale(Renderer target, Vector2 endScale, float duration)
			: base(target, endScale, duration)
		{

		}

		#region Public Methods

		/// <summary>
		/// When Enabled, it targets the shared material of the renderer. Else it targets the instance material.
		/// </summary>
		/// <param name="enabled">When True, it targets the shared material, else the instance material</param>
		public RaTweenMaterialScale SetSharedMaterial(bool enabled = true)
		{
			if(CanBeModified())
			{
				_targetSharedMaterial = enabled;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenMaterialScaleBase<Renderer> MaterialScaleClone()
		{
			RaTweenMaterialScale tween = new RaTweenMaterialScale();
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
		/// Tweens the Renderer's TextureScale's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scaleX">The X Axis textureScale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialScale TweenRendererScaleX(this Renderer self, float scaleX, float duration)
		{
			RaTweenMaterialScale tween = new RaTweenMaterialScale(self, Vector2.one * scaleX, duration);
			tween.OnlyIncludeAxis(Axis.X).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Renderer's TextureScale's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scaleY">The Y Axis textureScale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialScale TweenRendererScaleY(this Renderer self, float scaleY, float duration)
		{
			RaTweenMaterialScale tween = new RaTweenMaterialScale(self, Vector2.one * scaleY, duration);
			tween.OnlyIncludeAxis(Axis.Y).Play();
			return tween;
		}

		/// <summary>
		/// Tweens the Renderer's TextureScale to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="scale">The textureScale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialScale TweenRendererScale(this Renderer self, Vector2 scale, float duration)
		{
			return new RaTweenMaterialScale(self, scale, duration).Play();
		}

		/// <summary>
		/// Tweens the Renderer's TextureScale to the given value
		/// </summary>
		/// <param name="startScale">The textureScale to tween from</param>
		/// <param name="endScale">The textureScale to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenMaterialScale TweenRendererScale(this Renderer self, Vector2 startScale, Vector2 endScale, float duration)
		{
			return new RaTweenMaterialScale(self, startScale, endScale, duration).Play();
		}
	}

	#endregion
}