using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemData", menuName = "ScriptableObjects/ItemsData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite itemIcon;
    public enum ItemType { HpUp, DamageUp, SpeedUp, AttackSpeedUp }
    public ItemType itemType;
    public float itemValue;
    public float dropChance;
}
