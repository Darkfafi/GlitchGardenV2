using RaModelsSO;
using UnityEngine;

namespace Game.Campaign
{
	public class OverworldGameModelSO : RaModelSOBase
	{
		[SerializeField]
		private CampaignConfig _defaultCampaignConfig = null;

		[SerializeField]
		private PlayerConfig _defaultPlayerConfig = null;

		public CampaignModel Campaign
		{
			get; private set;
		}

		public PlayerModel Player
		{
			get; private set;
		}

		protected override void OnInit()
		{
			Campaign = new CampaignModel(_defaultCampaignConfig);
			Player = new PlayerModel(_defaultPlayerConfig.Data);
		}

		protected override void OnDeinit()
		{
			Player = default;
			Campaign = default;
		}
	}
}