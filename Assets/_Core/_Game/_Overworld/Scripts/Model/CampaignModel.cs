using RaCollection;
using System.Linq;

namespace Game.Campaign
{
	public class CampaignModel
	{
		public CampaignConfig Config
		{
			get; private set;
		}

		private RaElementCollection<CampaignLayerConfig> _nextLayerConfigs;
		public CampaignLayerModel LayerModel
		{
			get; private set;
		}

		public IReadOnlyRaElementCollection<CampaignLayerConfig> NextLayerConfigs => _nextLayerConfigs;

		public CampaignModel(CampaignConfig config)
		{
			Config = config;
			LayerModel = new CampaignLayerModel();
			_nextLayerConfigs = new RaElementCollection<CampaignLayerConfig>(Config.GetItems().ToArray());
		}

		public bool TrySetNextLayer()
		{
			if(_nextLayerConfigs.TryDequeue(out CampaignLayerConfig newLayer))
			{
				LayerModel.ReplaceData(newLayer, false);
				return true;
			}
			else
			{
				LayerModel.ClearData();
				return false;
			}
		}
	}
}