using System;
using UnityEngine;
using RaCollection;

namespace Game.Campaign
{
	public class CampaignEncounter : IDisposable, IRaCollectionElement
	{
		public string Id
		{
			get; private set;
		}

		public EncounterConfig Config
		{
			get; private set;
		}

		public CampaignEncounterRunner CurrentRunner
		{
			get; private set;
		}

		public CampaignEncounter(EncounterConfig config)
		{
			Id = Guid.NewGuid().ToString();
			Config = config;
		}

		public void Enter()
		{
			Exit();

			CurrentRunner = GameObject.Instantiate(Config.RunnerPrefab);
			GameObject.DontDestroyOnLoad(CurrentRunner);
			CurrentRunner.name = $"Encounter ({Id})";
			CurrentRunner.SetData(this);
		}

		public void Exit()
		{
			if(CurrentRunner != null)
			{
				CurrentRunner.ClearData();
				GameObject.Destroy(CurrentRunner.gameObject);
				CurrentRunner = null;
			}
		}

		public void Dispose()
		{
			if(CurrentRunner != null)
			{
				CurrentRunner.ClearData();
				GameObject.Destroy(CurrentRunner.gameObject);
				CurrentRunner = null;
			}
		}
	}
}