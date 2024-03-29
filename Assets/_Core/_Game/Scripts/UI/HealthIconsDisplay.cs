using Game;
using RaDataHolder;
using RaFSM;
using UnityEngine;

namespace UI
{
	public class HealthIconsDisplay : RaMonoDataHolderBase<Health>
	{
		public const int EMPTY_STATE_INDEX = 0;
		public const int FILLED_STATE_INDEX = 1;

		[SerializeField]
		private RaGOFSMRoot _iconTemplate = null;

		private RaGOFSMRoot[] _icons = null;

		private int _currentFilledDisplayed = 0;

		protected override void OnInitialization()
		{
			base.OnInitialization();
			_iconTemplate.gameObject.SetActive(false);
		}

		protected override void OnSetData()
		{
			_icons = new RaGOFSMRoot[Data.MaxHP];
			for(int i = 0; i < Data.MaxHP; i++)
			{
				RaGOFSMRoot icon = Instantiate(_iconTemplate, _iconTemplate.transform.parent);
				_icons[i] = icon;
				icon.gameObject.SetActive(true);
			}
		}

		protected override void OnSetDataResolved()
		{
			for(int i = 0; i < _icons.Length; i++)
			{
				RaGOFSMRoot icon = _icons[i];
				if(Data.IsFilled(i))
				{
					icon.RunFSM(FILLED_STATE_INDEX);
					_currentFilledDisplayed++;
				}
				else
				{
					icon.RunFSM(EMPTY_STATE_INDEX);
				}
			}

			Data.HealthChangedEvent += OnHealthChangedEvent;
		}

		protected override void OnClearData()
		{
			Data.HealthChangedEvent -= OnHealthChangedEvent;

			for(int i = _icons.Length - 1; i >= 0; i--)
			{
				Destroy(_icons[i].gameObject);
			}
			_icons = null;
		}

		private void OnHealthChangedEvent(Health health, int delta)
		{
			int absDelta = Mathf.Abs(delta);
			if(absDelta > 0)
			{
				int dir = delta / absDelta;
				for(int i = 0; i < absDelta; i++)
				{
					// Old Heart State
					RefreshCurrentIcon();

					_currentFilledDisplayed += dir;

					// New Heart State
					RefreshCurrentIcon();
				}
			}
		}

		private void RefreshCurrentIcon()
		{
			int index = _currentFilledDisplayed - 1;
			if(index >= 0)
			{
				_icons[index].SwitchState(Data.IsFilled(index) ? FILLED_STATE_INDEX : EMPTY_STATE_INDEX);
			}
		}
	}
}
