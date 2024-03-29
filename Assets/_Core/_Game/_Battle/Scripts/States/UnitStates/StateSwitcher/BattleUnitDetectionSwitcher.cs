﻿using RaFSM;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle
{
	public class BattleUnitDetectionSwitcher : StateSwitcherBase
	{
		[Header("References")]
		[SerializeField]
		[FormerlySerializedAs("_gridModelSO")]
		private BattleGridReferenceSO _gridReferenceSO = null;

		[SerializeField]
		private BattlePlayerDetectionType _detectionType = BattlePlayerDetectionType.Opposite;

		protected override bool CheckCondition(RaGOStateBase state)
		{
			BattleUnit unit = state.GetDependency<BattleUnit>();
			Vector2Int pos = unit.Position;
			pos += unit.Owner.GetOrientation(Vector2Int.right);

			bool condition = false;

			if(_gridReferenceSO.Grid.TryGetElement(pos, out GameGridElement targetElement))
			{
				if(_detectionType.HasFlag(BattlePlayerDetectionType.Same))
				{
					condition |= targetElement.TryGetUnit(unit.Owner.PlayerType, out _);
				}

				if(_detectionType.HasFlag(BattlePlayerDetectionType.Opposite))
				{
					condition |= targetElement.TryGetUnit(unit.Owner.GetOppositePlayerType(), out _);
				}
			}

			return condition;
		}
	}
}