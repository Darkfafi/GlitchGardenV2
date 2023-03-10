using RaDataHolder;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
	public class DraggingUnitElement : RaMonoDataHolderBase<UnitConfig>
	{
		[SerializeField]
		private UnitsMechanicSO _unitsMechanic = null;

		[SerializeField]
		private PlayersModelSO _playersModelSO = null;

		[SerializeField]
		private SpriteRenderer _iconRenderer = null;

		[SerializeField]
		private TrailRenderer _trailRenderer = null;

		private List<GameGridElement> _gridElements = new List<GameGridElement>();
		private ElementUnitSpot _previewTarget = null;

		public bool TryGetPlacementTarget(out ElementUnitSpot target)
		{
			target = _previewTarget;
			return target != null;
		}

		protected override void OnSetData()
		{
			gameObject.SetActive(true);
			_iconRenderer.sprite = Data.Icon;
			SetTrailColor(Data.Color);
		}

		protected override void OnClearData()
		{
			gameObject.SetActive(false);
			_iconRenderer.sprite = null;
			SetTrailColor(Color.white);
			_gridElements.Clear();
			RefreshPreviewTarget();
		}

		public void Setup()
		{
			OnClearData();
		}

		public bool TryCreateDraggingUnit()
		{
			if(TryGetPlacementTarget(out ElementUnitSpot spot))
			{
				return _unitsMechanic.CreateUnit(new Unit.CoreData() { Owner = _playersModelSO.GetPlayer(Player.Type.Home), Config = Data }, spot.Element.Position).IsSuccess;
			}
			return false;
		}

		public void RefreshPreviewTarget()
		{
			float itemDistance = float.MaxValue;
			ElementUnitSpot closestItem = null;
			for(int i = 0; i < _gridElements.Count; i++)
			{
				var element = _gridElements[i];
				float currentDistance = Vector3.Distance(element.transform.position, transform.position);
				if(currentDistance < itemDistance)
				{
					var response = _unitsMechanic.CanCreateUnit(new Unit.CoreData { Owner = _playersModelSO.GetPlayer(Player.Type.Home), Config = Data }, element.Position);
					if(response.IsSuccess && response.Locator.TryGetValue(out ElementUnitSpot currentItem))
					{
						closestItem = currentItem;
						itemDistance = currentDistance;
					}
				}
			}

			if(_previewTarget != closestItem)
			{
				if(_previewTarget != null)
				{
					_previewTarget.SetPreview(null);
				}

				_previewTarget = closestItem;

				if(_previewTarget != null)
				{
					_previewTarget.SetPreview(Data);
				}
			}
		}

		protected void OnTriggerEnter2D(Collider2D collision)
		{
			if(HasData)
			{
				if(collision.gameObject.TryGetComponent(out GameGridElement element))
				{
					_gridElements.Add(element);
					RefreshPreviewTarget();
				}
			}
		}

		protected void OnTriggerExit2D(Collider2D collision)
		{
			if(HasData)
			{
				if(collision.gameObject.TryGetComponent(out GameGridElement element))
				{
					_gridElements.Remove(element);
					RefreshPreviewTarget();
				}
			}
		}

		private void SetTrailColor(Color color)
		{
			_trailRenderer.startColor = color;
			color.a = _trailRenderer.endColor.a;
			_trailRenderer.endColor = color;
		}
	}
}