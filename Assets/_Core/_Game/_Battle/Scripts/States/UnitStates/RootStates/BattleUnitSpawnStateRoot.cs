using RaFSM;
using RaTweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle
{
	public class BattleUnitSpawnStateRoot : RaGOStateBase<BattleUnit>
	{
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
			if(_gridReferenceSO.Grid.TryGetUnitSpot(Dependency, out ElementBattleUnitSpot unitSpot))
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
						Dependency.SetState(BattleUnit.State.Behaviour);
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