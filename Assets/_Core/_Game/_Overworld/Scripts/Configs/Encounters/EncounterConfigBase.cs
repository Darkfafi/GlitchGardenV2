using UnityEngine;

namespace Game.Campaign
{
	public abstract class EncounterConfigBase : ScriptableObject
	{
		[field: SerializeField]
		public string EncounterName
		{
			get; private set;
		}

		[field: SerializeField]
		public Sprite DisplaySprite
		{
			get; private set;
		}

		public abstract void Enter(CampaignEncounter encounter);
	}
}