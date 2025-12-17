# Item System Documentation

This is a comprehensive item system implementation for Unity games, featuring inventory management, equipment system, and various item types.

## Features

### Core Features
✅ Item Type Enum (Consumable, Weapon, Armor, Misc)
✅ Item Rarity System (Common, Rare, Epic, Legendary)
✅ Base Item class with properties (name, type, description, price)
✅ Item stacking functionality
✅ Consumable items with various effects
✅ Weapon and Armor equipment system
✅ Inventory management (add, remove, sort, search)
✅ Equipment manager for equipped items
✅ Item comparison system
✅ Item enhancement/upgrade system
✅ Sample item data and demo

## Architecture

### Class Hierarchy
```
Item (Base Class)
├── ConsumableItem (Potions, Food)
└── EquippableItem (Abstract)
    ├── WeaponItem (Swords, Axes, Bows, etc.)
    └── ArmorItem (Helmets, Chestplates, Boots, etc.)
```

### Manager Classes
- **Inventory**: Manages player items, gold, stacking, and organization
- **EquipmentManager**: Handles equipped items, slots, and stat calculation

## Usage Examples

### Creating Items

#### Consumable Item
```csharp
ConsumableItem healthPotion = new ConsumableItem(
    "hp_potion", "Health Potion",
    "Restores 50 HP", 10,
    ConsumableEffect.HealthRestore, 50, 0f,
    ItemRarity.Common, 99
);
```

#### Weapon Item
```csharp
WeaponItem sword = new WeaponItem(
    "iron_sword", "Iron Sword",
    "A basic iron sword", 100,
    WeaponType.Sword, 25, 1.2f, 0.05f,
    EquipmentSlot.MainHand, 1, ItemRarity.Common
);
```

#### Armor Item
```csharp
ArmorItem helmet = new ArmorItem(
    "leather_helm", "Leather Helmet",
    "Basic leather headgear", 50,
    ArmorType.Leather, EquipmentSlot.Head, 10, 5, 20,
    1, ItemRarity.Common
);
```

### Using Inventory System

```csharp
// Create inventory
Inventory inventory = new Inventory(capacity: 50, startingGold: 1000);

// Add items
inventory.AddItem(healthPotion);
inventory.AddItem(sword);

// Remove items
inventory.RemoveItem(healthPotion);
inventory.RemoveItemById("iron_sword");

// Check for items
bool hasItem = inventory.HasItem("hp_potion", 5);

// Sort inventory
inventory.SortInventory(InventorySortType.Rarity);

// Get inventory summary
string summary = inventory.GetInventorySummary();

// Gold management
inventory.AddGold(500);
inventory.RemoveGold(200);
```

### Using Equipment System

```csharp
// Create equipment manager
EquipmentManager equipmentManager = new EquipmentManager(level: 10);

// Equip items
equipmentManager.EquipItem(sword);
equipmentManager.EquipItem(helmet);

// Unequip items
EquippableItem unequipped = equipmentManager.UnequipSlot(EquipmentSlot.MainHand);

// Get equipped item
EquippableItem equipped = equipmentManager.GetEquippedItem(EquipmentSlot.Head);

// Get total stats
EquipmentStats stats = equipmentManager.GetTotalStats();

// Get equipment summary
string summary = equipmentManager.GetEquipmentSummary();
```

### Using Consumable Items

```csharp
ConsumableItem potion = inventory.GetItemById("hp_potion") as ConsumableItem;

// Consume the item
if (potion != null && potion.Consume())
{
    // Item consumed successfully
    Debug.Log($"Consumed {potion.ItemName}");
}
```

### Item Enhancement

```csharp
WeaponItem weapon = inventory.GetItemById("iron_sword") as WeaponItem;

// Enhance the weapon
if (weapon != null && weapon.Enhance())
{
    Debug.Log($"Enhanced to +{weapon.EnhancementLevel}");
    Debug.Log($"New attack power: {weapon.TotalAttackPower}");
}
```

### Item Comparison

```csharp
WeaponItem sword1 = inventory.GetItemById("iron_sword") as WeaponItem;
WeaponItem sword2 = inventory.GetItemById("steel_sword") as WeaponItem;

// Compare weapons
if (sword1 != null && sword2 != null)
{
    string comparison = sword1.CompareWith(sword2);
    Debug.Log(comparison);
}

ArmorItem armor1 = inventory.GetItemById("leather_helm") as ArmorItem;
ArmorItem armor2 = inventory.GetItemById("iron_helm") as ArmorItem;

// Compare armor
if (armor1 != null && armor2 != null)
{
    string comparison = armor1.CompareWith(armor2);
    Debug.Log(comparison);
}
```

## Enums

### ItemType
- `Consumable` - Items that can be consumed (potions, food)
- `Weapon` - Weapons that can be equipped
- `Armor` - Armor pieces that can be equipped
- `Misc` - Miscellaneous items

### ItemRarity
- `Common` - Basic items
- `Rare` - Uncommon items
- `Epic` - Powerful items
- `Legendary` - Extremely rare and powerful items

### ConsumableEffect
- `HealthRestore` - Restores health points
- `ManaRestore` - Restores mana points
- `StrengthBoost` - Temporarily increases strength
- `DefenseBoost` - Temporarily increases defense
- `SpeedBoost` - Temporarily increases speed

### WeaponType
- `Sword` - One-handed sword
- `Axe` - One or two-handed axe
- `Bow` - Ranged weapon
- `Staff` - Magic weapon
- `Dagger` - Fast weapon
- `Spear` - Long reach weapon

### ArmorType
- `Cloth` - Light armor, good magic resistance
- `Leather` - Medium armor, balanced stats
- `Mail` - Heavy armor, good defense
- `Plate` - Very heavy armor, best defense

### EquipmentSlot
- `Head` - Helmet slot
- `Chest` - Chestplate slot
- `Legs` - Leg armor slot
- `Feet` - Boots slot
- `Hands` - Gloves slot
- `MainHand` - Main weapon slot
- `OffHand` - Shield or secondary weapon
- `TwoHand` - Two-handed weapon slot

## Demo

To see the item system in action:

1. Attach the `ItemSystemDemo` component to any GameObject in your scene
2. Run the scene
3. Check the Console for detailed output showing all system features

The demo includes:
- Creating various sample items
- Inventory management operations
- Equipment system functionality
- Consumable item usage
- Item comparison
- Item enhancement

## Design Principles

### Object-Oriented Design
- Clear inheritance hierarchy
- Proper encapsulation with private fields and public properties
- Abstract base classes for shared functionality
- Virtual methods for customizable behavior

### Extensibility
- Easy to add new item types by extending base classes
- New consumable effects can be added to the enum
- New equipment slots can be added easily
- Enhancement system is generic and works for all equipment

### Exception Handling
- Null checks throughout the system
- Validation of player level requirements
- Inventory capacity checks
- Stack size validation
- Proper warning messages via Debug.Log

### Unity Integration
- Uses `[Serializable]` attribute for Unity serialization
- Supports Unity's Sprite for item icons
- MonoBehaviour integration via demo class
- Compatible with Unity's inspector

## Future Enhancements

Potential additions to the system:
- Item crafting/combination system
- JSON-based item database loading
- Item quality/durability system
- Set bonuses for equipment
- Item sockets and gems
- Trading system
- Quest items
- Key items (non-droppable)
- Item tooltips and UI integration

## License

This item system is provided as-is for use in your Unity projects.
