using UnityEngine;

[DisallowMultipleComponent]
public sealed class LevelResetSystem : MonoBehaviour
{
    public void ResetLevel()
    {
        var behaviours = Object.FindObjectsByType<MonoBehaviour>(
            FindObjectsInactive.Include,
            FindObjectsSortMode.None
        );

        foreach (var mb in behaviours)
        {
            if (mb is IResettable resettable)
                resettable.ResetState();
        }

        Debug.Log("[LevelResetSystem] Level reset");
    }
}