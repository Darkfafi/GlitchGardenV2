using RaDataHolder;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle.UI
{
	public class BattleUnitsHUD : RaMonoDataHolderBase<BattlePlayer>
	{
		[Header("References")]
		[SerializeField]
		[FormerlySerializedAs("_gridModelSO")]
		private BattleGridReferenceSO _gridReferenceSO = null;

		[Header("Requirements")]
		[SerializeField]
		private BattleUnitHUDEntryUIElement _entryPrefab = null;
		[SerializeField]
		private RectTransform _optionsContainer = null;

		[Header("External")]
		[SerializeField]
		private DraggingBattleUnitElement _draggingUnitElement;

		private Dictionary<UnitConfig, BattleUnitHUDEntryUIElement> _unitHUDEntryMap = new Dictionary<UnitConfig, BattleUnitHUDEntryUIElement>();

		private BattleUnitHUDEntryUIElement _currentDraggingHUDEntry = null;

		public bool IsInteractable
		{
			get; private set;
		}

		public void SetInteractable(bool isInteractable)
		{
			IsInteractable = isInteractable && HasData;

			if(!isInteractable)
			{
				ReleaseDraggingUnit(false);
			}
		}

		protected override void OnSetData()
		{
			_draggingUnitElement.Setup();

			for(int i = 0; i < Data.Model.Units.Length; i++)
			{
				var unitConfig = Data.Model.Units[i];
				CreateEntry(unitConfig);
			}
		}

		protected override void OnClearData()
		{
			UnitConfig[] keys = _unitHUDEntryMap.Keys.ToArray();
			for(int i = keys.Length - 1; i >= 0; i--)
			{
				RemoveEntry(keys[i]);
			}

			if(_gridReferenceSO.Grid != null)
			{
				_gridReferenceSO.Grid.DirtyEvent -= OnGridDirtyEvent;
			}

			SetInteractable(false);
		}

		private void CreateEntry(UnitConfig item)
		{
			if(_unitHUDEntryMap.ContainsKey(item))
			{
				return;
			}

			BattleUnitHUDEntryUIElement itemView = Instantiate(_entryPrefab, _optionsContainer);

			_unitHUDEntryMap[item] = itemView;

			var resolver = itemView.SetData(item, false);
			{
				itemView.DragStartedEvent += OnDragStartedEvent;
				itemView.DraggingEvent += OnDraggingEvent;
				itemView.DragEndedEvent += OnDragEndedEvent;
			}
			resolver.Resolve();
		}

		private void RemoveEntry(UnitConfig item)
		{
			if(_unitHUDEntryMap.TryGetValue(item, out BattleUnitHUDEntryUIElement itemView))
			{
				itemView.DragStartedEvent -= OnDragStartedEvent;
				itemView.DraggingEvent -= OnDraggingEvent;
				itemView.DragEndedEvent -= OnDragEndedEvent;
				itemView.ClearData();

				Destroy(itemView.gameObject);
				_unitHUDEntryMap.Remove(item);
			}
		}

		private void OnDragStartedEvent(BattleUnitHUDEntryUIElement view)
		{
			if(_currentDraggingHUDEntry != null)
			{
				return;
			}

			if(IsInteractable)
			{
				_currentDraggingHUDEntry = view;
				_gridReferenceSO.Grid.DirtyEvent += OnGridDirtyEvent;
				view.SetGrabbed(true);
				_draggingUnitElement.SetData(new DraggingBattleUnitElement.CoreData { Player = Data, UnitConfig = view.Config }, true);
				_gridReferenceSO.Grid.ShowUnitBuildabilityGrid(view.Config);
			}
		}

		private void OnDraggingEvent(BattleUnitHUDEntryUIElement view)
		{
			if(view != _currentDraggingHUDEntry)
			{
				return;
			}

			if(IsInteractable)
			{
				Vector3 newPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				newPos.z = _draggingUnitElement.transform.position.z;
				_draggingUnitElement.transform.position = newPos;
				_draggingUnitElement.RefreshPreviewTarget();
			}
		}

		private void OnDragEndedEvent(BattleUnitHUDEntryUIElement view)
		{
			if(view != _currentDraggingHUDEntry)
			{
				return;
			}

			if(IsInteractable)
			{
				ReleaseDraggingUnit(true);
			}
		}

		private void ReleaseDraggingUnit(bool placeUnit)
		{
			if(_currentDraggingHUDEntry != null)
			{
				_gridReferenceSO.Grid.DirtyEvent -= OnGridDirtyEvent;
				_gridReferenceSO.Grid.ClearUnitBuildabilityGrid();

				if(placeUnit)
				{
					_draggingUnitElement.TryCreateDraggingUnit();
				}

				_draggingUnitElement.ClearData();

				_currentDraggingHUDEntry.SetGrabbed(false);
				_currentDraggingHUDEntry = null;
			}
		}

		private void OnGridDirtyEvent()
		{
			if(HasData)
			{
				if(_draggingUnitElement != null)
				{
					_draggingUnitElement.RefreshPreviewTarget();
				}
			}
		}
	}
}