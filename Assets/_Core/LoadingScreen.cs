using RaScenesSO;
using RaTweening;
using System;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : RaSceneLoaderBase
{
	[SerializeField]
	private CanvasGroup _group = null;

	[SerializeField]
	private Image _loadingBar = null;

	protected override void OnInitialize()
	{
		_group.alpha = 0f;
		Vector3 scale = _loadingBar.transform.localScale;
		scale.x = 0;
		_loadingBar.transform.localScale = scale;
	}

	protected override async Task DoIntro(CancellationToken token)
	{
		var tween = _group.TweenAlpha(1f, 1f);
		while(!tween.HasEnded)
		{
			await Task.Yield();
		}
	}

	protected override async Task DoOutro(CancellationToken token)
	{
		var tween = _group.TweenAlpha(0f, 1f);
		while(!tween.HasEnded)
		{
			await Task.Yield();
		}
	}

	protected override void OnDeinitialize()
	{
		RaTween.CompleteGroup(_loadingBar);
	}

	protected override void OnProgress(LoadingInfo loadingProgress)
	{
		RaTween.StopGroup(_loadingBar);
		_loadingBar.transform.TweenScaleX(loadingProgress.Progress, 0.5f).SetGroup(_loadingBar);
	}
}
