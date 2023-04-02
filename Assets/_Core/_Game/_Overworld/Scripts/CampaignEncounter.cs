namespace Game.Campaign
{
	public abstract class CampaignEncounter
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