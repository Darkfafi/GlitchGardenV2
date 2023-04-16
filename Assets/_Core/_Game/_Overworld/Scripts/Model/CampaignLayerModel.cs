using RaCollection;
using RaDataHolder;
using System;

namespace Game.Campaign
{
	public class CampaignLayerModel : RaDataHolderBase<CampaignLayerConfig>
	{
		public readonly int SlotCount = 3;

		private RaElementCollection<CampaignEncounter> _layerEncounters;
		private RaCollection<CampaignEncounter> _encountersPool = new RaCollection<CampaignEncounter>();

		public CampaignLayerConfig Config => Data;

		public IReadOnlyRaElementCollection<CampaignEncounter> LayerEncounters => _layerEncounters;
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

		public SlotType? CurrentSlotType
		{
			get; private set;
		}

		public CampaignLayerModel()
		{
			_layerEncounters = new RaElementCollection<CampaignEncounter>(onRemoveItem: (encounter, index) => encounter.Dispose());

			LeftSlot = new EncounterSlotModel();
			CenterSlot = new EncounterSlotModel();
			RightSlot = new EncounterSlotModel();
			CurrentSlotType = null;
		}

		protected override void OnSetData()
		{
			Config.Data.CreateEncountersConfigList().ForEach(encounterConfig =>
			{
				CampaignEncounter encounter = new CampaignEncounter(encounterConfig);
				_layerEncounters.Add(encounter);
				_encountersPool.Add(encounter);
			});
			_encountersPool.Shuffle();
			ForEachSlotType(slotType => TrySetNextEncounter(slotType));
		}

		protected override void OnClearData()
		{
			_encountersPool.Clear();
			_layerEncounters.Clear();
			LeftSlot = default;
			CenterSlot = default;
			RightSlot = default;
		}

		public void ClearCurrentSlotType()
		{
			CurrentSlotType = null;
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
			if(!CurrentSlotType.HasValue)
			{
				EncounterSlotModel slot = GetEncounterSlot(slotType);
				if(slot.HasData)
				{
					CurrentSlotType = slotType;
					slot.Encounter.Enter();
					return true;
				}
			}
			return false;
		}

		private bool TrySetNextEncounter()
		{
			if(CurrentSlotType.HasValue)
			{
				SlotType slotType = CurrentSlotType.Value;
				EncounterSlotModel slot = GetEncounterSlot(slotType);
				if(slot.HasData)
				{
					slot.Encounter.Exit();
					ClearCurrentSlotType();
					TrySetNextEncounter(slotType);
				}
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
					throw new NotImplementedException($"SlotType {slotType} has not been implemented");
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