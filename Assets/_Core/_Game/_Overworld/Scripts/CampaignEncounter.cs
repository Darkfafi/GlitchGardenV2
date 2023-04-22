using System;
using UnityEngine;
using RaCollection;

namespace Game.Campaign
{
	public class CampaignEncounter : IDisposable, IRaCollectionElement
	{
		public delegate void EncounterHandler(CampaignEncounter encounter);
		
		public event EncounterHandler EnteredEncounterEvent;
		public event EncounterHandler ExitedEncounterEvent;

		public event EncounterHandler StartedEncounterEvent;
		public event EncounterHandler ProgressedEncounterEvent;
		public event EncounterHandler EndedEncounterEvent;

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

		public bool IsEncounterRunning
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

			IsEncounterRunning = true;
			CurrentRunner = GameObject.Instantiate(Config.RunnerPrefab);
			GameObject.DontDestroyOnLoad(CurrentRunner);
			CurrentRunner.name = $"Encounter ({Id})";
			CurrentRunner.SetData(this);

			CurrentRunner.EncounterEndedEvent += OnEncounterEndedEvent;
			CurrentRunner.EncounterProgressedEvent += OnEncounterProgressedEvent;
			CurrentRunner.EncounterStartedEvent += OnEncounterStartedEvent;

			EnteredEncounterEvent?.Invoke(this);
		}

		public void Exit()
		{
			if(IsEncounterRunning)
			{
				IsEncounterRunning = false;
				if(CurrentRunner != null)
				{
					CurrentRunner.EncounterEndedEvent -= OnEncounterEndedEvent;
					CurrentRunner.EncounterProgressedEvent -= OnEncounterProgressedEvent;
					CurrentRunner.EncounterStartedEvent -= OnEncounterStartedEvent;

					CurrentRunner.ClearData();
					GameObject.Destroy(CurrentRunner.gameObject);
					CurrentRunner = null;
				}
				ExitedEncounterEvent?.Invoke(this);
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
			IsEncounterRunning = false;
		}

		private void OnEncounterStartedEvent(CampaignEncounterRunner runner)
		{
			StartedEncounterEvent?.Invoke(this);
		}

		private void OnEncounterProgressedEvent(CampaignEncounterRunner runner)
		{
			ProgressedEncounterEvent?.Invoke(this);
		}

		private void OnEncounterEndedEvent(CampaignEncounterRunner runner)
		{
			EndedEncounterEvent?.Invoke(this);
		}
	}
}