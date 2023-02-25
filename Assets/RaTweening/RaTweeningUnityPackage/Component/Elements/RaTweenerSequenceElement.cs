using System;
using System.Collections.Generic;
using UnityEngine;
using static RaTweening.RaTweenSequence;

namespace RaTweening.Core.Elements
{
	/// <summary>
	/// The element representing <see cref="RaTweenSequence"/> tweens within the <see cref="RaTweenerComponent"/>
	/// </summary>
	public class RaTweenerSequenceElement : RaTweenerElementBase
	{
		#region Editor Variables

		[SerializeField, HideInInspector]
		private List<EntryData> _sequenceEntries = new List<EntryData>();

		#endregion

		#region Lifecycle

		protected void OnDestroy()
		{
			if(_sequenceEntries != null && _sequenceEntries.Count > 0)
			{
				for(int i = _sequenceEntries.Count - 1; i >= 0; i--)
				{
					RaTweenerElementBase tweenElement = _sequenceEntries[i].TweenElement;
					if(tweenElement)
					{
#if UNITY_EDITOR
						if(RaTweenerComponentEditor.TryRemoveTween(tweenElement))
						{
							_sequenceEntries.RemoveAt(i);
						}
#else
						if(Application.isPlaying)
						{
							Destroy(tweenElement);
							_sequenceEntries.RemoveAt(i);
						}
#endif
					}
				}
				_sequenceEntries.Clear();
			}
		}

		#endregion

		#region Protected Methods

		protected override void Init(Type tweenType)
		{
			_sequenceEntries = new List<EntryData>();
		}

		protected override void CalculateDefaultValues()
		{
			if(_sequenceEntries != null && _sequenceEntries.Count > 0)
			{
				for(int i = _sequenceEntries.Count - 1; i >= 0; i--)
				{
					var element = _sequenceEntries[i].TweenElement;
					if(element != null)
					{
						element.CalculateDefaultValuesInternal();
					}
				}
			}
		}

		protected override RaTweenBase CreateTweenCore()
		{
			RaTweenSequence.EntryData[] entries = new RaTweenSequence.EntryData[_sequenceEntries.Count];
			for(int i = 0, c = entries.Length; i < c; i++)
			{
				var entry = _sequenceEntries[i];
				entries[i] = RaTweenSequence.EntryData.Create(entry.TweenElement.CreateTween(), entry.Stagger, entry.StaggerType);
			}
			return new RaTweenSequence(entries);
		}

		protected override string GetElementName()
		{
			return nameof(RaTweenSequence);
		}

		#endregion

		#region Internal Methods

		internal bool RegisterTweenElement(RaTweenerElementBase element)
		{
			_sequenceEntries.Add(new EntryData()
			{
				TweenElement = element,
				Stagger = 1f,
				StaggerType = StaggerType.FinalLoopExclDelay
			});

			return true;
		}

		internal bool UnregisterTweenElement(RaTweenerElementBase element)
		{
			for(int i = _sequenceEntries.Count - 1; i >= 0; i--)
			{
				var entry = _sequenceEntries[i];
				if(entry.TweenElement == element)
				{
					_sequenceEntries.RemoveAt(i);
					return true;
				}
			}
			return false;
		}

		#endregion

		#region Nested

		[Serializable]
		private struct EntryData
		{
			[SerializeReference]
			public RaTweenerElementBase TweenElement;
			[Range(0f, 1f)]
			public float Stagger;
			public StaggerType StaggerType;
		}

		#endregion
	}
}