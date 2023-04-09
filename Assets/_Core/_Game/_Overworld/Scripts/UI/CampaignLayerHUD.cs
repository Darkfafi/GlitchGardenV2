using RaDataHolder;
using System;
using UnityEngine;

namespace Game.Campaign
{
	public class CampaignLayerHUD : RaMonoDataHolderBase<CampaignLayerModel>
	{
		[field: SerializeField]
		public EncounterSlotDisplay LeftSlotDisplay
		{
			get; private set;
		}

		[field: SerializeField]
		public EncounterSlotDisplay CenterSlotDisplay
		{
			get; private set;
		}

		[field: SerializeField]
		public EncounterSlotDisplay RightSlotDisplay
		{
			get; private set;
		}

		protected override void OnSetData()
		{
			LeftSlotDisplay.SetData(Data.LeftSlot, false);
			CenterSlotDisplay.SetData(Data.CenterSlot, false);
			RightSlotDisplay.SetData(Data.RightSlot, false);
		}

		protected override void OnSetDataResolved()
		{
			LeftSlotDisplay.SlotActionPressedEvent += OnLeftSlotActionPressed;
			CenterSlotDisplay.SlotActionPressedEvent += OnCenterSlotActionPressed;
			RightSlotDisplay.SlotActionPressedEvent += OnRightSlotActionPressed;

			LeftSlotDisplay.Resolve();
			CenterSlotDisplay.Resolve();
			RightSlotDisplay.Resolve();
		}

		protected override void OnClearData()
		{
			RightSlotDisplay.ClearData();
			CenterSlotDisplay.ClearData();
			LeftSlotDisplay.ClearData();

			RightSlotDisplay.SlotActionPressedEvent -= OnRightSlotActionPressed;
			CenterSlotDisplay.SlotActionPressedEvent -= OnCenterSlotActionPressed;
			LeftSlotDisplay.SlotActionPressedEvent -= OnLeftSlotActionPressed;
		}

		public EncounterSlotDisplay GetEncounterSlotDisplay(CampaignLayerModel.SlotType slotType)
		{
			switch(slotType)
			{
				case CampaignLayerModel.SlotType.Left:
					return LeftSlotDisplay;
				case CampaignLayerModel.SlotType.Center:
					return CenterSlotDisplay;
				case CampaignLayerModel.SlotType.Right:
					return RightSlotDisplay;
				default:
					throw new NotImplementedException($"SlotType {slotType} has not been implemented");
			}
		}

		private void OnLeftSlotActionPressed(EncounterSlotDisplay display)
		{
			Data.TryEnter(CampaignLayerModel.SlotType.Left);
		}

		private void OnCenterSlotActionPressed(EncounterSlotDisplay display)
		{
			Data.TryEnter(CampaignLayerModel.SlotType.Center);
		}

		private void OnRightSlotActionPressed(EncounterSlotDisplay display)
		{
			Data.TryEnter(CampaignLayerModel.SlotType.Right);
		}
	}
}