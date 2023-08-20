using RaFSM;
using UnityEngine;

namespace Game.Battle
{
	public class UnitPlacesObjectState : RaGOStateBase<BattleUnit>
	{
		[Header("Events")]
		public RaGOStateEvent PlacedEvent;
		public RaGOStateEvent FailedPlacementEvent;

		[Header("Options")]
		[SerializeField]
		private GameObject _objectToPlace = null;

		[Header("Settings")]
		[SerializeField]
		private bool _placeOnEnter = true;

		[Header("Requirements")]
		[SerializeField]
		private BattleGridReferenceSO _gridReferenceSO = null;

		protected override void OnInit()
		{

		}

		protected override void OnEnter()
		{
			if(_placeOnEnter)
			{
				Place();
			}
		}

		protected override void OnExit(bool isSwitch)
		{

		}

		protected override void OnDeinit()
		{

		}

		public void Place()
		{
			if(_gridReferenceSO.Grid.TryGetUnitSpot(Dependency, out var spot))
			{
				GameObject objectPlaced = Instantiate(_objectToPlace);
				objectPlaced.transform.position = spot.GetUnitLocation();
				PlacedEvent.Invoke(this);
			}
			else
			{
				FailedPlacementEvent.Invoke(this);
			}
		}
	}
}