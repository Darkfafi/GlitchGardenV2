using RaTweening.Core;
using RaTweening.Core.Elements;
using System;
using System.Collections.Generic;
using UnityEngine;
using static RaTweening.RaTweenSequence;

namespace RaTweening
{
	/// <summary>
	/// This class takes control over all appended tweens submitted as sequence entries and evaluates them in sequence.
	/// OnComplete is called when all tweens have finished inside of it.
	/// > Note: A RaTweenSequence is constructed using the <see cref="RaTweenSequence.Create(EntryData[])"/> method \n
	/// > Note: Tweens can be converted easily to <see cref="EntryData"/> using the <see cref="RaTweenUtilExtensions.ToSequenceEntry(RaTweenBase, float, StaggerType)"/> method
	/// </summary>
	[RaTweenerElement(typeof(RaTweenerSequenceElement))]
	public sealed class RaTweenSequence : RaTweenBase
	{
		#region Editor Variables

		[SerializeField, SerializeReference]
		private List<EntryData> _sequenceEntries = new List<EntryData>();

		#endregion

		#region Variables

		private RaTweeningProcessor _processor = new RaTweeningProcessor();
		private int _index;
		private EntryData _headEntry;
		private float _time;
		private EntryData[] _liveSequence = null;

		#endregion

		#region Properties

		public override bool IsValid => _sequenceEntries.Count > 0;

		#endregion

		private RaTweenSequence()
			: this(new RaTweenBase[] { })
		{

		}

		private RaTweenSequence(RaTweenBase[] tweens)
			: base(0f)
		{
			tweens = tweens ?? new RaTweenBase[] { };
			EntryData[] entries = new EntryData[tweens.Length];
			for(int i = 0; i < tweens.Length; i++)
			{
				entries[i] = tweens[i].ToSequenceEntry();
			}
			Append(entries);
		}

		internal RaTweenSequence(IList<EntryData> entries)
			: base(0f)
		{
			Append(entries ?? new EntryData[] { });
		}

		#region Public Methods

		/// <summary>
		/// Creates an instance of a RaTweenSequence.
		/// > Note: This replaces the constructor \n
		/// > Note: This method uses the <see cref="Append(IList{EntryData})"/> method to append the tweens internally
		/// </summary>
		public static RaTweenSequence Create(params EntryData[] tweens)
		{
			return new RaTweenSequence(tweens).Play();
		}

		/// <summary>
		/// Appends a callback to the sequence. (converts it to a <see cref="EntryData"/>)
		/// > Note: A callback has a duration of 0 \n
		/// > Note: Consecutive callbacks are all executed within the same frame
		/// </summary>
		public RaTweenSequence AppendCallback(Action callback)
		{
			return Append(EntryData.Create(callback));
		}

		/// <summary>
		/// Appends a set of callbacks to the sequence. (converts it to a <see cref="EntryData"/>)
		/// > Note: A callback has a duration of 0 \n
		/// > Note: Consecutive callbacks are all executed within the same frame \n
		/// > Note: This method is more optimized than calling <see cref="AppendCallback(Action)"/> for each individual entry for it saves some calculation steps for when everything has been appended
		/// </summary>
		public RaTweenSequence AppendCallbacks(IList<Action> callbacks)
		{
			callbacks = callbacks ?? new Action[] { };
			EntryData[] entries = new EntryData[callbacks.Count];
			for(int i = 0; i < callbacks.Count; i++)
			{
				entries[i] = callbacks[i].ToSequenceEntry();
			}
			Append(entries);

			return this;
		}

		/// <summary>
		/// Appends a tween to the sequence. (converts it to a <see cref="EntryData"/>)
		/// > Note: The stagger has no effect when the tween is the last in the sequence \n
		/// > Note: The OnComplete is called when all tweens are finished, even if the last tween has a stagger lower than 1.
		/// </summary>
		/// <param name="stagger">The amount of % (value between 0 - 1) must be finished of this tween before the next tween of the sequence should be started</param>
		/// <param name="staggerType">The selected <see cref="StaggerType"/> which defines what the stagger parameter represents</param>
		public RaTweenSequence AppendTween(RaTweenBase tween,
			float stagger = 1f,
			StaggerType staggerType = StaggerType.FinalLoopExclDelay)
		{
			return Append(tween.ToSequenceEntry(stagger, staggerType));
		}

		/// <summary>
		/// Appends an Entry to the sequence.
		/// </summary>
		public RaTweenSequence Append(EntryData entry)
		{
			return Append(new EntryData[] { entry });
		}

		/// <summary>
		/// Appends a set of Entries to the sequence in order.
		/// > Note: This method is more optimized than calling <see cref="Append(EntryData)"/> for each individual entry for it saves some calculation steps for when everything has been appended \n
		/// > Note: This method is also used by the <see cref="Create(EntryData[])"/> method to optimize the creation of the tween
		/// </summary>
		public RaTweenSequence Append(IList<EntryData> entries)
		{
			return Insert(_sequenceEntries.Count, entries);
		}

		/// <summary>
		/// Prepends a callback to the sequence. (converts it to a <see cref="EntryData"/>)
		/// > Note: A callback has a duration of 0 \n
		/// > Note: Consecutive callbacks are all executed within the same frame
		/// </summary>
		public RaTweenSequence PrependCallback(Action callback)
		{
			return Prepend(EntryData.Create(callback));
		}

		/// <summary>
		/// Prepends a set of callbacks to the sequence. (converts it to a <see cref="EntryData"/>)
		/// > Note: A callback has a duration of 0 \n
		/// > Note: Consecutive callbacks are all executed within the same frame \n
		/// > Note: So if the sequence is [a, b, c] and you prepend [q, w, e], then the order would be [q, w, e, a, b, c] \n
		/// > Note: This method is more optimized than calling <see cref="PrependCallback(Action)"/> for each individual entry for it saves some calculation steps for when everything has been appended
		/// </summary>
		public RaTweenSequence PrependCallbacks(IList<Action> callbacks)
		{
			callbacks = callbacks ?? new Action[] { };
			EntryData[] entries = new EntryData[callbacks.Count];
			for(int i = 0; i < callbacks.Count; i++)
			{
				entries[i] = callbacks[i].ToSequenceEntry();
			}
			Prepend(entries);
			return this;
		}

		/// <summary>
		/// Preprends a tween to the sequence. (converts it to a <see cref="EntryData"/>)
		/// > Note: The stagger has no effect when the tween is the last in the sequence \n
		/// > Note: The OnComplete is called when all tweens are finished, even if the last tween has a stagger lower than 1.
		/// </summary>
		/// <param name="stagger">The amount of % (value between 0 - 1) must be finished of this tween before the next tween of the sequence should be started</param>
		/// <param name="staggerType">The selected <see cref="StaggerType"/> which defines what the stagger parameter represents</param>
		public RaTweenSequence PrependTween(RaTweenBase tween,
			float stagger = 1f,
			StaggerType staggerType = StaggerType.FinalLoopExclDelay)
		{
			return Prepend(tween.ToSequenceEntry(stagger, staggerType));
		}

		/// <summary>
		/// Preprends an Entry to the sequence.
		/// </summary>
		public RaTweenSequence Prepend(EntryData entry)
		{
			return Prepend(new EntryData[] { entry });
		}

		/// <summary>
		/// Preprends a set of Entries to the sequence in order.
		/// > Note: So if the sequence is [a, b, c] and you prepend [q, w, e], then the order would be [q, w, e, a, b, c] \n
		/// > Note: This method is more optimized than calling <see cref="Prepend(EntryData)"/> for each individual entry for it saves some calculation steps for when everything has been appended \n
		/// </summary>
		public RaTweenSequence Prepend(IList<EntryData> entries)
		{
			return Insert(0, entries);
		}

		/// <summary>
		/// Inserts a set of Entries at an index in the order it was given.
		/// > Note: So if the sequence is [a, b, c] and you insert [q, w, e] at index 0, then the order would be [q, w, e, a, b, c] \n
		/// > Note: When the index is out of range, it is clamped by the method. \n
		/// > Note: This method is more optimized than calling <see cref="Append(EntryData)"/> for each individual entry for it saves some calculation steps for when everything has been appended
		/// </summary>
		/// <param name="index">The index at which to Insert</param>
		/// <param name="entries">The entries to add to the sequence</param>
		/// <returns></returns>
		public RaTweenSequence Insert(int index, IList<EntryData> entries)
		{
			if(index <= 0)
			{
				index = 0;
			}
			else if(index > _sequenceEntries.Count)
			{
				index = _sequenceEntries.Count;
			}

			if(CanBeModified())
			{
				for(int i = entries.Count - 1; i >= 0; i--)
				{
					var entry = entries[i];
					if(entry.IsValidForAppend)
					{
						if(entry.Tween != null)
						{
							RaTweenBase tween = entry.Tween;

							if(RaTweeningEngine.HasInstance)
							{
								RaTweeningEngine.Instance.UnregisterTween(tween);
							}

							tween.SetStateInternal(State.Data);
						}

						_sequenceEntries.Insert(index, entry);
					}
				}

				CalculateDuration();
			}
			return this;
		}

		#endregion

		#region Protected Methods

		protected override void OnSetup()
		{
			base.OnSetup();
			ClearData();
			_liveSequence = new EntryData[_sequenceEntries.Count];

			for(int i = 0; i < _liveSequence.Length; i++)
			{
				_liveSequence[i] = _sequenceEntries[i].Clone();
			}
		}

		protected override void ResetData()
		{
			base.ResetData();
			ClearData();
		}

		protected override void OnKill()
		{
			base.OnKill();
			_processor?.Stop();
		}

		protected override void OnLoop()
		{
			base.OnLoop();
			ClearData();
			_processor?.Reset(false);
		}

		protected override void OnComplete()
		{
			base.OnComplete();
			_processor?.Complete(false);
		}

		protected override void ApplyReset(bool resumeAfterReset)
		{
			_processor?.Reset();
		}

		protected override void ApplyRewind(bool inclDelay)
		{
			_processor?.Rewind(inclDelay);
		}

		protected override RaTweenBase CloneSelf()
		{
			EntryData[] entries = new EntryData[_sequenceEntries.Count];

			for(int i = 0, c = entries.Length; i < c; i++)
			{
				entries[i] = _sequenceEntries[i].Clone();
			}

			return new RaTweenSequence(entries);
		}

		protected override void Dispose()
		{
			_processor.Dispose();
			ClearData();
			_liveSequence = null;
		}

		protected override void Evaluate(float normalizedValue)
		{
			if(_liveSequence != null && _liveSequence.Length > 0)
			{
				if(_headEntry.IsEmpty || _headEntry.ReadyToStartNext())
				{
					bool isCallback;
					do
					{
						isCallback = false;
						_index++;
						if(_index < _liveSequence.Length)
						{
							_headEntry = _liveSequence[_index];
							if(isCallback = _headEntry.IsCallback)
							{
								_headEntry.Callback.Invoke();
							}
							else
							{
								if(_headEntry.Tween.IsAlive(true))
								{
									_headEntry.Tween.Resume();
								}
								else
								{
									_processor.RegisterTween(_headEntry.Tween);
								}
							}
						}
					}
					while(isCallback);
				}

				float newTime = Time;
				float deltaTime = newTime - _time;
				_time = newTime;

				_processor.Step(deltaTime, false);
			}
		}

		protected override float CalculateEvaluation()
		{
			return Progress;
		}

		protected override void SetDefaultValues() { }

		#endregion

		#region Private Methods

		private void CalculateDuration()
		{
			if(IsInfinite)
			{
				return;
			}

			float newStartTime = 0f;
			float newDuration = 0f;
			for(int i = 0, c = _sequenceEntries.Count; i < c; i++)
			{
				var entry = _sequenceEntries[i];

				if(entry.IsCallback)
				{
					continue;
				}

				if(entry.Tween.IsInfiniteLoop)
				{
					SetInfiniteDuration();
					return;
				}

				float entryTweenDuration = entry.Tween.TotalLoopingDuration;
				newDuration = Mathf.Max(newStartTime + entryTweenDuration, newDuration);

				switch(entry.StaggerType)
				{
					case StaggerType.Total:
						newStartTime += entryTweenDuration * entry.Stagger;
						break;
					case StaggerType.FinalLoop:
						newStartTime += entryTweenDuration - (entry.Tween.TotalDuration * (1f - entry.Stagger));
						break;
					case StaggerType.FinalLoopExclDelay:
						newStartTime += entryTweenDuration - (entry.Tween.Duration * (1f - entry.Stagger));
						break;
					default:
						throw new Exception($"Not Implemented {entry.StaggerType}");
				}
			}

			SetDuration(newDuration);
		}

		private void ClearData()
		{
			_headEntry = default;
			_index = -1;
			_time = 0f;
		}

		#endregion

		#region Nested

		/// <summary>
		/// This struct represents an entry within a sequence. Holding the data what tween to play and their sequence settings
		/// </summary>
		public struct EntryData
		{
			#region Variables

			/// <summary>
			/// The Tween to play in the sequence
			/// </summary>
			public readonly RaTweenBase Tween;

			/// <summary>
			/// The Callback to fire within the sequence
			/// </summary>
			public readonly Action Callback;

			#endregion

			#region Properties

			/// <summary>
			/// Returns true if the EntryData represents a Tween
			/// > Note: An <see cref="EntryData"/> can either be a <see cref="IsTween"/> or a <see cref="IsCallback"/>
			/// </summary>
			public bool IsTween => Tween != null;

			/// <summary>
			/// Returns True if the EntryData Represents a Callback
			/// > Note: An <see cref="EntryData"/> can either be a <see cref="IsCallback"/> or a <see cref="IsTween"/>
			/// </summary>
			public bool IsCallback => Callback != null;

			/// <summary>
			/// The amount of % (value between 0 - 1) must be finished of this tween before the next tween of the sequence should be started
			/// </summary>
			public float Stagger
			{
				get; private set;
			}

			/// <summary>
			/// The selected <see cref="StaggerType"/> which defines what the stagger parameter represents
			/// </summary>
			public StaggerType StaggerType
			{
				get; private set;
			}

			/// <summary>
			/// Returns true if there is a tween inside the entry data
			/// </summary>
			public bool IsEmpty => !IsTween && !IsCallback;

			/// <summary>
			/// Returns true if the entry is valid for appending to the <see cref="RaTweenSequence"/>
			/// This is based on it not being empty and the <see cref="RaTweenBase.CanBeModified"/> returning true
			/// </summary>
			public bool IsValidForAppend => IsCallback || (IsTween && Tween.CanBeModified());

			#endregion

			private EntryData(RaTweenBase tween, float stagger, StaggerType staggerType)
			{
				Tween = tween;
				Stagger = stagger;
				StaggerType = staggerType;
				Callback = null;
			}

			private EntryData(Action callback)
			{
				Tween = null;
				Stagger = 0f;
				StaggerType = StaggerType.Total;
				Callback = callback;
			}

			#region Public Methods

			/// <summary>
			/// Creates an <see cref="EntryData"/> containing the given tween and its specified settings
			/// > Note: The stagger has no effect when the tween is the last in the sequence \n
			/// > Note: The OnComplete is called when all tweens are finished, even if the last tween has a stagger lower than 1.
			/// </summary>
			/// <param name="stagger">The amount of % (value between 0 - 1) must be finished of this tween before the next tween of the sequence should be started. Default: 1</param>
			/// <param name="staggerType">The selected <see cref="StaggerType"/> which defines what the stagger parameter represents. Default: <see cref="StaggerType.FinalLoopExclDelay"/></param>
			public static EntryData Create(RaTweenBase tween, float stagger = 1f, StaggerType staggerType = StaggerType.FinalLoopExclDelay)
			{
				return new EntryData(tween, stagger, staggerType);
			}

			/// <summary>
			/// Creates an <see cref="EntryData"/> containing a callback to fire
			/// > Note: This has a duration of 0;
			/// </summary>
			public static EntryData Create(Action callback)
			{
				return new EntryData(callback);
			}

			/// <summary>
			/// Sets the StaggerType of the current Entry
			/// </summary>
			/// <param name="staggerType">The selected <see cref="StaggerType"/> which defines what the stagger parameter represents. Default: <see cref="StaggerType.FinalLoopExclDelay"/></param>
			public EntryData SetStaggerType(StaggerType staggerType)
			{
				StaggerType = staggerType;
				return this;
			}

			/// <summary>
			/// Sets the Stagger of the current Entry
			/// </summary>
			/// <param name="stagger">The amount of % (value between 0 - 1) must be finished of this tween before the next tween of the sequence should be started. Default: 1</param>
			public EntryData SetStagger(float stagger)
			{
				Stagger = Mathf.Clamp01(stagger);
				return this;
			}

			/// <summary>
			/// Creates an Entry Data containing a clone of the tween within and with the same settings of the Entry Data
			/// </summary>
			/// <returns>The New Clone instance</returns>
			public EntryData Clone()
			{
				if(Tween != null)
				{
					return new EntryData(Tween.Clone(true), Stagger, StaggerType);
				}
				else
				{
					return new EntryData(Callback);
				}
			}

			#endregion

			#region Internal Methods

			internal bool ReadyToStartNext()
			{
				if(Tween == null)
				{
					return true;
				}

				switch(StaggerType)
				{
					case StaggerType.Total:
						return Tween.TotalLoopingProgress >= Stagger;
					case StaggerType.FinalLoop:
						return Tween.HasReachedLoopEnd && Tween.TotalProgress >= Stagger;
					case StaggerType.FinalLoopExclDelay:
						return Tween.HasReachedLoopEnd && Tween.HasNoDelayRemaining && Tween.Progress >= Stagger;
					default:
						throw new Exception($"Not Implemented {StaggerType}");
				}
			}

			#endregion
		}

		/// <summary>
		/// Each of the enum values represents a stagger setting
		/// </summary>
		public enum StaggerType
		{
			/// <summary>
			/// The Stagger value represents the % of the entire duration including loops & delays <see cref="RaTweenBase.TotalLoopingDuration"/>
			/// </summary>
			Total,

			/// <summary>
			/// The Stagger value represents the % of the last or only evaluation of the tween + delay <see cref="RaTweenBase.TotalDuration"/>
			/// </summary>
			FinalLoop,

			/// <summary>
			/// The Stagger value represents the % of the last or only evaluation of the tween <see cref="RaTweenBase.Duration"/>
			/// </summary>
			FinalLoopExclDelay
		}

		#endregion
	}

	public static partial class RaTweenUtilExtensions
	{
		/// <summary>
		/// Creates an <see cref="EntryData"/> containing the given tween and its specified settings
		/// > Note: The stagger has no effect when the tween is the last in the sequence \n
		/// > Note: The OnComplete is called when all tweens are finished, even if the last tween has a stagger lower than 1.
		/// </summary>
		/// <param name="stagger">The amount of % (value between 0 - 1) must be finished of this tween before the next tween of the sequence should be started. Default: 1</param>
		/// <param name="staggerType">The selected <see cref="StaggerType"/> which defines what the stagger parameter represents. Default: <see cref="StaggerType.FinalLoopExclDelay"/></param>
		/// <returns>The created <see cref="EntryData"/></returns>
		public static EntryData ToSequenceEntry(this RaTweenBase tween, float stagger = 1f, StaggerType staggerType = StaggerType.FinalLoopExclDelay)
		{
			return EntryData.Create(tween, stagger, staggerType);
		}

		/// <summary>
		/// Creates an <see cref="EntryData"/> containing the given callback
		/// > Note: A callback has a duration of 0 \n
		/// > Note: Consecutive callbacks are all executed within the same frame
		/// </summary>
		/// <returns>The created <see cref="EntryData"/></returns>
		public static EntryData ToSequenceEntry(this Action callback)
		{
			return EntryData.Create(callback);
		}
	}
}