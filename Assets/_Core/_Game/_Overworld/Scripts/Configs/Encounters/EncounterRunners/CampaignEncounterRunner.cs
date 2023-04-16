using RaDataHolder;
using RaFSM;
using UnityEngine;

namespace Game.Campaign
{
	public class CampaignEncounterRunner : RaMonoDataHolderBase<CampaignEncounter>, IRaFSMCycler
	{
		public delegate void EncounterHandler(CampaignEncounterRunner runner);
		public event EncounterHandler EncounterStartedEvent;
		public event EncounterHandler EncounterProgressedEvent;
		public event EncounterHandler EncounterEndedEvent;

		[field: SerializeField]
		public EncounterTypeConfig EncounterType
		{
			get; private set;
		}

		private RaGOFiniteStateMachine _fsm;

		public CampaignEncounter Encounter => Data;

		public bool IsRunning => _fsm != null && _fsm.IsRunning;

		protected override void OnSetData()
		{
			_fsm = new RaGOFiniteStateMachine(this, RaGOFiniteStateMachine.GetGOStates(transform));
		}

		protected override void OnSetDataResolved()
		{
			base.OnSetDataResolved();
			_fsm.SwitchState(0);
			EncounterStartedEvent?.Invoke(this);
		}

		protected override void OnClearData()
		{
			if(_fsm != null)
			{
				_fsm.Dispose();
				_fsm = null;
			}
		}

		public void GoToNextState()
		{
			if(IsRunning)
			{
				int index = _fsm.GoToNextState(false);
				EncounterProgressedEvent?.Invoke(this);
				if(index == RaGOFiniteStateMachine.NO_STATE_INDEX)
				{
					EncounterEndedEvent?.Invoke(this);
				}
			}
		}
	}
}