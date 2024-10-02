using System;
using UnityEngine;

public enum ConsumableTypes
{
    Heal,
    Shield,
    Boost
}

[Serializable]
public class ItemConsumableAttributes
{
    public ConsumableTypes type;
    public float buff;
    public float timeInSeconds;
}

[CreateAssetMenu(fileName = "ItemConsumables", menuName = "Item/Consumables")]
public class SOItemConsumables : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemConsumableAttributes[] attributes;
}