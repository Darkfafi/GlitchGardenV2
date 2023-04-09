using RaDataHolder;

namespace Game.Campaign
{
	public class EncounterSlotModel : RaDataHolderBase<CampaignEncounter>
	{
		public CampaignEncounter Encounter => Data;

		protected override void OnSetData()
		{

		}

		protected override void OnClearData()
		{

		}
	}
}