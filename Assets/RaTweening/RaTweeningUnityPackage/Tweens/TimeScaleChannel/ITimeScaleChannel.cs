namespace RaTweening.TimeScale
{
	/// <summary>
	/// Represents a channel which indicates the Scale at which the Real-Time DeltaTime should be scaled for a Tween.
	/// > Note: This affects the evaluation speed of all the tweens referring to the channel.
	/// </summary>
	public interface ITimeScaleChannel
	{
		/// <summary>
		/// The TimeScale with which scale the Real-Time DeltaTime which is used to evaluate a tween
		/// </summary>
		float TimeScale
		{
			get;
		}
	}
}