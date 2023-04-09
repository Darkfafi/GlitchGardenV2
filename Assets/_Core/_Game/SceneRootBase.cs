using RaModelsSO;
using UnityEngine;

public abstract class SceneRootBase : MonoBehaviour
{
	[field: SerializeField, Header("Base")]
	protected RaModelSOCollection Models
	{
		get; private set;
	}

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
