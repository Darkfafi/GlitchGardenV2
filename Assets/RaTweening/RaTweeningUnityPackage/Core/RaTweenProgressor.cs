using UnityEngine;

namespace RaTweening.Core
{
	internal class RaTweenProgressor
	{
		#region Consts

		public const float InfinityValue = -1f;

		#endregion

		#region Properties

		public float Time
		{
			get; private set;
		}

		public float Duration
		{
			get; private set;
		}

		public float Progress
		{
			get
			{
				if(IsEmpty)
				{
					return 1f;
				}

				if(IsInfinite)
				{
					return InfinityValue;
				}

				return Time / Duration;
			}
		}

		public bool IsCompleted => Mathf.Approximately(Time, Duration);

		public bool IsEmpty => Mathf.Approximately(Duration, 0f);

		public bool IsInfinite => Mathf.Approximately(Duration, InfinityValue);

		#endregion

		public RaTweenProgressor(float duration)
		{
			Time = 0f;
			SetDuration(duration);
		}

		#region Public Methods

		public void SetInfiniteDuration()
		{
			Duration = InfinityValue;
		}

		public void SetDuration(float duration)
		{
			Duration = Mathf.Max(0f, duration);
		}

		public void Reset()
		{
			Time = 0f;
		}

		public float Step(float delta)
		{
			if(IsInfinite)
			{
				Time += delta;
				return 0f;
			}
			else
			{
				float finalUnclamped = Time + delta;
				float remaining = Mathf.Max(0, finalUnclamped - Duration);
				Time = Mathf.Clamp(finalUnclamped, 0f, Duration);
				return remaining;
			}
		}

		public void Complete()
		{
			if(IsInfinite)
			{
				return;
			}

			Time = Duration;
		}

		#endregion
	}
}