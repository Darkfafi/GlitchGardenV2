using UnityEngine;

namespace RaTweening.TimeScale
{
	/// <summary>
	/// A <see cref="TimeScaleChannelSO"/> which can be used to manually set the TimeScale of an entire group of tweens and live modify them. \n
	/// > Note: To create an instance of this object, go to `RaTweening/TimeScaleChannels/Create CustomTimeScaleChannelSO` under the Create Context Menu of Unity.
	/// </summary>
	[CreateAssetMenu(menuName = "RaTweening/TimeScaleChannels/Create CustomTimeScaleChannelSO")]
	public class CustomTimeScaleChannelSO : TimeScaleChannelSO
	{
		#region Editor Variables

		[SerializeField]
		private float _timeScale = 1f;

		#endregion

		#region Properties

		public override float TimeScale => _timeScale;

		#endregion

		#region Public Methods

		/// <summary>
		/// Sets the TimeScale of this Channel. Modifying the <see cref="RaTweenBase.TimeScale"/> Tweens referring to this channel.
		/// </summary>
		public void SetTimeScale(float timeScale)
		{
			_timeScale = timeScale;
		}

		#endregion
	}
}
