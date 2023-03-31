using UnityEngine;

namespace Game.Battle
{
	[CreateAssetMenu(menuName = "Behaviours/References/BattlePlayerSidesReferenceSO")]
	public class BattlePlayerSidesReferenceSO : BattleGameReferenceSOBase
	{
		public BattlePlayerSide HomePlayerSide
		{
			get; private set;
		}

		public BattlePlayerSide AwayPlayerSide
		{
			get; private set;
		}

		protected override void OnSetup()
		{
			HomePlayerSide = Game.HomePlayerSide;
			AwayPlayerSide = Game.AwayPlayerSide;
		}

		protected override void OnStart()
		{

		}

		protected override void OnEnd()
		{
			AwayPlayerSide = null;
			HomePlayerSide = null;
		}

		public BattlePlayer GetPlayer(BattlePlayer.Type playerType)
		{
			return GetPlayerSide(playerType).Player;
		}


		public BattlePlayerSide GetPlayerSide(BattlePlayer.Type playerType)
		{
			switch(playerType)
			{
				case BattlePlayer.Type.Home:
					return HomePlayerSide;
				case BattlePlayer.Type.Away:
					return AwayPlayerSide;
			}
			return default;
		}
	}
}