using NestedSO;

namespace Game.Campaign
{
	/*
	 * Campaign
	 * > Layers
	 * >> Entries
	 * Example
	 * Campaign
	 * > Layers
	 * > - Entrance Layer
	 * >> - Guards: Weight: 5, Min: 1, Max: 10, Encounter: Guard Encounter
	 * >> - Rats: Weight: 10, Min: 0, Max: 0, Encounter 
	 */
	public class CampaignConfig : NestedSOCollectionBase<CampaignLayerConfig>
	{
	
	}
}