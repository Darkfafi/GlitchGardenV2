namespace Game.Campaign
{
	public class CampaignEncounter
	{
		public EncounterConfigBase Config
		{
			get; private set;
		}

		public CampaignEncounter(EncounterConfigBase config)
		{
			Config = config;
		}

		public void Enter()
		{
			Config.Enter(this);
		}
	}
}