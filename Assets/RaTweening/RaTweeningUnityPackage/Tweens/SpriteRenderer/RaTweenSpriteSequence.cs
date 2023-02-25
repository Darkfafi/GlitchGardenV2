using System;
using UnityEngine;
using RaTweening.RaSpriteRenderer;
using RaTweening.Tools;

namespace RaTweening.RaSpriteRenderer
{
	/// <summary>
	/// This class handles the logics of tweening the Sprite of a SpriteRenderer as a sequence of sprites
	/// > Note: This can be used for sprite animations \n
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenSpriteSequence : RaTween, IRaTweenTarget
	{
		#region Editor Variables

		[Header("Properties")]
		[SerializeField]
		[Tooltip("The Target to Apply the Tween Evaluation To (modify the Sprite of)")]
		private SpriteRenderer _target = default;

		[SerializeField]
		[Tooltip("The Sprites which to apply in sequence based on the Evaluation Curve.\n6 frames on a 6 seconds with a Linear Evaluation will have each Sprite Display exactly 1 second each.")]
		private Sprite[] _sprites = null;

		#endregion

		#region Properties

		public override bool IsValid => _target != null;

		#endregion

		public RaTweenSpriteSequence()
			: base()
		{
		
		}

		public RaTweenSpriteSequence(SpriteRenderer target, Sprite[] sprites, float duration)
			: base(duration)
		{
			_target = target;
			_sprites = sprites;
		}

		#region Public Methods

		public Type GetTargetTypeRaw() => typeof(SpriteRenderer);

		public void SetTargetRaw(object value)
		{
			if(value is SpriteRenderer renderer)
			{
				_target = renderer;
			}
		}

		public RaTweenSpriteSequence SetTarget(SpriteRenderer target)
		{
			if(CanBeModified())
			{
				_target = target;
			}
			return this;
		}

		public RaTweenSpriteSequence SetSprites(Sprite[] sprites)
		{
			if(CanBeModified())
			{
				_sprites = sprites;
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void Dispose()
		{
			_target = default;
			_sprites = default;
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_sprites == null || _sprites.Length == 0)
			{
				return;
			}

			_target.sprite = _sprites[_sprites.GetIndexByNormalizedValue(normalizedValue)];
		}

		protected override RaTween RaTweenClone()
		{
			RaTweenSpriteSequence tween = new RaTweenSpriteSequence();
			tween._sprites = _sprites;
			tween._target = _target;
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
		/// Tweens the SpriteRenderer through a sequence of Sprites.
		/// > Note: A linear sequence would mean a linear animation, a curve would cause the animation to sit on a sprite for longer. \n
		/// > Note: Overshooting in value causes the sprites to wrap around the sequence, so an overshoot of 1.1 would go from the last sprite to the first sprite
		/// </summary>
		/// <param name="sprites">The sequence of sprites to tween over</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenSpriteSequence TweenSpriteSequence(this SpriteRenderer self, Sprite[] sprites, float duration)
		{
			return new RaTweenSpriteSequence(self, sprites, duration).Play();
		}
	}

	#endregion
}