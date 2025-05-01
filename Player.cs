using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents the player character in the dungeon exploration game.
    /// The Player class manages the player's properties, actions, and interactions with other game elements.
    /// </summary>
    public class Player : Creature, IHealable
    {
        // Player properties and actions management
        public Weapon Weapon { get; set; }  // Gets or sets the weapon currently equipped by the player
        public int EquippedWeaponDamage { get; set; }  // Gets or sets the damage value of the equipped weapon
        public List<Item> Inventory { get; set; }  // Gets or sets the list of items in the player's inventory
        public int PlayerX { get; set; }  // Gets or sets the player's current X-coordinate in the game world
        public int PlayerY { get; set; }  // Gets or sets the player's current Y-coordinate in the game world
        public Room CurrentRoom { get; set; }  // Gets or sets the room that the player is currently in
        public int Score { get; set; }  // Gets or sets the player's score in the game

        // Private manager instances for handling game functionalities
        private RoomManager _roomManager;
        private InventoryManager _inventoryManager;
        private CombatManager _combatManager;
        private UIManager _uiManager;

        /// <summary>
        /// Initializes a new instance of the Player class with specified properties.
        /// </summary>
        /// <param name="name">The name of the player.</param>
        /// <param name="startX">The starting X-coordinate of the player.</param>
        /// <param name="startY">The starting Y-coordinate of the player.</param>
        /// <param name="currentRoom">The room the player begins in.</param>
        public Player(string name, int startX, int startY, Room currentRoom)
            : base(name, 100, 10)  // Calls the base Creature class constructor
        {
            Inventory = new List<Item>(); 
            PlayerX = startX;  
            PlayerY = startY;  
            CurrentRoom = currentRoom; 
            Weapon = null;
            Score = 0;

            // Initializes the various manager objects to handle specific game functionalities
            _uiManager = new UIManager();
            _roomManager = new RoomManager();
            _inventoryManager = new InventoryManager(this, _uiManager);
            _combatManager = new CombatManager(this, _uiManager);
        }

        /// <summary>
        /// Adds an item to the player's inventory. If the item is a weapon, it equips the weapon.
        /// </summary>
        /// <param name="item">The item to be added to the player's inventory.</param>
        public void AddItem(Item item)
        {
            // If the item is a weapon, equip it using the InventoryManager
            if (item is Weapon weapon)
            {
                _inventoryManager.EquipWeapon(weapon, true);
            }
            // Add the item to the player's inventory list
            Inventory.Add(item);

            // Sort the inventory
            Inventory = _inventoryManager.GetSortedInventory();
        }

        /// <summary>
        /// Allows the player to use an item from their inventory.
        /// This method prompts the inventory manager to handle the item usage.
        /// </summary>
        public void UseItem()
        {
            // Calls the inventory manager to handle the inventory interface
            _inventoryManager.HandleInventory();
        }

        /// <summary>
        /// Allows the player to pick up an item from the current room.
        /// The player selects an item from the room's list of items, which is then added to their inventory.
        /// </summary>
        public void PickUpItem()
        {
            // Check if there are items in the current room
            if (CurrentRoom.Items.Count == 0)
            {
                _uiManager.ShowMessage("No items to pick up.");
                return;
            }

            // Check if there are enemies in the room
            if (CurrentRoom.Enemies.Count > 0)
            {
                // If there are enemies, there's a chance the enemy will do a sneak attack
                Random random = new Random();
                int chance = random.Next(1, 11);
                if (chance < 4)
                {
                    _uiManager.ShowMessage("You try to pick up an item, but the enemies attack you!");
                    _uiManager.WaitForInput();
                    _combatManager.FightEnemy(true);
                    return;
                }
            }

            // Prompts the player to select an item to pick up
            int choice = _uiManager.ShowItemSelection(
                CurrentRoom.Items,
                "Choose an item to pick up:"
            );

            // If the player selects a valid item, add it to the inventory and remove it from the room
            if (choice <= CurrentRoom.Items.Count)
            {
                AddItem(CurrentRoom.Items[choice - 1]);
                CurrentRoom.Items.RemoveAt(choice - 1);  // Remove the selected item from the room
            }
        }

        /// <summary>
        /// Heals the player by using a potion from their inventory.
        /// </summary>
        /// <param name="potion">The potion being used to heal the player.</param>
        public void Heal(Potion potion)
        {
            // Calls the inventory manager to use the consumable potion
            _inventoryManager.UseConsumable(potion);
        }

        /// <summary>
        /// Handles the player's death event.
        /// </summary>
        /// <param name="killer"> The creature object that is killing the player.</param>
        public override void OnDeath(Creature killer)
        {
            base.OnDeath(killer);
            _uiManager.ShowMessage($"GAME OVER", true);
            Game.IsGameOver = true;
        }
    }
}
