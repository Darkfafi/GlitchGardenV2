using UnityEngine;

namespace Game.Campaign
{
	public abstract class EncounterConfigBase : ScriptableObject
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
		public EncounterActionTypeConfig ActionType
		{
			get; private set;
		}

		public abstract void Enter(CampaignEncounter encounter);
	}
}