using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Campaign
{
	public class OverworldGameSceneRoot : SceneRootBase
	{
		[field: SerializeField]
		public OverworldUIGroup OverworldUI
		{
			get; private set;
		}

		private OverworldGameModelSO _overworldGameModelSO = null;

		protected CampaignModel Campaign => _overworldGameModelSO != null ? _overworldGameModelSO.Campaign : null;

		protected override void OnSetup()
		{
			_overworldGameModelSO = Models.GetModelSO<OverworldGameModelSO>();

			OverworldUI.SetData(Campaign, false);
		}

		protected override void OnStart()
		{
			if(!Campaign.LayerModel.HasData)
			{
				Campaign.TrySetNextLayer();
			}

			OverworldUI.Resolve();
			OverworldUI.RegisterUser(this);
		}

		protected override void OnEnd()
		{
			OverworldUI.UnregisterUser(this);

			OverworldUI.ClearData();
			_overworldGameModelSO = null;
		}
	}
}
