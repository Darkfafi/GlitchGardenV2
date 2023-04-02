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
					icon.SwitchState(FILLED_STATE_INDEX);
					_currentFilledDisplayed++;
				}
				else
				{
					icon.SwitchState(EMPTY_STATE_INDEX);
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
			int dir = delta / absDelta;
			for(int i = 0; i < absDelta; i++)
			{
				_currentFilledDisplayed += dir;
				_icons[_currentFilledDisplayed].SwitchState(Data.IsFilled(_currentFilledDisplayed) ? FILLED_STATE_INDEX : EMPTY_STATE_INDEX);
			}
		}
	}
}
