using RaTweening.UI.RaShadow;
using UnityEngine;
using UnityEngine.UI;
using static RaTweening.RaVector2Options;

namespace RaTweening.UI.RaShadow
{
	/// <summary>
	/// A <see cref="RaTweenDynamic{TargetT, ValueT}"/> tween handles the logics of tweening the Distance of a Shadow
	/// > Note: <see cref="RaTweenUtilExtensions"/> for all extension methods
	/// </summary>
	public class RaTweenDistance : RaTweenDynamic<Shadow, Vector2>
	{
		#region Editor Variables

		[Header("RaTweenDistance")]
		[SerializeField]
		[Tooltip("Which Axes of the Vector Value to exclude from the Tween.\n" +
			"This allows for other tweens, animations or code to affect the excluded Axes")]
		private Axis _excludeAxes = Axis.None;

		#endregion

		public RaTweenDistance()
			: base()
		{

		}

		public RaTweenDistance(Shadow target, Vector2 startDistance, Vector2 endDistance, float duration)
			: base(target, startDistance, endDistance, duration)
		{

		}

		public RaTweenDistance(Shadow target, Vector2 endDistance, float duration)
			: base(target, endDistance, duration)
		{

		}

		public RaTweenDistance(Shadow target, Vector2 startDistance, Shadow endDistance, float duration)
			: base(target, startDistance, default, duration)
		{
			SetEndRef(endDistance);
		}

		#region Public Methods


		/// <summary>
		/// Makes it so the given axes are excluded from the tween. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="excludeAxes">The axes to exclude. This is a flagged value</param>
		public RaTweenDistance SetExcludeAxis(Axis excludeAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = excludeAxes;
			}
			return this;
		}

		/// <summary>
		/// Makes it so the given axes which are not specified by the input are excluded. So they remain the value they have at the moments of evaluation
		/// </summary>
		/// <param name="inclAxes">The axes to include. This is a flagged value</param>
		public RaTweenDistance OnlyIncludeAxis(Axis inclAxes)
		{
			if(CanBeModified())
			{
				_excludeAxes = GetOnlyIncludeAxes(inclAxes);
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override RaTweenDynamic<Shadow, Vector2> DynamicClone()
		{
			RaTweenDistance tween = new RaTweenDistance();
			tween._excludeAxes = _excludeAxes;
			return tween;
		}

		protected override void DynamicEvaluation(float normalizedValue, Shadow target, Vector2 start, Vector2 end)
		{
			Vector2 delta = end - start;
			target.effectDistance = ApplyExcludeAxes(target.effectDistance, start + (delta * normalizedValue), _excludeAxes);
		}

		protected override Vector2 GetEndByDelta(Vector2 start, Vector2 delta)
		{
			return start + delta;
		}

		protected override Vector2 ReadValue(Shadow reference)
		{
			return reference.effectDistance;
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
		/// Tweens the Shadow's Distance's X Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="distanceX">The X Axis distance to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenDistance TweenDistanceX(this Shadow self, float distanceX, float duration)
		{
			return new RaTweenDistance(self, Vector2.one * distanceX, duration)
				.OnlyIncludeAxis(Axis.X)
				.Play();
		}

		/// <summary>
		/// Tweens the Shadow's Distance's Y Axis to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="distanceY">The Y Axis distance to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenDistance TweenDistanceY(this Shadow self, float distanceY, float duration)
		{
			return new RaTweenDistance(self, Vector2.one * distanceY, duration)
				.OnlyIncludeAxis(Axis.Y)
				.Play();
		}

		/// <summary>
		/// Tweens the Shadow's Distance to the given value
		/// > Note: Through this method, the <see cref="RaTweenDynamic{TargetT, ValueT}.SetStartDynamic(bool)"/> is set to true
		/// </summary>
		/// <param name="distance">The distance to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenDistance TweenDistance(this Shadow self, Vector2 distance, float duration)
		{
			return new RaTweenDistance(self, distance, duration).Play();
		}

		/// <summary>
		/// Tweens the Shadow's Distance to the given value
		/// </summary>
		/// <param name="startDistance">The distance to tween from</param>
		/// <param name="endDistance">The distance to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenDistance TweenDistance(this Shadow self, Vector2 startDistance, Vector2 endDistance, float duration)
		{
			return new RaTweenDistance(self, startDistance, endDistance, duration).Play();
		}

		/// <summary>
		/// Tweens the Shadow's Distance to the given value
		/// </summary>
		/// <param name="startDistance">The distance to tween from</param>
		/// <param name="endTarget">The reference to which's distance to tween to</param>
		/// <param name="duration">The duration of the tween in seconds</param>
		public static RaTweenDistance TweenDistance(this Shadow self, Vector2 startDistance, Shadow endTarget, float duration)
		{
			return new RaTweenDistance(self, startDistance, endTarget, duration).Play();
		}
	}

	#endregion
}