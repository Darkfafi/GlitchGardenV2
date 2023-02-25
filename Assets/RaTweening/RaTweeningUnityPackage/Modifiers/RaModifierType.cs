namespace RaTweening
{
	/// <summary>
	/// Each of the enum values represents an modifier curve.
	/// </summary>
	public enum RaModifierType
	{
		/// <summary>
		/// No modification is applied
		/// </summary>
		None		= 0,

		/// <summary>
		/// The tween is evaluated in reverse
		/// </summary>
		Reverse		= 1,

		/// <summary>
		/// The tween is evaluated in 50% of the time to the end, and then the other 50% is used to play it in reverse
		/// </summary>
		Yoyo		= 2,
		/// <summary>
		/// The same as <see cref="Yoyo"/> but uses a smoothened curve by using an Absolute Sine Wave Curve
		/// </summary>
		AbsSin		= 3
	}
}