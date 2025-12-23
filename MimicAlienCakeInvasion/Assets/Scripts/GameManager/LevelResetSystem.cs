using UnityEngine;

[DisallowMultipleComponent]
public sealed class LevelResetSystem : MonoBehaviour
{
    public void ResetLevel()
    {
        var resettables = Object.FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (var mb in resettables)
        {
            if (mb is IResettable resettable)
            {
                resettable.ResetState();
            }
        }

        Debug.Log("[LevelResetSystem] Level reset");
    }
}