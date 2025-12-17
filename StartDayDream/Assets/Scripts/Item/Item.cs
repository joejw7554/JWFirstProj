using System;
using UnityEngine;

/// <summary>
/// Base class for all items in the game
/// </summary>
[Serializable]
public class Item
{
    [SerializeField] private string itemName;
    [SerializeField] private string itemId;
    [SerializeField] private ItemType itemType;
    [SerializeField] private ItemRarity rarity;
    [SerializeField] private string description;
    [SerializeField] private int price;
    [SerializeField] private bool isStackable;
    [SerializeField] private int maxStackSize;
    [SerializeField] private int currentStackSize;
    [SerializeField] private Sprite icon;

    public string ItemName => itemName;
    public string ItemId => itemId;
    public ItemType ItemType => itemType;
    public ItemRarity Rarity => rarity;
    public string Description => description;
    public int Price => price;
    public bool IsStackable => isStackable;
    public int MaxStackSize => maxStackSize;
    public int CurrentStackSize => currentStackSize;
    public Sprite Icon => icon;

    public Item(string id, string name, ItemType type, string desc, int price, 
                ItemRarity rarity = ItemRarity.Common, bool stackable = false, 
                int maxStack = 1)
    {
        itemId = id;
        itemName = name;
        itemType = type;
        description = desc;
        this.price = price;
        this.rarity = rarity;
        isStackable = stackable;
        maxStackSize = stackable ? maxStack : 1;
        currentStackSize = 1;
    }

    /// <summary>
    /// Add items to the current stack
    /// </summary>
    /// <returns>Number of items that couldn't be added (overflow)</returns>
    public int AddToStack(int amount)
    {
        if (!isStackable)
        {
            return amount;
        }

        int totalAmount = currentStackSize + amount;
        if (totalAmount <= maxStackSize)
        {
            currentStackSize = totalAmount;
            return 0;
        }

        currentStackSize = maxStackSize;
        return totalAmount - maxStackSize;
    }

    /// <summary>
    /// Remove items from the current stack
    /// </summary>
    /// <returns>True if successful, false if not enough items</returns>
    public bool RemoveFromStack(int amount)
    {
        if (currentStackSize >= amount)
        {
            currentStackSize -= amount;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Get detailed information about the item
    /// </summary>
    public virtual string GetItemInfo()
    {
        string info = $"=== {itemName} ===\n";
        info += $"Type: {itemType}\n";
        info += $"Rarity: {rarity}\n";
        info += $"Price: {price} Gold\n";
        info += $"Description: {description}\n";
        
        if (isStackable)
        {
            info += $"Stack: {currentStackSize}/{maxStackSize}\n";
        }

        return info;
    }

    /// <summary>
    /// Clone the item (useful for creating new instances)
    /// </summary>
    public virtual Item Clone()
    {
        return new Item(itemId, itemName, itemType, description, price, 
                       rarity, isStackable, maxStackSize);
    }
}
