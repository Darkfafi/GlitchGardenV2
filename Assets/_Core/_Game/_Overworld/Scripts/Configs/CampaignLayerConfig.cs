using System;
using System.Collections.Generic;
using UnityEngine;
using static RandomUtils;
using RaCollection;

namespace Game.Campaign
{
	public class CampaignLayerConfig : ScriptableObject
	{
		[field: SerializeField]
		public int PreferredEncounterAmount
		{
			get; private set;
		}

		[field: SerializeField]
		public LayerEncounterData[] LayerEncounters
		{
			get; private set;
		}

		public List<EncounterConfigBase> CreateEncounters()
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
				AddEncounter(pool.GetRandomEntry());
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

		[Serializable]
		public class LayerEncounterData : IHasWeight
		{
			[field: SerializeField]
			public int Weight
			{
				get; set;
			}
			public int MinAmount;
			public int MaxAmount;
			public EncounterConfigBase Encounter;
		}
	}
}