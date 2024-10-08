using System;
using UnityEngine;

[Serializable]
public class ItemUpgradeAttribute
{
    public PlayerStatsTypes type;
    public float buff;
}

[CreateAssetMenu(fileName = "ItemUpgrade", menuName = "Item/Upgrades")]
public class SOItemUpgrades : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public ItemUpgradeAttribute[] attributes;
}
