using RaDataHolder;
using UnityEngine;

public class UnitVisuals : RaMonoDataHolderBase<Unit>
{
	[field: SerializeField]
	public HealthVisualizor HealthVisualizer = null;

	[field: SerializeField]
	public Transform OrientationTransform = null;

	[field: SerializeField]
	public Transform VisualsContainer = null;

	[field: SerializeField]
	public SpriteRenderer UnitRenderer = null;

	protected override void OnSetData()
	{
		UnitRenderer.sprite = Data.UnitData.Config.Icon;
		OrientationTransform.localScale = Data.Owner.GetOrientation(OrientationTransform.localScale);
		HealthVisualizer.SetData(Data.Health, false);
	}

	protected override void OnSetDataResolved()
	{
		HealthVisualizer.Resolve();
	}

	protected override void OnClearData()
	{
		HealthVisualizer.ClearData();
		UnitRenderer.sprite = null;
		OrientationTransform.localScale = Vector3.one;
	}
}
