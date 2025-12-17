using System;
using UnityEngine;

/// <summary>
/// Enum defining equipment slots
/// </summary>
public enum EquipmentSlot
{
    Head,
    Chest,
    Legs,
    Feet,
    Hands,
    MainHand,
    OffHand,
    TwoHand
}

/// <summary>
/// Base class for equippable items (weapons and armor)
/// </summary>
[Serializable]
public abstract class EquippableItem : Item
{
    [SerializeField] private EquipmentSlot equipmentSlot;
    [SerializeField] private int requiredLevel;
    [SerializeField] private bool isEquipped;
    [SerializeField] private int enhancementLevel;
    [SerializeField] private int maxEnhancementLevel;

    public EquipmentSlot EquipmentSlot => equipmentSlot;
    public int RequiredLevel => requiredLevel;
    public bool IsEquipped => isEquipped;
    public int EnhancementLevel => enhancementLevel;
    public int MaxEnhancementLevel => maxEnhancementLevel;

    protected EquippableItem(string id, string name, ItemType type, string desc, 
                            int price, EquipmentSlot slot, int reqLevel = 1,
                            ItemRarity rarity = ItemRarity.Common, int maxEnhancement = 10)
        : base(id, name, type, desc, price, rarity, false, 1)
    {
        equipmentSlot = slot;
        requiredLevel = reqLevel;
        isEquipped = false;
        enhancementLevel = 0;
        maxEnhancementLevel = maxEnhancement;
    }

    /// <summary>
    /// Equip the item
    /// </summary>
    public virtual bool Equip()
    {
        if (isEquipped)
        {
            Debug.LogWarning($"{ItemName} is already equipped!");
            return false;
        }

        isEquipped = true;
        Debug.Log($"Equipped {ItemName}");
        return true;
    }

    /// <summary>
    /// Unequip the item
    /// </summary>
    public virtual bool Unequip()
    {
        if (!isEquipped)
        {
            Debug.LogWarning($"{ItemName} is not equipped!");
            return false;
        }

        isEquipped = false;
        Debug.Log($"Unequipped {ItemName}");
        return true;
    }

    /// <summary>
    /// Enhance the item to increase its power
    /// </summary>
    /// <returns>True if enhancement was successful</returns>
    public bool Enhance()
    {
        if (enhancementLevel >= maxEnhancementLevel)
        {
            Debug.LogWarning($"{ItemName} is already at max enhancement level!");
            return false;
        }

        enhancementLevel++;
        Debug.Log($"{ItemName} enhanced to level {enhancementLevel}");
        return true;
    }

    public override string GetItemInfo()
    {
        string info = base.GetItemInfo();
        info += $"Equipment Slot: {equipmentSlot}\n";
        info += $"Required Level: {requiredLevel}\n";
        info += $"Enhancement: +{enhancementLevel}\n";
        info += $"Status: {(isEquipped ? "Equipped" : "Not Equipped")}\n";
        return info;
    }
}
