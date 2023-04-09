using RaCollection;
using RaDataHolder;
using System;

namespace Game.Campaign
{
	public class CampaignLayerModel : RaDataHolderBase<CampaignLayerConfig>
	{
		public readonly int SlotCount = 3;

		private RaCollection<CampaignEncounter> _encountersPool = new RaCollection<CampaignEncounter>();

		public CampaignLayerConfig Config => Data;
		public IReadOnlyRaCollection<CampaignEncounter> EncountersPool => _encountersPool;

		public EncounterSlotModel LeftSlot
		{
			get; private set;
		}

		public EncounterSlotModel CenterSlot
		{
			get; private set;
		}

		public EncounterSlotModel RightSlot
		{
			get; private set;
		}

		public CampaignLayerModel()
		{
			LeftSlot = new EncounterSlotModel();
			CenterSlot = new EncounterSlotModel();
			RightSlot = new EncounterSlotModel();
		}

		protected override void OnSetData()
		{
			Config.Data.CreateEncountersConfigList().ForEach(encounterConfig =>
			{
				CampaignEncounter encounter = new CampaignEncounter(encounterConfig);
				_encountersPool.Add(encounter);
			});
			_encountersPool.Shuffle();
			ForEachSlotType(slotType => TrySetNextEncounter(slotType));
		}

		protected override void OnClearData()
		{
			_encountersPool.Clear();
			LeftSlot = default;
			CenterSlot = default;
			RightSlot = default;
		}

		public bool TrySetNextEncounter(SlotType slotType)
		{
			if(_encountersPool.TryDequeue(out CampaignEncounter newEncounter))
			{
				GetEncounterSlot(slotType).ReplaceData(newEncounter);
				return true;
			}
			return false;
		}

		public bool TryEnter(SlotType slotType)
		{
			EncounterSlotModel slot = GetEncounterSlot(slotType);
			if(slot.HasData)
			{
				slot.Encounter.Enter();
				return true;
			}
			return false;
		}

		public EncounterSlotModel GetEncounterSlot(SlotType slotType)
		{
			switch(slotType)
			{
				case SlotType.Left:
					return LeftSlot;

				case SlotType.Center:
					return CenterSlot;

				case SlotType.Right:
					return RightSlot;
				default:
					throw new System.NotImplementedException($"SlotType {slotType} has not been implemented");
			}
		}

		public EncounterSlotModel GetEncounterSlot(int index)
		{
			return GetEncounterSlot((SlotType)index);
		}

		public void ForEachSlotType(Action<SlotType> action)
		{
			for(int i = 0; i < SlotCount; i++)
			{
				SlotType slotType = (SlotType)i;
;				action?.Invoke(slotType);
			}
		}

		public void ForEachSlot(Action<EncounterSlotModel> action)
		{
			for(int i = 0; i < SlotCount; i++)
			{
				EncounterSlotModel slot = GetEncounterSlot(i);
				action?.Invoke(slot);
			}
		}


		public enum SlotType
		{
			Left = 0,
			Center = 1,
			Right = 2
		}
	}
}