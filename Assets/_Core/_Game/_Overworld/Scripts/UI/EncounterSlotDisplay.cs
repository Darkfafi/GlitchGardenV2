using RaDataHolder;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Campaign
{
	public class EncounterSlotDisplay : RaMonoDataHolderBase<EncounterSlotModel>
	{
		[Header("Encounter")]
		[SerializeField]
		private TMP_Text _titleLabel = null;

		[SerializeField]
		private TMP_Text _descriptionLabel = null;

		[SerializeField]
		private Image _encounterIconImage = null;

		[Header("Encounter - Type")]
		[SerializeField]
		private Image _encounterTypeIconImage = null;
		[SerializeField]
		private TMP_Text _encounterTypeActionLabel = null;

		public EncounterConfigBase EncounterConfig => HasEncounter ? Data.Encounter.Config : null;
		public bool HasEncounter => Data.Encounter != null;

		protected override void OnSetData()
		{
			
		}

		protected override void OnSetDataResolved()
		{
			Data.DataSetEvent += OnNewEncounterSetEvent;
			Data.DataClearedEvent += OnCurrentEncounterClearedEvent;
			if(HasEncounter)
			{
				OnNewEncounterSetEvent(Data);
			}
			else
			{
				OnCurrentEncounterClearedEvent(Data);
			}
		}

		private void OnNewEncounterSetEvent(RaDataHolderBase<CampaignEncounter> holder)
		{
			_titleLabel.text = EncounterConfig.Title;
			_encounterIconImage.sprite = EncounterConfig.IconSprite;
			_descriptionLabel.text = EncounterConfig.Description;

			_encounterTypeIconImage.sprite = EncounterConfig.ActionType.IconSprite;
			_encounterTypeActionLabel.text = EncounterConfig.ActionType.ActionText;
			_encounterTypeActionLabel.color = EncounterConfig.ActionType.ActionColor;
		}

		private void OnCurrentEncounterClearedEvent(RaDataHolderBase<CampaignEncounter> holder)
		{
			_titleLabel.text = null;
			_encounterIconImage.sprite = null;
			_descriptionLabel.text = null;

			_encounterTypeIconImage.sprite = null;
			_encounterTypeActionLabel.text = null;
			_encounterTypeActionLabel.color = default;
		}

		protected override void OnClearData()
		{
			OnCurrentEncounterClearedEvent(Data);
			Data.DataSetEvent -= OnNewEncounterSetEvent;
			Data.DataClearedEvent -= OnCurrentEncounterClearedEvent;
		}
	}
}