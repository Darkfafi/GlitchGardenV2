using RaModelsSO;
using UnityEngine;

public class ModelLocator
{
    public static RaModelSOLocator Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = Resources.Load<RaModelSOLocator>(nameof(RaModelSOLocator));
            }
            return _instance;
        }
    }

    public static T GetModelSO<T>(System.Predicate<T> predicate = null)
        where T : RaModelSOBase
    {
        return Instance.GetModelSO(predicate);
    }

    private static RaModelSOLocator _instance = null;
}
