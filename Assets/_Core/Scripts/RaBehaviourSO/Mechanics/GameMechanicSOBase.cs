using RaBehaviourSO;
using UnityEngine;

public abstract class GameMechanicSOBase : RaBehaviourSOBase
{
	protected override bool CheckDependency(ScriptableObject dependency, out string message)
	{
		if(dependency is ModelSOBase modelSO)
		{
			if(!modelSO.HasSetData)
			{
				message = "Data not set!";
				return false;
			}
		}
		message = default;
		return true;
	}
}
