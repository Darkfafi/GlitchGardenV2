namespace Game.Campaign
{
	public class CampaignModel
	{
		public EncounterConfigCollection Config
		{
			get; private set;
		}

		public CampaignModel(EncounterConfigCollection config)
		{
			Config = config;
		}
	}
}