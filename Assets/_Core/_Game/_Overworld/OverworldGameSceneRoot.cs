using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Campaign
{
	public class OverworldGameSceneRoot : SceneRootBase
	{
		[field: SerializeField]
		public OverworldUIGroup OverworldUI
		{
			get; private set;
		}

		private OverworldGameModelSO _overworldGameModelSO = null;

		protected CampaignModel Campaign => _overworldGameModelSO != null ? _overworldGameModelSO.Campaign : null;

		protected override void OnSetup()
		{
			_overworldGameModelSO = Models.GetModelSO<OverworldGameModelSO>();

			OverworldUI.SetData(_overworldGameModelSO, false);
		}

		protected override void OnStart()
		{
			if(!Campaign.LayerModel.HasData)
			{
				Campaign.TrySetNextLayer();
			}

			OverworldUI.Resolve();
			OverworldUI.RegisterUser(this);

			Campaign.LayerModel.ForEachSlot((slot) => 
			{
				slot.EndedEncounterEvent += OnEndedEncounterEvent;
			});

			if(Campaign.LayerModel.TryGetCurrentEncounterSlot(out EncounterSlotModel encounterSlotModel))
			{
				// Reward Display
				if(encounterSlotModel.TryGetCurrentState(out EncounterRewardReadyState encounterReward))
				{
					encounterReward.ClaimReward();
				}
			}	
		}

		private void OnEndedEncounterEvent(EncounterSlotModel encounterSlot)
		{
			if(Campaign.LayerModel.TryGetCurrentEncounterSlot(out EncounterSlotModel currentEncounterSlot) && currentEncounterSlot == encounterSlot)
			{
				Campaign.LayerModel.TrySetNextEncounter();
			}
		}

		protected override void OnEnd()
		{
			Campaign.LayerModel.ForEachSlot(x =>
			{
				x.EndedEncounterEvent -= OnEndedEncounterEvent;
			});

			OverworldUI.UnregisterUser(this);

			OverworldUI.ClearData();
			_overworldGameModelSO = null;
		}
	}
}
