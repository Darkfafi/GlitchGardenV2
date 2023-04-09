using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Campaign
{
	public class OverworldGameSceneRoot : SceneRootBase
	{
		private OverworldGameModelSO _overworldGameModelSO = null;

		protected CampaignModel Campaign => _overworldGameModelSO != null ? _overworldGameModelSO.Campaign : null;

		protected override void OnSetup()
		{
			_overworldGameModelSO = Models.GetModelSO<OverworldGameModelSO>();
		}

		protected override void OnStart()
		{
			if(!Campaign.LayerModel.HasData)
			{
				Campaign.TrySetNextLayer();
			}
		}

		protected override void OnEnd()
		{
			_overworldGameModelSO = null;
		}
	}
}
