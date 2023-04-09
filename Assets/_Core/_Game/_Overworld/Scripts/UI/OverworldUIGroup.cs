using UI;
using UnityEngine;

namespace Game.Campaign
{
	public class OverworldUIGroup : GenericUIGroup<CampaignModel>
	{
		[field: SerializeField]
		public CampaignLayerHUD CampaignLayerHUD
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			CampaignLayerHUD.SetData(Data.LayerModel, false);
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