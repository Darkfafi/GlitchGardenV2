using System.Collections.Generic;

namespace Game.Battle
{
	[System.Flags]
	public enum BattlePlayerDetectionType
	{
		Same = 1,
		Opposite = 2
	}

	public static class BattlePlayerDetectionUtils
	{
		public static List<BattlePlayer.Type> DetectPlayerTypes(this BattlePlayer self, BattlePlayerDetectionType detectionType)
		{
			List<BattlePlayer.Type> list = new List<BattlePlayer.Type>(2);
			if(detectionType.HasFlag(BattlePlayerDetectionType.Same))
			{
				list.Add(self.PlayerType);
			}

			if(detectionType.HasFlag(BattlePlayerDetectionType.Opposite))
			{
				list.Add(self.GetOppositePlayerType());
			}
			return list;
		}
	}
}