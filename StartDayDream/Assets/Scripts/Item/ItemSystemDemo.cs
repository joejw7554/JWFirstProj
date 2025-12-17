using UnityEngine;

/// <summary>
/// Demo class showing how to use the item system with sample data
/// </summary>
public class ItemSystemDemo : MonoBehaviour
{
    private Inventory inventory;
    private EquipmentManager equipmentManager;

    void Start()
    {
        Debug.Log("=== Item System Demo Started ===\n");
        
        // Initialize systems
        InitializeSystems();
        
        // Create sample items
        CreateSampleItems();
        
        // Demonstrate functionality
        DemonstrateInventorySystem();
        DemonstrateEquipmentSystem();
        DemonstrateConsumableSystem();
        DemonstrateItemComparison();
        DemonstrateEnhancementSystem();
        
        Debug.Log("\n=== Item System Demo Completed ===");
    }

    private void InitializeSystems()
    {
        inventory = new Inventory(capacity: 50, startingGold: 1000);
        equipmentManager = new EquipmentManager(level: 10);
        Debug.Log("Systems initialized\n");
    }

    private void CreateSampleItems()
    {
        Debug.Log("--- Creating Sample Items ---");
        
        // Consumable items
        ConsumableItem healthPotion = new ConsumableItem(
            "hp_potion_small", "Small Health Potion", 
            "Restores 50 HP", 10,
            ConsumableEffect.HealthRestore, 50, 0f,
            ItemRarity.Common, 99
        );

        ConsumableItem manaPotion = new ConsumableItem(
            "mp_potion_small", "Small Mana Potion",
            "Restores 30 MP", 15,
            ConsumableEffect.ManaRestore, 30, 0f,
            ItemRarity.Common, 99
        );

        ConsumableItem strengthPotion = new ConsumableItem(
            "str_boost", "Potion of Strength",
            "Increases strength for a short time", 50,
            ConsumableEffect.StrengthBoost, 10, 30f,
            ItemRarity.Rare, 10
        );

        // Weapons
        WeaponItem ironSword = new WeaponItem(
            "iron_sword", "Iron Sword",
            "A basic iron sword", 100,
            WeaponType.Sword, 25, 1.2f, 0.05f,
            EquipmentSlot.MainHand, 1, ItemRarity.Common
        );

        WeaponItem steelAxe = new WeaponItem(
            "steel_axe", "Steel Axe",
            "A heavy steel axe", 250,
            WeaponType.Axe, 40, 0.9f, 0.08f,
            EquipmentSlot.TwoHand, 5, ItemRarity.Rare
        );

        WeaponItem legendarySword = new WeaponItem(
            "legend_sword", "Legendary Excalibur",
            "A legendary sword of immense power", 5000,
            WeaponType.Sword, 100, 1.5f, 0.15f,
            EquipmentSlot.MainHand, 20, ItemRarity.Legendary
        );

        // Armor
        ArmorItem leatherHelmet = new ArmorItem(
            "leather_helm", "Leather Helmet",
            "Basic leather headgear", 50,
            ArmorType.Leather, EquipmentSlot.Head, 10, 5, 20,
            1, ItemRarity.Common
        );

        ArmorItem plateChest = new ArmorItem(
            "plate_chest", "Plate Chestplate",
            "Heavy plate armor for the chest", 500,
            ArmorType.Plate, EquipmentSlot.Chest, 50, 20, 100,
            10, ItemRarity.Epic
        );

        ArmorItem clothBoots = new ArmorItem(
            "cloth_boots", "Cloth Boots",
            "Light cloth footwear", 30,
            ArmorType.Cloth, EquipmentSlot.Feet, 5, 15, 10,
            1, ItemRarity.Common
        );

        // Misc items
        Item goldCoin = new Item(
            "gold_coin", "Gold Coin",
            ItemType.Misc, "Shiny gold coins", 1,
            ItemRarity.Common, true, 999
        );

        // Add items to inventory
        inventory.AddItem(healthPotion.Clone());
        inventory.AddItem(healthPotion.Clone());
        inventory.AddItem(healthPotion.Clone());
        inventory.AddItem(manaPotion.Clone());
        inventory.AddItem(manaPotion.Clone());
        inventory.AddItem(strengthPotion.Clone());
        inventory.AddItem(ironSword.Clone());
        inventory.AddItem(steelAxe.Clone());
        inventory.AddItem(legendarySword.Clone());
        inventory.AddItem(leatherHelmet.Clone());
        inventory.AddItem(plateChest.Clone());
        inventory.AddItem(clothBoots.Clone());
        
        Item goldStack = goldCoin.Clone();
        goldStack.AddToStack(99);
        inventory.AddItem(goldStack);

        Debug.Log("Sample items created and added to inventory\n");
    }

    private void DemonstrateInventorySystem()
    {
        Debug.Log("--- Inventory System Demo ---");
        Debug.Log(inventory.GetInventorySummary());
        
        // Sort inventory
        inventory.SortInventory(InventorySortType.Rarity);
        Debug.Log("Inventory sorted by rarity");
        
        // Check for specific item
        bool hasPotion = inventory.HasItem("hp_potion_small", 3);
        Debug.Log($"Has 3+ health potions: {hasPotion}");
        
        // Gold operations
        inventory.AddGold(500);
        inventory.RemoveGold(200);
        
        Debug.Log("");
    }

    private void DemonstrateEquipmentSystem()
    {
        Debug.Log("--- Equipment System Demo ---");
        
        // Get weapons from inventory
        WeaponItem ironSword = inventory.GetItemById("iron_sword") as WeaponItem;
        WeaponItem steelAxe = inventory.GetItemById("steel_axe") as WeaponItem;
        
        // Get armor from inventory
        ArmorItem helmet = inventory.GetItemById("leather_helm") as ArmorItem;
        ArmorItem chest = inventory.GetItemById("plate_chest") as ArmorItem;
        ArmorItem boots = inventory.GetItemById("cloth_boots") as ArmorItem;

        // Equip items
        if (ironSword != null) equipmentManager.EquipItem(ironSword);
        if (helmet != null) equipmentManager.EquipItem(helmet);
        if (chest != null) equipmentManager.EquipItem(chest);
        if (boots != null) equipmentManager.EquipItem(boots);
        
        Debug.Log(equipmentManager.GetEquipmentSummary());
        
        // Try to equip two-handed weapon
        if (steelAxe != null)
        {
            Debug.Log("Equipping two-handed axe...");
            equipmentManager.EquipItem(steelAxe);
            Debug.Log(equipmentManager.GetEquipmentSummary());
        }
        
        Debug.Log("");
    }

    private void DemonstrateConsumableSystem()
    {
        Debug.Log("--- Consumable System Demo ---");
        
        ConsumableItem healthPotion = inventory.GetItemById("hp_potion_small") as ConsumableItem;
        
        if (healthPotion != null)
        {
            Debug.Log(healthPotion.GetItemInfo());
            Debug.Log("Consuming health potion...");
            healthPotion.Consume();
            Debug.Log($"Remaining stack: {healthPotion.CurrentStackSize}");
        }

        ConsumableItem strengthPotion = inventory.GetItemById("str_boost") as ConsumableItem;
        
        if (strengthPotion != null)
        {
            Debug.Log(strengthPotion.GetItemInfo());
            Debug.Log("Consuming strength potion...");
            strengthPotion.Consume();
        }
        
        Debug.Log("");
    }

    private void DemonstrateItemComparison()
    {
        Debug.Log("--- Item Comparison Demo ---");
        
        WeaponItem ironSword = inventory.GetItemById("iron_sword") as WeaponItem;
        WeaponItem legendSword = inventory.GetItemById("legend_sword") as WeaponItem;
        
        if (ironSword != null && legendSword != null)
        {
            Debug.Log(ironSword.CompareWith(legendSword));
        }

        ArmorItem helmet = inventory.GetItemById("leather_helm") as ArmorItem;
        ArmorItem chest = inventory.GetItemById("plate_chest") as ArmorItem;
        
        if (helmet != null && chest != null)
        {
            Debug.Log(helmet.CompareWith(chest));
        }
        
        Debug.Log("");
    }

    private void DemonstrateEnhancementSystem()
    {
        Debug.Log("--- Enhancement System Demo ---");
        
        WeaponItem legendSword = inventory.GetItemById("legend_sword") as WeaponItem;
        
        if (legendSword != null)
        {
            Debug.Log($"Before enhancement: {legendSword.ItemName}");
            Debug.Log(legendSword.GetItemInfo());
            
            // Enhance the weapon multiple times
            for (int i = 0; i < 3; i++)
            {
                legendSword.Enhance();
            }
            
            Debug.Log($"\nAfter enhancement: {legendSword.ItemName}");
            Debug.Log(legendSword.GetItemInfo());
        }

        ArmorItem chest = inventory.GetItemById("plate_chest") as ArmorItem;
        
        if (chest != null)
        {
            Debug.Log($"Before enhancement: {chest.ItemName}");
            Debug.Log(chest.GetItemInfo());
            
            // Enhance the armor multiple times
            for (int i = 0; i < 5; i++)
            {
                chest.Enhance();
            }
            
            Debug.Log($"\nAfter enhancement: {chest.ItemName}");
            Debug.Log(chest.GetItemInfo());
        }
        
        Debug.Log("");
    }
}
