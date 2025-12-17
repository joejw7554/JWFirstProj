using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages equipped items and equipment slots
/// </summary>
[Serializable]
public class EquipmentManager
{
    [SerializeField] private Dictionary<EquipmentSlot, EquippableItem> equippedItems;
    [SerializeField] private int playerLevel;

    public int PlayerLevel => playerLevel;

    public EquipmentManager(int level = 1)
    {
        equippedItems = new Dictionary<EquipmentSlot, EquippableItem>();
        playerLevel = level;
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        // Initialize all equipment slots as empty
        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            if (!equippedItems.ContainsKey(slot))
            {
                equippedItems[slot] = null;
            }
        }
    }

    /// <summary>
    /// Equip an item to the appropriate slot
    /// </summary>
    public bool EquipItem(EquippableItem item)
    {
        if (item == null)
        {
            Debug.LogError("Cannot equip null item");
            return false;
        }

        // Check level requirement
        if (playerLevel < item.RequiredLevel)
        {
            Debug.LogWarning($"Cannot equip {item.ItemName}. Required level: {item.RequiredLevel}");
            return false;
        }

        EquipmentSlot slot = item.EquipmentSlot;

        // Handle two-handed weapons
        if (slot == EquipmentSlot.TwoHand)
        {
            // Unequip main hand and off hand
            UnequipSlot(EquipmentSlot.MainHand);
            UnequipSlot(EquipmentSlot.OffHand);
        }
        else if (slot == EquipmentSlot.MainHand || slot == EquipmentSlot.OffHand)
        {
            // Unequip two-handed weapon if equipped
            UnequipSlot(EquipmentSlot.TwoHand);
        }

        // Unequip current item in slot if exists
        if (equippedItems[slot] != null)
        {
            UnequipSlot(slot);
        }

        // Equip the new item
        equippedItems[slot] = item;
        item.Equip();
        Debug.Log($"Equipped {item.ItemName} to {slot}");
        return true;
    }

    /// <summary>
    /// Unequip an item from a specific slot
    /// </summary>
    public EquippableItem UnequipSlot(EquipmentSlot slot)
    {
        if (equippedItems[slot] == null)
        {
            Debug.LogWarning($"No item equipped in {slot} slot");
            return null;
        }

        EquippableItem item = equippedItems[slot];
        item.Unequip();
        equippedItems[slot] = null;
        Debug.Log($"Unequipped {item.ItemName} from {slot}");
        return item;
    }

    /// <summary>
    /// Get equipped item from a specific slot
    /// </summary>
    public EquippableItem GetEquippedItem(EquipmentSlot slot)
    {
        return equippedItems.ContainsKey(slot) ? equippedItems[slot] : null;
    }

    /// <summary>
    /// Check if a slot has an item equipped
    /// </summary>
    public bool IsSlotEquipped(EquipmentSlot slot)
    {
        return equippedItems.ContainsKey(slot) && equippedItems[slot] != null;
    }

    /// <summary>
    /// Get all equipped items
    /// </summary>
    public List<EquippableItem> GetAllEquippedItems()
    {
        List<EquippableItem> equipped = new List<EquippableItem>();
        
        foreach (var kvp in equippedItems)
        {
            if (kvp.Value != null)
            {
                equipped.Add(kvp.Value);
            }
        }

        return equipped;
    }

    /// <summary>
    /// Calculate total stats from all equipped items
    /// </summary>
    public EquipmentStats GetTotalStats()
    {
        EquipmentStats stats = new EquipmentStats();

        foreach (var kvp in equippedItems)
        {
            if (kvp.Value == null) continue;

            if (kvp.Value is WeaponItem weapon)
            {
                stats.TotalAttackPower += weapon.TotalAttackPower;
                stats.TotalAttackSpeed += weapon.AttackSpeed;
                stats.TotalCriticalChance += weapon.CriticalChance;
            }
            else if (kvp.Value is ArmorItem armor)
            {
                stats.TotalDefense += armor.TotalDefense;
                stats.TotalMagicResistance += armor.TotalMagicResistance;
                stats.TotalHealthBonus += armor.HealthBonus;
            }
        }

        return stats;
    }

    /// <summary>
    /// Unequip all items
    /// </summary>
    public List<EquippableItem> UnequipAll()
    {
        List<EquippableItem> unequippedItems = new List<EquippableItem>();

        foreach (EquipmentSlot slot in Enum.GetValues(typeof(EquipmentSlot)))
        {
            if (equippedItems[slot] != null)
            {
                EquippableItem item = UnequipSlot(slot);
                if (item != null)
                {
                    unequippedItems.Add(item);
                }
            }
        }

        return unequippedItems;
    }

    /// <summary>
    /// Set player level
    /// </summary>
    public void SetPlayerLevel(int level)
    {
        if (level < 1)
        {
            Debug.LogWarning("Player level must be at least 1");
            return;
        }

        playerLevel = level;
        Debug.Log($"Player level set to {level}");
    }

    /// <summary>
    /// Get equipment summary
    /// </summary>
    public string GetEquipmentSummary()
    {
        string summary = "=== Equipment ===\n";
        summary += $"Player Level: {playerLevel}\n\n";

        bool hasEquipment = false;

        foreach (var kvp in equippedItems)
        {
            if (kvp.Value != null)
            {
                hasEquipment = true;
                summary += $"[{kvp.Key}] {kvp.Value.ItemName} +{kvp.Value.EnhancementLevel}\n";
            }
        }

        if (!hasEquipment)
        {
            summary += "No equipment equipped.\n";
        }

        summary += "\n--- Total Stats ---\n";
        EquipmentStats stats = GetTotalStats();
        summary += stats.ToString();

        return summary;
    }
}

/// <summary>
/// Struct to hold total equipment stats
/// </summary>
[Serializable]
public struct EquipmentStats
{
    public int TotalAttackPower;
    public float TotalAttackSpeed;
    public float TotalCriticalChance;
    public int TotalDefense;
    public int TotalMagicResistance;
    public int TotalHealthBonus;

    public override string ToString()
    {
        string stats = "";
        
        if (TotalAttackPower > 0)
            stats += $"Attack Power: {TotalAttackPower}\n";
        
        if (TotalAttackSpeed > 0)
            stats += $"Attack Speed: {TotalAttackSpeed:F2}\n";
        
        if (TotalCriticalChance > 0)
            stats += $"Critical Chance: {TotalCriticalChance:P0}\n";
        
        if (TotalDefense > 0)
            stats += $"Defense: {TotalDefense}\n";
        
        if (TotalMagicResistance > 0)
            stats += $"Magic Resistance: {TotalMagicResistance}\n";
        
        if (TotalHealthBonus > 0)
            stats += $"Health Bonus: +{TotalHealthBonus} HP\n";

        return stats.Length > 0 ? stats : "No stats from equipment.\n";
    }
}
