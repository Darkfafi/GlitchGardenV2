using UnityEngine;

namespace Game.Campaign
{
	public class EncounterTypeConfig : ScriptableObject
	{
		[field: SerializeField]
		public Sprite IconSprite
		{
			get; private set;
		}

		[field: SerializeField]
		public Color ActionColor
		{
			get; private set;
		} = Color.white;

		[field: SerializeField]
		public string ActionText
		{
			get; private set;
		} = "Enter";
	}
}