using UnityEngine;

namespace RaTweening.Core
{
	/// <summary>
	/// Util class that handles the logics of <see cref="RaEasingType"/>
	/// </summary>
	public static class RaTweenEasing
	{
		#region Consts

		private const float C1 = 1.70158f;
		private const float C2 = C1 * 1.525f;
		private const float C3 = C1 + 1f;
		private const float C4 = (2f * Mathf.PI) / 3f;
		private const float C5 = (2f * Mathf.PI) / 4.5f;

		private const float N1 = 7.5625f;
		private const float D1 = 2.75f;

		#endregion

		#region Public Methods

		/// <summary>
		/// Gives the Evaluation value of x, where x represents the time and the value returned is the current value within the evaluated curve
		/// </summary>
		/// <param name="easingType">The EasingType to evaluate over x</param>
		/// <param name="x">The horizontal time axis within the curve, from which the value is desired.</param>
		/// <returns>The Value on x on the curve of the EasingType</returns>
		public static float Evaluate(RaEasingType easingType, float x)
		{
			switch(easingType)
			{
				// Sine
				case RaEasingType.InSine:
					return 1f - Mathf.Cos((x * Mathf.PI) / 2f);
				case RaEasingType.OutSine:
					return Mathf.Sin((x * Mathf.PI) / 2f);
				case RaEasingType.InOutSine:
					return -(Mathf.Cos(Mathf.PI * x) - 1f) / 2f;

				// Quad
				case RaEasingType.InQuad:
					return x * x;
				case RaEasingType.OutQuad:
					return 1f - (1f - x) * (1f - x);
				case RaEasingType.InOutQuad:
					return x < 0.5f ? 2f * x * x : 1f - Mathf.Pow(-2f * x + 2f, 2f) / 2f;

				// Cubic
				case RaEasingType.InCubic:
					return x * x * x;
				case RaEasingType.OutCubic:
					return 1f - Mathf.Pow(1f - x, 3f);
				case RaEasingType.InOutCubic:
					return x < 0.5f ? 4f * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 3f) / 2f;

				// Quart
				case RaEasingType.InQuart:
					return x * x * x * x;
				case RaEasingType.OutQuart:
					return 1f - Mathf.Pow(1f - x, 4f);
				case RaEasingType.InOutQuart:
					return x < 0.5f ? 8f * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 4f) / 2f;

				// Quint
				case RaEasingType.InQuint:
					return x * x * x * x * x;
				case RaEasingType.OutQuint:
					return 1f - Mathf.Pow(1f - x, 5f);
				case RaEasingType.InOutQuint:
					return x < 0.5f ? 16f * x * x * x * x * x : 1f - Mathf.Pow(-2f * x + 2f, 6f) / 2f;

				// Expo
				case RaEasingType.InExpo:
					return Mathf.Approximately(x, 0f) ? 0f : Mathf.Pow(2f, 10f * x - 10f);
				case RaEasingType.OutExpo:
					return Mathf.Approximately(x, 1f) ? 1f : 1f - Mathf.Pow(2f, -10f * x);
				case RaEasingType.InOutExpo:
					return Mathf.Approximately(x, 0f)
						  ? 0f
						  : Mathf.Approximately(x, 1f)
						  ? 1f
						  : x < 0.5f ? Mathf.Pow(2f, 20f * x - 10f) / 2f
						  : (2f - Mathf.Pow(2f, -20f * x + 10f)) / 2f;

				// Circ
				case RaEasingType.InCirc:
					return 1f - Mathf.Sqrt(1f - Mathf.Pow(x, 2f));
				case RaEasingType.OutCirc:
					return Mathf.Sqrt(1f - Mathf.Pow(x - 1f, 2f));
				case RaEasingType.InOutCirc:
					return x < 0.5f
						  ? (1f - Mathf.Sqrt(1f - Mathf.Pow(2f * x, 2f))) / 2f
						  : (Mathf.Sqrt(1f - Mathf.Pow(-2f * x + 2f, 2f)) + 1f) / 2f;
				// Back
				case RaEasingType.InBack:
					return C3 * x * x * x - C1 * x * x;
				case RaEasingType.OutBack:
					return 1f + C3 * Mathf.Pow(x - 1f, 3f) + C1 * Mathf.Pow(x - 1f, 2f);
				case RaEasingType.InOutBack:
					return x < 0.5f
						  ? (Mathf.Pow(2f * x, 2f) * ((C2 + 1f) * 2f * x - C2)) / 2f
						  : (Mathf.Pow(2f * x - 2f, 2f) * ((C2 + 1f) * (x * 2f - 2f) + C2) + 2f) / 2f;

				// Elastic
				case RaEasingType.InElastic:
					return Mathf.Approximately(x, 0f)
						  ? 0f
						  : Mathf.Approximately(x, 1f)
						  ? 1f
						  : -Mathf.Pow(2f, 10f * x - 10f) * Mathf.Sin((x * 10f - 10.75f) * C4);
				case RaEasingType.OutElastic:
					return Mathf.Approximately(x, 0f)
						  ? 0f
						  : Mathf.Approximately(x, 1f)
						  ? 1f
						  : Mathf.Pow(2f, -10f * x) * Mathf.Sin((x * 10f - 0.75f) * C4) + 1f;
				case RaEasingType.InOutElastic:
					return Mathf.Approximately(x, 0f)
						  ? 0f
						  : Mathf.Approximately(x, 1f)
						  ? 1f
						  : x < 0.5f
						  ? -(Mathf.Pow(2f, 20f * x - 10f) * Mathf.Sin((20f * x - 11.125f) * C5)) / 2f
						  : (Mathf.Pow(2f, -20f * x + 10f) * Mathf.Sin((20f * x - 11.125f) * C5)) / 2f + 1f;

				// Bounce
				case RaEasingType.InBounce:
					return 1f - OutBounce(1f - x);
				case RaEasingType.OutBounce:
					return OutBounce(x);
				case RaEasingType.InOutBounce:
					return x < 0.5f
						  ? (1f - OutBounce(1f - 2f * x)) / 2f
						  : (1f + OutBounce(2f * x - 1f)) / 2f;

				// Default
				case RaEasingType.Linear:
					return x;

				// Not Implemented Exception
				default:
					throw new System.NotImplementedException($"{nameof(RaEasingType)} {easingType} not implemented");
			}

			float OutBounce(float v)
			{
				if(v < 1f / D1)
				{
					return N1 * v * v;
				}
				else if(v < 2f / D1)
				{
					return N1 * (v -= 1.5f / D1) * v + 0.75f;
				}
				else if(v < 2.5f / D1)
				{
					return N1 * (v -= 2.25f / D1) * v + 0.9375f;
				}
				else
				{
					return N1 * (v -= 2.625f / D1) * v + 0.984375f;
				}
			}
		}

		#endregion
	}
}