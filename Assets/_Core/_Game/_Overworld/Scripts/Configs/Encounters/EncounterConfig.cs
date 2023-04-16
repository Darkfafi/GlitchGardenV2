using UnityEngine;

namespace Game.Campaign
{
	public class EncounterConfig : ScriptableObject
	{
		[field: SerializeField]
		public string Title
		{
			get; private set;
		}

		[field: SerializeField]
		public Sprite IconSprite
		{
			get; private set;
		}

		[field: SerializeField, TextArea]
		public string Description
		{
			get; private set;
		}

		[field: SerializeField]
		public CampaignEncounterRunner RunnerPrefab
		{
			get; private set;
		}

		public EncounterTypeConfig EncounterType => RunnerPrefab.EncounterType;
	}
}