using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages the player's inventory system
/// </summary>
[Serializable]
public class Inventory
{
    [SerializeField] private List<Item> items;
    [SerializeField] private int maxCapacity;
    [SerializeField] private int gold;

    public int MaxCapacity => maxCapacity;
    public int CurrentCapacity => items.Count;
    public int Gold => gold;
    public List<Item> Items => items;

    public Inventory(int capacity = 100, int startingGold = 0)
    {
        items = new List<Item>();
        maxCapacity = capacity;
        gold = startingGold;
    }

    /// <summary>
    /// Add an item to the inventory
    /// </summary>
    public bool AddItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("Cannot add null item to inventory");
            return false;
        }

        // Check for stackable items
        if (item.IsStackable)
        {
            Item existingItem = items.FirstOrDefault(i => 
                i.ItemId == item.ItemId && 
                i.CurrentStackSize < i.MaxStackSize);

            if (existingItem != null)
            {
                int overflow = existingItem.AddToStack(item.CurrentStackSize);
                
                // If there's overflow, create a new stack
                if (overflow > 0)
                {
                    if (CurrentCapacity >= maxCapacity)
                    {
                        Debug.LogWarning("Inventory is full!");
                        return false;
                    }

                    Item newStack = item.Clone();
                    newStack.RemoveFromStack(item.CurrentStackSize - overflow);
                    items.Add(newStack);
                }

                Debug.Log($"Added {item.CurrentStackSize} {item.ItemName}(s) to inventory");
                return true;
            }
        }

        // Add as new item
        if (CurrentCapacity >= maxCapacity)
        {
            Debug.LogWarning("Inventory is full!");
            return false;
        }

        items.Add(item);
        Debug.Log($"Added {item.ItemName} to inventory");
        return true;
    }

    /// <summary>
    /// Remove an item from the inventory
    /// </summary>
    public bool RemoveItem(Item item)
    {
        if (item == null || !items.Contains(item))
        {
            Debug.LogWarning("Item not found in inventory");
            return false;
        }

        items.Remove(item);
        Debug.Log($"Removed {item.ItemName} from inventory");
        return true;
    }

    /// <summary>
    /// Remove an item by ID and optional amount
    /// </summary>
    public bool RemoveItemById(string itemId, int amount = 1)
    {
        List<Item> matchingItems = items.Where(i => i.ItemId == itemId).ToList();
        
        if (matchingItems.Count == 0)
        {
            Debug.LogWarning($"Item with ID {itemId} not found");
            return false;
        }

        int remainingToRemove = amount;
        List<Item> itemsToRemove = new List<Item>();

        foreach (Item item in matchingItems)
        {
            if (item.IsStackable)
            {
                int stackSize = item.CurrentStackSize;
                
                if (stackSize >= remainingToRemove)
                {
                    item.RemoveFromStack(remainingToRemove);
                    
                    if (item.CurrentStackSize == 0)
                    {
                        itemsToRemove.Add(item);
                    }
                    
                    remainingToRemove = 0;
                    break;
                }
                else
                {
                    remainingToRemove -= stackSize;
                    itemsToRemove.Add(item);
                }
            }
            else
            {
                itemsToRemove.Add(item);
                remainingToRemove--;
                
                if (remainingToRemove == 0)
                {
                    break;
                }
            }
        }

        // Remove all marked items
        foreach (Item item in itemsToRemove)
        {
            items.Remove(item);
        }

        return remainingToRemove == 0;
    }

    /// <summary>
    /// Get item by ID
    /// </summary>
    public Item GetItemById(string itemId)
    {
        return items.FirstOrDefault(i => i.ItemId == itemId);
    }

    /// <summary>
    /// Get all items of a specific type
    /// </summary>
    public List<Item> GetItemsByType(ItemType type)
    {
        return items.Where(i => i.ItemType == type).ToList();
    }

    /// <summary>
    /// Check if inventory has an item
    /// </summary>
    public bool HasItem(string itemId, int amount = 1)
    {
        List<Item> matchingItems = items.Where(i => i.ItemId == itemId).ToList();
        
        if (matchingItems.Count == 0)
        {
            return false;
        }

        int totalAmount = matchingItems.Sum(i => i.CurrentStackSize);
        return totalAmount >= amount;
    }

    /// <summary>
    /// Add gold to inventory
    /// </summary>
    public void AddGold(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot add negative gold");
            return;
        }

        gold += amount;
        Debug.Log($"Added {amount} gold. Total: {gold}");
    }

    /// <summary>
    /// Remove gold from inventory
    /// </summary>
    public bool RemoveGold(int amount)
    {
        if (amount < 0)
        {
            Debug.LogWarning("Cannot remove negative gold");
            return false;
        }

        if (gold < amount)
        {
            Debug.LogWarning("Not enough gold");
            return false;
        }

        gold -= amount;
        Debug.Log($"Removed {amount} gold. Remaining: {gold}");
        return true;
    }

    /// <summary>
    /// Sort inventory by various criteria
    /// </summary>
    public void SortInventory(InventorySortType sortType)
    {
        switch (sortType)
        {
            case InventorySortType.Name:
                items = items.OrderBy(i => i.ItemName).ToList();
                break;
            case InventorySortType.Type:
                items = items.OrderBy(i => i.ItemType).ToList();
                break;
            case InventorySortType.Rarity:
                items = items.OrderByDescending(i => i.Rarity).ToList();
                break;
            case InventorySortType.Price:
                items = items.OrderByDescending(i => i.Price).ToList();
                break;
        }

        Debug.Log($"Inventory sorted by {sortType}");
    }

    /// <summary>
    /// Clear all items from inventory
    /// </summary>
    public void ClearInventory()
    {
        items.Clear();
        Debug.Log("Inventory cleared");
    }

    /// <summary>
    /// Get inventory summary
    /// </summary>
    public string GetInventorySummary()
    {
        string summary = $"=== Inventory ({CurrentCapacity}/{maxCapacity}) ===\n";
        summary += $"Gold: {gold}\n\n";

        if (items.Count == 0)
        {
            summary += "Inventory is empty.\n";
            return summary;
        }

        foreach (Item item in items)
        {
            if (item.IsStackable)
            {
                summary += $"- {item.ItemName} x{item.CurrentStackSize} ({item.Rarity})\n";
            }
            else
            {
                summary += $"- {item.ItemName} ({item.Rarity})\n";
            }
        }

        return summary;
    }
}

/// <summary>
/// Enum for inventory sorting options
/// </summary>
public enum InventorySortType
{
    Name,
    Type,
    Rarity,
    Price
}
