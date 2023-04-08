using UnityEngine;

public abstract class SceneRootBase : MonoBehaviour
{
	protected void Awake()
	{
		OnSetup();	
	}

    protected void Start()
    {
		OnStart();
    }

	protected void OnDestroy()
	{
		OnEnd();
	}

	protected abstract void OnSetup();
	protected abstract void OnStart();
	protected abstract void OnEnd();
}
