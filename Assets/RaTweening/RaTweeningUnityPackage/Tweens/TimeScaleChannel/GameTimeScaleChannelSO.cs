using UnityEngine;

namespace RaTweening.TimeScale
{
	/// <summary>
	/// The ScriptableObject equivalent of <see cref="GameTimeScaleChannel"/>, which represents Unity's `Time.timeScale` value. \n
	/// > Note: For API Support, see <see cref="RaTweenUtilExtensions.SetToGameTime{TweenT}(TweenT)"/>
	/// </summary>
	[CreateAssetMenu(menuName = "RaTweening/TimeScaleChannels/Create GameTimeScaleChannelSO")]
	public class GameTimeScaleChannelSO : TimeScaleChannelSO
	{
		public override float TimeScale => GameTimeScaleChannel.Instance.TimeScale;
	}
}
