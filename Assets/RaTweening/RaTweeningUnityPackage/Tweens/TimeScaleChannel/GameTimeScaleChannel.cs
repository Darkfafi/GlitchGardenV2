using UnityEngine;

namespace RaTweening.TimeScale
{
	/// <summary>
	/// This Singleton <see cref="ITimeScaleChannel"/> represents Unity's `Time.timeScale` value. \n
	/// > Note: For <see cref="RaTweenerComponent"/> support, see <see cref="GameTimeScaleChannelSO"/> \n
	/// > Note: For API Support, see <see cref="RaTweenUtilExtensions.SetToGameTime{TweenT}(TweenT)"/>
	/// </summary>
	public class GameTimeScaleChannel : ITimeScaleChannel
	{
		public static GameTimeScaleChannel Instance
		{
			get
			{
				if(_instance == null)
				{
					_instance = new GameTimeScaleChannel();
				}

				return _instance;
			}	
		}

		private static GameTimeScaleChannel _instance = null;

		/// <summary>
		/// Time.timeScale
		/// </summary>
		public float TimeScale => Time.timeScale;

		private GameTimeScaleChannel()
		{
		
		}
	}
}