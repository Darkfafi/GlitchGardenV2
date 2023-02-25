using RaTweening.RaCamera;
using System;
using UnityEngine;
using static RaTweening.RaRectOptions;

namespace RaTweening.RaCamera
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Rect of a Camera
	/// > Note: You can have it target the Pixel Rect by calling <see cref="RaTweenRect.SetPixelRect(bool)"/> \n
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	[Serializable]
	public class RaTweenRect : RaTweenDynamic<Camera, Rect>
	{
		#region Editor Variables

		[Header("RaTweenRect")]
		[SerializeField]
		[Tooltip("When true, the `Camera.pixelRect` will be affected instead of the `Camera.rect` \n" +
			"The values will represent pixels instead of Units")]
		private bool _isPixelRect = false;

		[SerializeField]
		[Tooltip("Which Values of the Rect to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Values")]
		private RectValue _excludeValues = RectValue.None;

		#endregion

		public RaTweenRect()
			: base()
		{

		}

		public RaTweenRect(Camera target, Rect startRect, Rect endRect, float duration)
			: base(target, startRect, endRect, duration)
		{

		}

		public RaTweenRect(Camera target, Rect endRect, float duration)
			: base(target, endRect, duration)
		{

		}

		#region Public Methods

		/// <summary>
		/// When true, the <see cref="Camera.pixelRect"/> will be affected instead of the <see cref="Camera.rect"/>
		/// > Note: The values will represent pixels instead of Units
		/// </summary>
		/// <param name="enabled"></param>
		/// <returns></returns>
		public RaTweenRect SetPixelRect(bool enabled = true)
		{
			if(CanBeModified())
			{
				_isPixelRect = enabled;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given Rect Values are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeValues">The Rect Values to exclude. This is a flagged value</param>
		public RaTweenRect SetExcludeValues(RectValue excludeValues)
		{
			if(CanBeModified())
			{
				_excludeValues = excludeValues;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given Rect Values which are not specified by the input are excluded. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="inclValues">The Rect Values to include. This is a flagged value</param>
		public RaTweenRect OnlyIncludeRectValues(RectValue inclValues)
		{
			if(CanBeModified())
			{
				_excludeValues = GetOnlyIncludeRectValues(inclValues);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void SetDefaultValues()
		{
			base.SetDefaultValues();
			SetStartValue(Target != null ? ReadValue(Target) : new Rect(Vector2.zero, Vector2.one));
			SetEndValue(new Rect(Vector2.zero, Vector2.one));
		}

		protected override void DynamicEvaluation(float normalizedValue, Camera target, Rect start, Rect end)
		{
			Rect delta = new Rect(end.x - start.x, end.y - start.y, end.width - start.width, end.height - start.height);
			
			delta.x *= normalizedValue;
			delta.y *= normalizedValue;
			delta.width *= normalizedValue;
			delta.height *= normalizedValue;

			WriteValue(target, ApplyExcludeRectValues(ReadValue(target), GetEndByDelta(start, delta), _excludeValues));
		}

		protected override RaTweenDynamic<Camera, Rect> DynamicClone()
		{
			RaTweenRect tween = new RaTweenRect();
			tween._excludeValues = _excludeValues;
			tween._isPixelRect = _isPixelRect;
			return tween;
		}

		protected override Rect ReadValue(Camera reference)
		{
			if(_isPixelRect)
			{
				return reference.pixelRect;
			}

			return reference.rect;
		}

		protected override Rect GetEndByDelta(Rect start, Rect delta)
		{
			return new Rect(start.x + delta.x, start.y + delta.y, start.width + delta.width, start.height + delta.height);
		}

		#endregion

		#region Private Methods

		private void WriteValue(Camera target, Rect value)
		{
			if(_isPixelRect)
			{
				target.pixelRect = value;
			}
			else
			{
				target.rect = value;
			}
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
		/// Tweens the Camera's Rect's X Axis
		/// > Note: You can have it target the Pixel Rect by calling <see cref="RaTweenRect.SetPixelRect(bool)"/> \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The X Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRect TweenRectX(this Camera self, float value, float duration)
		{
			Rect rect = new Rect(value, value, value, value);
			return new RaTweenRect(self, rect, duration)
				.OnlyIncludeRectValues(RectValue.X)
				.Play();
		}

		/// <summary>
		/// Tweens the Camera's Rect's Y Axis
		/// > Note: You can have it target the Pixel Rect by calling <see cref="RaTweenRect.SetPixelRect(bool)"/> \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The Y Axis value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRect TweenRectY(this Camera self, float value, float duration)
		{
			Rect rect = new Rect(value, value, value, value);
			return new RaTweenRect(self, rect, duration)
				.OnlyIncludeRectValues(RectValue.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the Camera's Rect's Width
		/// > Note: You can have it target the Pixel Rect by calling <see cref="RaTweenRect.SetPixelRect(bool)"/> \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The blue value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRect TweenRectWidth(this Camera self, float value, float duration)
		{
			Rect rect = new Rect(value, value, value, value);
			return new RaTweenRect(self, rect, duration)
				.OnlyIncludeRectValues(RectValue.Width)
				.Play();
		}

		/// <summary>
		/// Tweens the Camera's Rect's Height
		/// > Note: You can have it target the Pixel Rect by calling <see cref="RaTweenRect.SetPixelRect(bool)"/> \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="value">The blue value to tween to (value between 0 - 1)</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRect TweenRectHeight(this Camera self, float value, float duration)
		{
			Rect rect = new Rect(value, value, value, value);
			return new RaTweenRect(self, rect, duration)
				.OnlyIncludeRectValues(RectValue.Height)
				.Play();
		}

		/// <summary>
		/// Tweens the Camera's Rect.
		/// > Note: You can have it target the Pixel Rect by calling <see cref="RaTweenRect.SetPixelRect(bool)"/> \n
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="rect">The rect value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRect TweenRect(this Camera self, Rect rect, float duration)
		{
			return new RaTweenRect(self, rect, duration).Play();
		}

		/// <summary>
		/// Tweens the Camera's Rect.
		/// > Note: You can have it target the Pixel Rect by calling <see cref="RaTweenRect.SetPixelRect(bool)"/> \n
		/// </summary>
		/// <param name="startRect">The rect value to tween from</param>
		/// <param name="endRect">The rect value to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenRect TweenRect(this Camera self, Rect startRect, Rect endRect, float duration)
		{
			return new RaTweenRect(self, startRect, endRect, duration).Play();
		}
	}

	#endregion
}