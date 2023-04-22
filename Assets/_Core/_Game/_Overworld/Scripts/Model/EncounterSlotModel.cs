using RaDataHolder;
using RaFSM;
using System;

namespace Game.Campaign
{
	public class EncounterSlotModel : RaDataHolderBase<CampaignEncounter>
	{
		public delegate void EncounterHandler(EncounterSlotModel encounterSlot);
		public event EncounterHandler EnteredEncounterEvent;
		public event EncounterHandler ExitedEncounterEvent;

		public event EncounterHandler StartedEncounterEvent;
		public event EncounterHandler ProgressedEncounterEvent;
		public event EncounterHandler EndedEncounterEvent;

		public CampaignEncounter Encounter => Data;

		public bool IsEncounterRunning => Encounter != null ? Encounter.IsEncounterRunning : false;

		protected override void OnSetData()
		{
			Data.EnteredEncounterEvent += OnEnteredEncounterEvent;
			Data.ExitedEncounterEvent += OnExitedEncounterEvent;

			Data.StartedEncounterEvent += OnStartedEncounterEvent;
			Data.ProgressedEncounterEvent += OnProgressedEncounterEvent;
			Data.EndedEncounterEvent += OnEndedEncounterEvent;
		}

		protected override void OnClearData()
		{
			Data.EndedEncounterEvent -= OnEndedEncounterEvent;
			Data.ProgressedEncounterEvent -= OnProgressedEncounterEvent;
			Data.StartedEncounterEvent -= OnStartedEncounterEvent;

			Data.EnteredEncounterEvent -= OnEnteredEncounterEvent;
			Data.ExitedEncounterEvent -= OnExitedEncounterEvent;
		}

		public bool TryGetCurrentState<T>(out T state)
			where T : RaGOStateBase
		{
			if(IsEncounterRunning)
			{
				return Encounter.CurrentRunner.TryGetCurrentState(out state);
			}
			state = default;
			return false;
		}

		private void OnEnteredEncounterEvent(CampaignEncounter encounter)
		{
			EnteredEncounterEvent?.Invoke(this);
		}

		private void OnStartedEncounterEvent(CampaignEncounter encounter)
		{
			StartedEncounterEvent?.Invoke(this);
		}

		private void OnProgressedEncounterEvent(CampaignEncounter encounter)
		{
			ProgressedEncounterEvent?.Invoke(this);
		}

		private void OnEndedEncounterEvent(CampaignEncounter encounter)
		{
			EndedEncounterEvent?.Invoke(this);
		}

		private void OnExitedEncounterEvent(CampaignEncounter encounter)
		{
			ExitedEncounterEvent?.Invoke(this);
		}
	}
}