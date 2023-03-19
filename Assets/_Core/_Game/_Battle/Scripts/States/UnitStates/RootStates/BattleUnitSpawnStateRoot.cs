using RaFSM;
using RaTweening;
using UnityEngine;

namespace Game.Battle
{
	public class BattleUnitSpawnStateRoot : RaGOStateBase<BattleUnit>
	{
		[SerializeField]
		private BattleGridModelSO _gridModelSO = null;

		protected BattleUnitVisuals UnitVisuals => Dependency.UnitVisuals;

		protected override void OnInit()
		{

		}

		protected override void OnEnter()
		{
			if(_gridModelSO.Grid.TryGetUnitSpot(Dependency, out ElementBattleUnitSpot unitSpot))
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