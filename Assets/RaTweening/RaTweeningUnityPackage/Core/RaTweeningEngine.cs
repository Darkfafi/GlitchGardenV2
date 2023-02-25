using UnityEngine;

namespace RaTweening.Core
{
	/// <summary>
	/// This DontDestroyOnLoad - Singleton handles the progressing through all tweens registered to it. 
	/// Tweens are Registered to the engine by calling their <see cref="RaTweenUtilExtensions.Play{TweenT}(TweenT)"/> method.
	/// This class does its work automatically by the internal systems and is not required to be manually called by the user.
	/// > Note: By Default, a tween runs in RealTime. To change it to GameTime, call <see cref="RaTweenUtilExtensions.SetToGameTime{TweenT}(TweenT)"/> on that tween.
	/// </summary>
	public class RaTweeningEngine : MonoBehaviour
	{
		#region Consts

		private const string Name = "<" + nameof(RaTweeningEngine) + ">";

		#endregion

		#region Variables

		private readonly RaTweeningProcessor _processor = new RaTweeningProcessor(true);

		#endregion

		#region Properties

		public static bool HasInstance => _instance != null;

		public static RaTweeningEngine Instance
		{
			get
			{
				if(!HasInstance)
				{
					_instance = new GameObject("<"+nameof(RaTweeningEngine) +">").AddComponent<RaTweeningEngine>();
					DontDestroyOnLoad(_instance);
				}
				return _instance;
			}
		}

		private static RaTweeningEngine _instance = null;

		#endregion

		#region Lifecycle

		protected void LateUpdate()
		{
			_processor.Step(Mathf.Min(Time.unscaledDeltaTime, Time.maximumDeltaTime));
			name = $"{Name} [{_processor.TweensCount}]";
		}

		protected void OnDestroy()
		{
			_processor.Dispose();
		}

		#endregion

		#region Internal Methods

		internal TweenT RegisterTween<TweenT>(TweenT tween)
			where TweenT : RaTweenBase
		{
			return _processor.RegisterTween(tween);
		}

		internal TweenT UnregisterTween<TweenT>(TweenT tween)
			where TweenT : RaTweenBase
		{
			return _processor.UnregisterTween(tween);
		}

		#endregion
	}
}