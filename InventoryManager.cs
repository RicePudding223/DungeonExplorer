namespace DungeonExplorer
{
    /// <summary>
    /// Manages and handles all interactions related to the player's inventory, including equipping items and using consumables.
    /// </summary>
    public class InventoryManager
    {
        private readonly Player _player;
        private readonly UIManager _uiManager;

        /// <summary>
        /// Initializes a new instance of the InventoryManager class.
        /// </summary>
        /// <param name="player"> The player whose inventory is managed.</param>
        /// <param name="uiManager"> The UI manager used to display inventory-related messages.</param>
        public InventoryManager(Player player, UIManager uiManager)
        {
            _player = player;
            _uiManager = uiManager;
        }

        /// <summary>
        /// Manages the flow for interacting with the player's inventory, including displaying items and allowing the user to select one.
        /// </summary>
        public void HandleInventory()
        {
            if (_player.Inventory.Count == 0)
            {
                // If the inventory is empty, display a message to the player.
                _uiManager.ShowMessage("Your inventory is empty.", true);
                return;
            }

            // Display the inventory and prompt the player to select an item.
            int choice = _uiManager.ShowItemSelection(_player.Inventory, "Select an item to use:");

            // If a valid item is selected, process it accordingly.
            if (choice <= _player.Inventory.Count)
            {
                Item selectedItem = _player.Inventory[choice - 1];
                ProcessSelectedItem(selectedItem);
            }
        }

        /// <summary>
        /// Equips a weapon, either from the inventory or from the ground if found.
        /// </summary>
        /// <param name="weapon"> The weapon to equip.</param>
        /// <param name="fromGround"> A flag indicating if the weapon is being picked up from the ground.</param>
        public void EquipWeapon(Weapon weapon, bool fromGround = false)
        {
            if (fromGround)
            {
                // Prompt the player to confirm if they want to equip the found weapon.
                bool equipNow = _uiManager.ShowYesNoPrompt($"You found {weapon.Name}. Equip it now?");
                if (!equipNow) return;
            }

            // Equip the selected weapon and update the player's stats accordingly.
            _player.Weapon = weapon;
            _player.EquippedWeaponDamage = weapon.Damage;
            _uiManager.ShowMessage($"You equipped {weapon.Name}.", false);
        }

        /// <summary>
        /// Uses a consumable item from the player's inventory.
        /// </summary>
        /// <param name="consumable"> The consumable item to use.</param>
        public void UseConsumable(IConsumable consumable)
        {
            // Apply the effects of the consumable to the player and remove it from the inventory.
            consumable.Use(_player);
            _player.Inventory.Remove(consumable as Item);
            _uiManager.ShowMessage($"You used {((Item)consumable).Name}.", false);
        }

        /// <summary>
        /// Processes the selected item by determining its type and performing the appropriate action (e.g., equip a weapon or use a consumable).
        /// </summary>
        /// <param name="item"> The item to process.</param>
        private void ProcessSelectedItem(Item item)
        {
            // Handle different types of items based on their specific behavior.
            switch (item)
            {
                case Weapon weapon:
                    EquipWeapon(weapon);
                    break;

                case IConsumable consumable:
                    UseConsumable(consumable);
                    break;

                default:
                    // If the item cannot be used directly, show an appropriate message.
                    _uiManager.ShowMessage($"You can't use {item.Name} directly.", true);
                    break;
            }
        }
    }
}
