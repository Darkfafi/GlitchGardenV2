namespace RaTweening
{
	/// <summary>
	/// Each of the enum values represents an evaluation curve.
	/// See https://easings.net for more details on what each of the terms represents
	/// </summary>
	public enum RaEasingType
	{
		// Default
		Linear = 0,

		// Sine
		InSine = 10,
		OutSine = 11,
		InOutSine = 12,

		// Quad
		InQuad = 20,
		OutQuad = 21,
		InOutQuad = 22,

		// Cubic
		InCubic = 30,
		OutCubic = 31,
		InOutCubic = 32,

		// Quart
		InQuart = 40,
		OutQuart = 41,
		InOutQuart = 42,

		// Quint
		InQuint = 50,
		OutQuint = 51,
		InOutQuint = 52,

		// Expo
		InExpo = 60,
		OutExpo = 61,
		InOutExpo = 62,

		// Circ
		InCirc = 70,
		OutCirc = 71,
		InOutCirc = 72,

		// Back
		InBack = 80,
		OutBack = 81,
		InOutBack = 82,

		// Elastic
		InElastic = 90,
		OutElastic = 91,
		InOutElastic = 92,

		// Bounce
		InBounce = 100,
		OutBounce = 101,
		InOutBounce = 102
	}
}