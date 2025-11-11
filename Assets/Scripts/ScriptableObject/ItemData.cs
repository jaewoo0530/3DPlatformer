using UnityEngine;

public enum ItemType
{
    Equipable,
    Consumable
}

[CreateAssetMenu(fileName = "Item", menuName = "New Item")]
public class ItemData : ScriptableObject
{
    [Header("Info")]
    public string displayName;
    public string description;
    public ItemType type;

    [Header("Equip")]
    public GameObject equipPrefab;
}