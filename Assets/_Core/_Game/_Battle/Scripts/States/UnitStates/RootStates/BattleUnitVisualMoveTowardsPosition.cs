using RaFSM;
using RaTweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle
{
	public class BattleUnitVisualMoveTowardsPosition : RaGOStateBase<BattleUnit>
	{
		[Header("Events")]
		public RaGOStateEvent StartedMoveVisualEvent;
		public RaGOStateEvent CompletedMoveVisualEvent;
		public RaGOStateEvent FailedMoveVisualEvent;

		[Header("Options")]
		[SerializeField]
		private float _speed = 1f;

		[Header("References")]
		[SerializeField]
		[FormerlySerializedAs("_gridModelSO")]
		private BattleGridReferenceSO _gridReferenceSO = null;

		protected BattleUnitVisuals UnitVisuals => Dependency.UnitVisuals;

		protected override void OnInit()
		{

		}

		protected override void OnEnter()
		{
			if(_gridReferenceSO.Grid.TryGetElement(_gridReferenceSO.Grid.ToGridPos(Dependency.transform.position), 
				out GameGridElement currentVisualPosElement))
			{
				Vector2Int currentPos = currentVisualPosElement.Position;
				Vector2Int destination = Dependency.Position;
				Vector2Int deltaNormalized = destination - currentPos;

				if(deltaNormalized.x != 0)
				{
					deltaNormalized.x = (int)Mathf.Sign(deltaNormalized.x);
				}

				if(deltaNormalized.y != 0)
				{
					deltaNormalized.y = (int)Mathf.Sign(deltaNormalized.y);
				}

				if(deltaNormalized.x != 0 || deltaNormalized.y != 0)
				{
					Vector2Int newPos = currentPos + deltaNormalized;

					if(_gridReferenceSO.Grid.TryGetUnitSpot(newPos, Dependency.Owner, out ElementBattleUnitSpot newSpot))
					{
						Dependency.transform.TweenMove(newSpot.GetUnitLocation(), deltaNormalized.magnitude / _speed)
							.OnStart(() =>
							{
								Dependency.UnitVisuals.SetOrientationToDirection(deltaNormalized.x);
								StartedMoveVisualEvent.Invoke(this);
							}).OnComplete(() =>
							{
								Dependency.UnitVisuals.SetOrientationToAlignment();
								CompletedMoveVisualEvent.Invoke(this);
							});

						// Early Return to mark as success
						return;
					}
				}
			}

			FailedMoveVisualEvent.Invoke(this);
		}

		protected override void OnExit(bool isSwitch)
		{

		}

		protected override void OnDeinit()
		{

		}
	}
}