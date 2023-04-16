using RaModelsSO;
using UnityEngine;

public abstract class SceneRootBase : MonoBehaviour
{
	[field: SerializeField, Header("Base")]
	protected RaModelSOLocator Models
	{
		get; private set;
	}

	protected void Awake()
	{
		OnSetup();	
	}

    protected void Start()
    {
		if(TryGetLoadingScreen(out LoadingScreen loadingScreen))
		{
			loadingScreen.LoadingEndedEvent.AddListener(OnStart);
		}
		else
		{
			OnStart();
		}
    }

	protected void OnDestroy()
	{
		OnEnd();
	}

	protected abstract void OnSetup();
	protected abstract void OnStart();
	protected abstract void OnEnd();

	private bool TryGetLoadingScreen(out LoadingScreen loadingScreen)
	{
		loadingScreen = FindObjectOfType<LoadingScreen>();
		return loadingScreen != null;
	}
}
