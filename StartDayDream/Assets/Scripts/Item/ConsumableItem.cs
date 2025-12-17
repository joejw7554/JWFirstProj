using System;
using UnityEngine;

/// <summary>
/// Enum defining types of consumable effects
/// </summary>
public enum ConsumableEffect
{
    HealthRestore,
    ManaRestore,
    StrengthBoost,
    DefenseBoost,
    SpeedBoost
}

/// <summary>
/// Represents consumable items like potions and food
/// </summary>
[Serializable]
public class ConsumableItem : Item
{
    [SerializeField] private ConsumableEffect effectType;
    [SerializeField] private int effectAmount;
    [SerializeField] private float effectDuration;

    public ConsumableEffect EffectType => effectType;
    public int EffectAmount => effectAmount;
    public float EffectDuration => effectDuration;

    public ConsumableItem(string id, string name, string desc, int price,
                         ConsumableEffect effect, int amount, float duration = 0f,
                         ItemRarity rarity = ItemRarity.Common, int maxStack = 99)
        : base(id, name, ItemType.Consumable, desc, price, rarity, true, maxStack)
    {
        effectType = effect;
        effectAmount = amount;
        effectDuration = duration;
    }

    /// <summary>
    /// Use the consumable item and apply its effect
    /// </summary>
    /// <returns>True if successfully consumed</returns>
    public bool Consume()
    {
        if (CurrentStackSize <= 0)
        {
            return false;
        }

        RemoveFromStack(1);
        ApplyEffect();
        return true;
    }

    /// <summary>
    /// Apply the consumable's effect
    /// </summary>
    private void ApplyEffect()
    {
        string effectMessage = $"Applied {effectType}: ";
        
        switch (effectType)
        {
            case ConsumableEffect.HealthRestore:
                effectMessage += $"Restored {effectAmount} HP";
                break;
            case ConsumableEffect.ManaRestore:
                effectMessage += $"Restored {effectAmount} MP";
                break;
            case ConsumableEffect.StrengthBoost:
                effectMessage += $"Increased Strength by {effectAmount} for {effectDuration}s";
                break;
            case ConsumableEffect.DefenseBoost:
                effectMessage += $"Increased Defense by {effectAmount} for {effectDuration}s";
                break;
            case ConsumableEffect.SpeedBoost:
                effectMessage += $"Increased Speed by {effectAmount} for {effectDuration}s";
                break;
        }
        
        Debug.Log(effectMessage);
    }

    public override string GetItemInfo()
    {
        string info = base.GetItemInfo();
        info += $"Effect: {effectType}\n";
        info += $"Amount: {effectAmount}\n";
        
        if (effectDuration > 0)
        {
            info += $"Duration: {effectDuration}s\n";
        }
        
        return info;
    }

    public override Item Clone()
    {
        return new ConsumableItem(ItemId, ItemName, Description, Price,
                                 effectType, effectAmount, effectDuration,
                                 Rarity, MaxStackSize);
    }
}
