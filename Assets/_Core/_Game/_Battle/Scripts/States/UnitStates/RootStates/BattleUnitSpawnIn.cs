using RaFSM;
using RaTweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle
{

	public class BattleUnitSpawnIn : RaGOStateBase<BattleUnit>
	{
		[Header("Events")]
		public RaGOStateEvent OnSpawnEndedEvent;

		[Header("Options")]
		[SerializeField]
		private bool _spawnInFirstColumnOfPosition = false;

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
			Vector2Int positionToSpawnAt = Dependency.Position;

			if(_spawnInFirstColumnOfPosition)
			{
				positionToSpawnAt.x = _gridReferenceSO.Grid.GetPlayerColumn(Dependency.Owner.PlayerType);
			}

			if(_gridReferenceSO.Grid.TryGetUnitSpot(positionToSpawnAt, Dependency.Owner, out ElementBattleUnitSpot unitSpot))
			{
				UnitVisuals.VisualsContainer.gameObject.SetActive(false);
				Dependency.transform.TweenScale(0f, 0.5f)
					.SetEasing(RaEasingType.OutBack).From()
					.OnStart(() =>
					{
						Dependency.transform.position = unitSpot.GetUnitLocation();
						UnitVisuals.VisualsContainer.gameObject.SetActive(true);
					})
					.OnComplete(() =>
					{
						OnSpawnEndedEvent.Invoke(this);
					});
			}
		}

		protected override void OnExit(bool isSwitch)
		{

		}

		protected override void OnDeinit()
		{

		}
	}
}