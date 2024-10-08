using System;
using UnityEngine;

public enum WeaponAttachmentType
{
    Magazine
}

[Serializable]
public class WeaponAttachmentAttribute
{
    public PlayerStatsTypes type;
    public float buff;
}

[CreateAssetMenu(fileName = "WeaponAttachment", menuName = "Item/Weapon Attachment")]
public class SOWeaponAttachment : ScriptableObject
{
    public string attachmentName;
    public Sprite attachmentIcon;
    public WeaponAttachmentType attachmentType;
}
