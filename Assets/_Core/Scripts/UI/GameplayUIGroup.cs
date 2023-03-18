using RaTweening;
using UnityEngine;

namespace UI
{
	public class GameplayUIGroup : UsableElementBase
	{
		[field: SerializeField]
		public UnitsHUD UnitsHUD
		{
			get; private set;
		}

		[field: SerializeField]
		public WalletCurrencyUIElement ResourcesHUD
		{
			get; private set;
		}

		[SerializeField]
		private CanvasGroup _content = null;

		private RaTweenBase _tween = null;

		protected override void OnInitialization()
		{
			_content.gameObject.SetActive(false);
			_content.alpha = 0;
		}

		protected override void OnUseStarted()
		{
			if(_tween != null)
			{
				_tween.Stop();
				_tween = null;
			}

			_tween = _content.TweenAlpha(1f, 0.4f).OnStart(()=> { _content.gameObject.SetActive(true); });
		}

		protected override void OnUseEnded()
		{
			if(_tween != null)
			{
				_tween.Stop();
				_tween = null;
			}

			_tween = _content.TweenAlpha(0f, 0.4f).OnComplete(() => { _content.gameObject.SetActive(false); });
		}

		protected override void OnDeinitialization()
		{
			if(_tween != null)
			{
				_tween.Stop();
				_tween = null;
			}

			_content.gameObject.SetActive(false);
			_content.alpha = 0;
		}
	}
}