using RaFSM;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Battle
{
	public class SetTargetBattlePlayerState : RaGOStateBase<BattleUnit, TargetBattlePlayerData>
	{
		public RaGOStateEvent ChangedTargetEvent;
		public RaGOStateEvent NoChangeInTargetEvent;

		[SerializeField]
		private int _range = 1;

		[SerializeField]
		private BattlePlayerDetectionType _detectionType = BattlePlayerDetectionType.Opposite;

		[Header("References")]
		[SerializeField]
		private BattlePlayerSidesReferenceSO _playerSideReferenceSO = null;
		[SerializeField]
		private BattleGridReferenceSO _gridReferenceSO = null;

		protected override void OnInit()
		{

		}

		protected override void OnEnter()
		{
			RefreshTarget();
		}

		protected override void OnExit(bool isSwitch)
		{

		}

		protected override void OnDeinit()
		{

		}

		private BattlePlayer RefreshTarget()
		{
			BattlePlayer playerToSet = null;

			int unitColumn = DependencyA.Position.x;

			List<BattlePlayer.Type> targetPlayerTypes = DependencyA.Owner.DetectPlayerTypes(_detectionType);

			for(int i = 0; i < targetPlayerTypes.Count; i++)
			{
				BattlePlayer.Type playerType = targetPlayerTypes[i];

				int playerColumn = _gridReferenceSO.Grid.GetPlayerColumn(playerType);

				if(Mathf.Abs(playerColumn - unitColumn) < _range)
				{
					playerToSet = _playerSideReferenceSO.GetPlayer(playerType);
					break;
				}
			}

			if(DependencyB.SetTarget(playerToSet))
			{
				ChangedTargetEvent.Invoke(this);
			}
			else
			{
				NoChangeInTargetEvent.Invoke(this);
			}

			return playerToSet;
		}
	}
}