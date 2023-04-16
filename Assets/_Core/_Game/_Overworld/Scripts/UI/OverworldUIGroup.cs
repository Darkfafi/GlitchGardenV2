using UI;
using UnityEngine;

namespace Game.Campaign
{
	public class OverworldUIGroup : GenericUIGroup<OverworldGameModelSO>
	{
		[field: SerializeField]
		public CampaignLayerHUD CampaignLayerHUD
		{
			get; private set;
		}

		[field: SerializeField]
		public HealthIconsDisplay PlayerHealthHUD
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			CampaignLayerHUD.SetData(Data.Campaign.LayerModel, false);
			PlayerHealthHUD.SetData(Data.Player.Health);
		}

		protected override void OnResolveData()
		{
			CampaignLayerHUD.Resolve();
		}

		protected override void OnClearData()
		{
			CampaignLayerHUD.ClearData();
		}
	}
}