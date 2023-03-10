using RaDataHolder;
using UnityEngine;

public class UnitVisuals : RaMonoDataHolderBase<Unit>
{
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
	}

	protected override void OnClearData()
	{
		UnitRenderer.sprite = null;
		OrientationTransform.localScale = Vector3.one;
	}
}