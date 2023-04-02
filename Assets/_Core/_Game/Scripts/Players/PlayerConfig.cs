using UnityEngine;

namespace Game
{
	public class PlayerConfig : ScriptableObject
	{
		[field: SerializeField]
		public int HP
		{
			get; private set;
		}

		[field: SerializeField]
		public UnitConfig[] Units
		{
			get; private set;
		}
	}
}