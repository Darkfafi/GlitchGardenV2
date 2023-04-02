namespace Game.Campaign
{
	public class CampaignModel
	{
		public CampaignConfig Config
		{
			get; private set;
		}

		public CampaignModel(CampaignConfig config)
		{
			Config = config;
		}
	}
}