using RaFSM;
using RaTweening;
using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Battle
{
	public class BattleGameIntroductionState : RaGOStateBase<BattleGameSceneRoot>
	{
		public RaGOStateEvent CompletedIntroductionEvent;

		[Header("References")]
		[SerializeField]
		[FormerlySerializedAs("_gridModelSO")]
		private BattleGridReferenceSO _gridReferenceSO = null;

		private RaTweenSequence _sequence = null;
		private float _cameraZoomLevel = 0f;

		protected override void OnInit()
		{
			
		}

		protected override void OnEnter()
		{
			_gridReferenceSO.Grid.ForEach((pos, element) =>
			{
				element.gameObject.SetActive(false);
			});

			_cameraZoomLevel = Camera.main.orthographicSize;
			_sequence = RaTweenSequence.Create();
			_sequence.SetDelay(0.5f);
			_sequence.Append(Camera.main.TweenOrthoSize(_cameraZoomLevel + 0.2f, 0.5f).SetEasing(RaEasingType.OutQuad).ToSequenceEntry(0.5f));
			_sequence.Append(_gridReferenceSO.Grid.GridHighlightRenderer.TweenColorA(0.4f, 0.25f).ToSequenceEntry());
			_sequence.Append(_gridReferenceSO.Grid.GridHighlightRenderer.transform.TweenPunchPos(Vector3.one * 0.1f, 0.1f).ToSequenceEntry());
			_sequence.AppendCallback(() =>
			{
				_gridReferenceSO.Grid.ForEach((pos, element) =>
				{
					float p = (float)pos.x / _gridReferenceSO.Grid.GridData.Size.x;
					element.transform.TweenPunchScale(Vector3.one * 0.15f, 0.45f)
					.SetDelay(p * 1.75f)
						.OnStart(() => element.gameObject.SetActive(true))
						.OnComplete(() => element.transform.localScale = Vector3.one);
				});
			});
			_sequence.Append(Camera.main.TweenOrthoSize(_cameraZoomLevel, 0.5f).SetEasing(RaEasingType.OutQuad).SetDelay(2f).ToSequenceEntry(0.5f));
			_sequence.Append(_gridReferenceSO.Grid.GridHighlightRenderer.TweenColorA(0f, 0.25f).ToSequenceEntry());
			_sequence.OnComplete(() => CompletedIntroductionEvent.Invoke(this));
		}

		protected override void OnExit(bool isSwitch)
		{
			if(_sequence != null)
			{
				_sequence.Stop();
				_sequence = null;
			}

			if(isSwitch)
			{
				Camera.main.orthographicSize = _cameraZoomLevel;
				_gridReferenceSO.Grid.ForEach((pos, element) =>
				{
					element.gameObject.SetActive(true);
					element.transform.localScale = Vector3.one;
				});
				_cameraZoomLevel = default;
			}
		}

		protected override void OnDeinit()
		{

		}
	}
}