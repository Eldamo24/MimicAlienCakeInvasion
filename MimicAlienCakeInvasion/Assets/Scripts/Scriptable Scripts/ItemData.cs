using UnityEngine;

[CreateAssetMenu(fileName = "ItemData_", menuName = "Game/Items/Item Data", order = 0)]
public sealed class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string itemId;           
    public string displayName;      

    [Header("Debug")]
    [TextArea]
    public string description;
}