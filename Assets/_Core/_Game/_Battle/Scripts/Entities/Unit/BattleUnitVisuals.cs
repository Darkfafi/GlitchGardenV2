using RaDataHolder;
using UnityEngine;

namespace Game.Battle
{
	public class BattleUnitVisuals : RaMonoDataHolderBase<BattleUnit>
	{
		[field: SerializeField]
		public HealthInteractionView HealthVisualizer = null;

		[field: SerializeField]
		public Transform OrientationTransform = null;

		[field: SerializeField]
		public Transform VisualsContainer = null;

		[field: SerializeField]
		public SpriteRenderer UnitRenderer = null;

		protected override void OnSetData()
		{
			UnitRenderer.sprite = Data.UnitData.Config.Icon;
			SetOrientationToAlignment();
			HealthVisualizer.SetData(Data.Health, false);
		}

		protected override void OnSetDataResolved()
		{
			HealthVisualizer.Resolve();
		}

		public void SetOrientationToDirection(int direction)
		{
			Vector3 scale = OrientationTransform.localScale;
			scale.x = Mathf.Abs(scale.x) * Mathf.Sign(direction);
			OrientationTransform.localScale = scale;
		}

		public void SetOrientationToAlignment()
		{
			OrientationTransform.localScale = Data.Owner.GetOrientation(OrientationTransform.localScale);
		}

		protected override void OnClearData()
		{
			HealthVisualizer.ClearData();
			UnitRenderer.sprite = null;
			OrientationTransform.localScale = Vector3.one;
		}
	}
}