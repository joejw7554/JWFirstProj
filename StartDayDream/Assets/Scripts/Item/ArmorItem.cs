using System;
using UnityEngine;

/// <summary>
/// Enum defining armor types
/// </summary>
public enum ArmorType
{
    Cloth,
    Leather,
    Mail,
    Plate
}

/// <summary>
/// Represents armor items
/// </summary>
[Serializable]
public class ArmorItem : EquippableItem
{
    [SerializeField] private ArmorType armorType;
    [SerializeField] private int defense;
    [SerializeField] private int magicResistance;
    [SerializeField] private int healthBonus;

    public ArmorType ArmorType => armorType;
    public int Defense => defense;
    public int MagicResistance => magicResistance;
    public int HealthBonus => healthBonus;

    // Calculate total defense with enhancement bonus
    public int TotalDefense => defense + (EnhancementLevel * 3);
    public int TotalMagicResistance => magicResistance + (EnhancementLevel * 2);

    public ArmorItem(string id, string name, string desc, int price,
                    ArmorType type, EquipmentSlot slot, int def, 
                    int magicRes = 0, int hpBonus = 0,
                    int reqLevel = 1, ItemRarity rarity = ItemRarity.Common)
        : base(id, name, ItemType.Armor, desc, price, slot, reqLevel, rarity)
    {
        armorType = type;
        defense = def;
        magicResistance = magicRes;
        healthBonus = hpBonus;
    }

    /// <summary>
    /// Compare this armor with another armor
    /// </summary>
    public string CompareWith(ArmorItem other)
    {
        if (other == null)
        {
            return "No armor to compare with.";
        }

        // Only compare if same equipment slot
        if (EquipmentSlot != other.EquipmentSlot)
        {
            return $"Cannot compare {EquipmentSlot} with {other.EquipmentSlot}.";
        }

        string comparison = $"=== Comparing Armor ===\n";
        comparison += $"{ItemName} vs {other.ItemName}\n\n";
        
        int defDiff = TotalDefense - other.TotalDefense;
        comparison += $"Defense: {TotalDefense} vs {other.TotalDefense} ";
        comparison += defDiff > 0 ? $"(+{defDiff})\n" : defDiff < 0 ? $"({defDiff})\n" : "(Equal)\n";
        
        int magicResDiff = TotalMagicResistance - other.TotalMagicResistance;
        comparison += $"Magic Resistance: {TotalMagicResistance} vs {other.TotalMagicResistance} ";
        comparison += magicResDiff > 0 ? $"(+{magicResDiff})\n" : magicResDiff < 0 ? $"({magicResDiff})\n" : "(Equal)\n";
        
        int hpDiff = healthBonus - other.healthBonus;
        comparison += $"Health Bonus: {healthBonus} vs {other.healthBonus} ";
        comparison += hpDiff > 0 ? $"(+{hpDiff})\n" : hpDiff < 0 ? $"({hpDiff})\n" : "(Equal)\n";

        return comparison;
    }

    public override string GetItemInfo()
    {
        string info = base.GetItemInfo();
        info += $"Armor Type: {armorType}\n";
        info += $"Defense: {defense} (+{EnhancementLevel * 3}) = {TotalDefense}\n";
        info += $"Magic Resistance: {magicResistance} (+{EnhancementLevel * 2}) = {TotalMagicResistance}\n";
        info += $"Health Bonus: +{healthBonus} HP\n";
        return info;
    }

    public override Item Clone()
    {
        return new ArmorItem(ItemId, ItemName, Description, Price,
                           armorType, EquipmentSlot, defense, 
                           magicResistance, healthBonus,
                           RequiredLevel, Rarity);
    }
}
