using System;
using UnityEngine;

/// <summary>
/// Enum defining weapon types
/// </summary>
public enum WeaponType
{
    Sword,
    Axe,
    Bow,
    Staff,
    Dagger,
    Spear
}

/// <summary>
/// Represents weapon items
/// </summary>
[Serializable]
public class WeaponItem : EquippableItem
{
    [SerializeField] private WeaponType weaponType;
    [SerializeField] private int attackPower;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float criticalChance;

    public WeaponType WeaponType => weaponType;
    public int AttackPower => attackPower;
    public float AttackSpeed => attackSpeed;
    public float CriticalChance => criticalChance;

    // Calculate total attack power with enhancement bonus
    public int TotalAttackPower => attackPower + (EnhancementLevel * 5);

    public WeaponItem(string id, string name, string desc, int price,
                     WeaponType type, int attack, float attackSpd = 1.0f,
                     float critChance = 0.05f, EquipmentSlot slot = EquipmentSlot.MainHand,
                     int reqLevel = 1, ItemRarity rarity = ItemRarity.Common)
        : base(id, name, ItemType.Weapon, desc, price, slot, reqLevel, rarity)
    {
        weaponType = type;
        attackPower = attack;
        attackSpeed = attackSpd;
        criticalChance = critChance;
    }

    /// <summary>
    /// Compare this weapon with another weapon
    /// </summary>
    public string CompareWith(WeaponItem other)
    {
        if (other == null)
        {
            return "No weapon to compare with.";
        }

        string comparison = $"=== Comparing Weapons ===\n";
        comparison += $"{ItemName} vs {other.ItemName}\n\n";
        
        int attackDiff = TotalAttackPower - other.TotalAttackPower;
        comparison += $"Attack Power: {TotalAttackPower} vs {other.TotalAttackPower} ";
        comparison += attackDiff > 0 ? $"(+{attackDiff})\n" : attackDiff < 0 ? $"({attackDiff})\n" : "(Equal)\n";
        
        float speedDiff = attackSpeed - other.attackSpeed;
        comparison += $"Attack Speed: {attackSpeed:F2} vs {other.attackSpeed:F2} ";
        comparison += speedDiff > 0 ? $"(+{speedDiff:F2})\n" : speedDiff < 0 ? $"({speedDiff:F2})\n" : "(Equal)\n";
        
        float critDiff = criticalChance - other.criticalChance;
        comparison += $"Critical Chance: {criticalChance:P0} vs {other.criticalChance:P0} ";
        comparison += critDiff > 0 ? $"(+{critDiff:P0})\n" : critDiff < 0 ? $"({critDiff:P0})\n" : "(Equal)\n";

        return comparison;
    }

    public override string GetItemInfo()
    {
        string info = base.GetItemInfo();
        info += $"Weapon Type: {weaponType}\n";
        info += $"Attack Power: {attackPower} (+{EnhancementLevel * 5}) = {TotalAttackPower}\n";
        info += $"Attack Speed: {attackSpeed:F2}\n";
        info += $"Critical Chance: {criticalChance:P0}\n";
        return info;
    }

    public override Item Clone()
    {
        return new WeaponItem(ItemId, ItemName, Description, Price,
                            weaponType, attackPower, attackSpeed, criticalChance,
                            EquipmentSlot, RequiredLevel, Rarity);
    }
}
