using RaCollection;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Campaign
{
	public class CampaignLayerConfig : ConfigBase<CampaignLayerConfig.CoreData>
	{
		[Serializable]
		public class LayerEncounterData : IRaCollectionWeightedItem
		{
			[field: SerializeField]
			public int RaItemWeight
			{
				get; set;
			}
			public int MinAmount;
			public int MaxAmount;
			public EncounterConfigBase Encounter;
		}

		[Serializable]
		public struct CoreData
		{
			public int PreferredEncounterAmount;
			public LayerEncounterData[] LayerEncounters;

			public List<EncounterConfigBase> CreateEncountersConfigList()
			{
				List<EncounterConfigBase> encountersCreated = new List<EncounterConfigBase>();

				List<LayerEncounterData> pool = new List<LayerEncounterData>();
				Dictionary<LayerEncounterData, int> creationData = new Dictionary<LayerEncounterData, int>();

				LayerEncounters.ForEach((x) =>
				{
					pool.Add(x);
					creationData[x] = 0;

					if(x.MinAmount > 0)
					{
						for(int i = 0; i < x.MinAmount; i++)
						{
							AddEncounter(x);
						}
					}
				});

				while(pool.Count > 0 && encountersCreated.Count < PreferredEncounterAmount)
				{
					AddEncounter(pool.GetRandomItem());
				}

				return encountersCreated;

				void AddEncounter(LayerEncounterData layer)
				{
					if(creationData.TryGetValue(layer, out int amount))
					{
						amount++;
						creationData[layer] = amount;
						if(layer.MaxAmount > 0 && amount >= layer.MaxAmount)
						{
							pool.Remove(layer);
						}
					}

					encountersCreated.Add(layer.Encounter);
				}
			}
		}
	}
}