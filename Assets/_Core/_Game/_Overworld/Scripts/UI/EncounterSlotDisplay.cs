using RaDataHolder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using RaFSM;

namespace Game.Campaign
{
	public class EncounterSlotDisplay : RaMonoDataHolderBase<EncounterSlotModel>
	{
		public const int EMPTY_SLOT_INDEX = 0;
		public const int FILLED_SLOT_INDEX = 1;

		public delegate void SlotHandler(EncounterSlotDisplay display);
		public event SlotHandler SlotActionPressedEvent;

		[Header("Encounter")]
		[SerializeField]
		private TMP_Text _titleLabel = null;

		[SerializeField]
		private TMP_Text _descriptionLabel = null;

		[SerializeField]
		private Image _encounterIconImage = null;

		[SerializeField]
		private GameObject _runningDisplay = null;

		[Header("Encounter - Type")]
		[SerializeField]
		private Image _encounterTypeIconImage = null;
		[SerializeField]
		private TMP_Text _encounterTypeActionLabel = null;

		private RaGOFiniteStateMachine _fsm = null;

		public EncounterConfig EncounterConfig => HasEncounter ? Data.Encounter.Config : null;
		public bool HasEncounter => Data.Encounter != null;

		protected override void OnInitialization()
		{
			base.OnInitialization();
			_fsm = new RaGOFiniteStateMachine(this, RaGOFiniteStateMachine.GetGOStates(transform));
		}

		protected override void OnSetData()
		{
			_runningDisplay.SetActive(false);
		}

		protected override void OnSetDataResolved()
		{
			Data.DataSetEvent += OnNewEncounterSetEvent;
			Data.DataClearedEvent += OnCurrentEncounterClearedEvent;

			Data.EnteredEncounterEvent += OnEnteredEncounterEvent;
			Data.ExitedEncounterEvent += OnExitedEncounterEvent;

			if(HasEncounter)
			{
				OnNewEncounterSetEvent(Data);
			}
			else
			{
				OnCurrentEncounterClearedEvent(Data);
			}

			if(Data.IsEncounterRunning)
			{
				OnEnteredEncounterEvent(Data);
			}
			else
			{
				OnExitedEncounterEvent(Data);
			}
		}

		protected override void OnClearData()
		{
			OnCurrentEncounterClearedEvent(Data);
			
			Data.EnteredEncounterEvent -= OnEnteredEncounterEvent;
			Data.ExitedEncounterEvent -= OnExitedEncounterEvent;

			Data.DataSetEvent -= OnNewEncounterSetEvent;
			Data.DataClearedEvent -= OnCurrentEncounterClearedEvent;
		}

		protected override void OnDeinitialization()
		{
			if(_fsm != null)
			{
				_fsm.Dispose();
				_fsm = null;
			}
			base.OnDeinitialization();
		}

		public void Editor_OnActionButtonPressed()
		{
			if(HasData && HasEncounter)
			{
				SlotActionPressedEvent?.Invoke(this);
			}
		}

		private void OnNewEncounterSetEvent(RaDataHolderBase<CampaignEncounter> holder)
		{
			_titleLabel.text = EncounterConfig.Title;
			_encounterIconImage.sprite = EncounterConfig.IconSprite;
			_descriptionLabel.text = EncounterConfig.Description;

			_encounterTypeIconImage.sprite = EncounterConfig.EncounterType.IconSprite;
			_encounterTypeActionLabel.text = EncounterConfig.EncounterType.ActionText;
			_encounterTypeActionLabel.color = EncounterConfig.EncounterType.ActionColor;

			_fsm.SwitchState(FILLED_SLOT_INDEX);
		}

		private void OnCurrentEncounterClearedEvent(RaDataHolderBase<CampaignEncounter> holder)
		{
			_titleLabel.text = null;
			_encounterIconImage.sprite = null;
			_descriptionLabel.text = null;

			_encounterTypeIconImage.sprite = null;
			_encounterTypeActionLabel.text = null;
			_encounterTypeActionLabel.color = default;

			_fsm.SwitchState(EMPTY_SLOT_INDEX);
		}

		private void OnEnteredEncounterEvent(EncounterSlotModel encounter)
		{
			_runningDisplay.SetActive(true);
		}

		private void OnExitedEncounterEvent(EncounterSlotModel encounter)
		{
			_runningDisplay.SetActive(false);
		}
	}
}