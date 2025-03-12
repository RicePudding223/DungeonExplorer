using System;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// Class to manage the player's inventory.
    /// </summary>
    public class InventoryManager
    {
        private Player player;

        /// <summary>
        /// Initialises a new instance of the InventoryManager class.
        /// </summary>
        /// <param name="player"> The player involved in the combat.</param>
        public InventoryManager(Player player)
        {
            this.player = player;
        }

        /// <summary>
        /// Method to equip a weapon.
        /// </summary>
        /// <param name="item"> The weapon to equip.</param>
        public void EquipWeapon(string item)
        {
            try
            {
                // Prints out question, allows user to enter decision
                Console.WriteLine("\nYou picked up a weapon, do you want to equip it?");
                Console.Write("1. Yes\t\t2. No\n: ");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    player.EquippedWeaponDamage = GameData.GetWeapons()[item];
                    player.Weapon = item;
                    Console.WriteLine($"\nYou equipped the {item}\n");
                    Thread.Sleep(1500);
                }
                else if (choice == "2")
                {
                    Console.WriteLine("\nYou decide to keep the weapon in your inventory.\n");
                    Thread.Sleep(1500);
                }
                else
                {
                    Console.WriteLine("\nInvalid choice. Try again.\n");
                    Thread.Sleep(600);
                    EquipWeapon(item);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Method to allow player to use an item from their inventory.
        /// </summary>
        public void UseItem()
        {
            while (true)
            {
                try
                {
                    // If there are items in inventory, display them and allow user to choose
                    if (player.Inventory.Count > 0)
                    {
                        Console.WriteLine("\nYou decide to use an item, which item do you want to use?");
                        for (int i = 0; i < player.Inventory.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {player.Inventory[i]}");
                        }

                        Console.Write($"{player.Inventory.Count + 1}. Exit \n: ");
                        string choiceS = Console.ReadLine();
                        int.TryParse(choiceS, out int choice);

                        // If the choice is valid, check if it's a weapon or a potion
                        if (choice > 0 && choice <= player.Inventory.Count)
                        {
                            if (GameData.GetWeapons().ContainsKey(player.Inventory[choice - 1]))
                            {
                                EquipWeapon(player.Inventory[choice - 1]);
                                break;
                            }

                            if (GameData.GetPotions().ContainsKey(player.Inventory[choice - 1]))
                            {
                                player.Health += GameData.GetPotions()[player.Inventory[choice - 1]];
                                Console.WriteLine($"\nYou use a potion and gain {GameData.GetPotions()[player.Inventory[choice - 1]]} health.\n");
                                // Stops the player from going over max health
                                if (player.Health > player.MaxHealth)
                                {
                                    player.Health = player.MaxHealth;
                                }
                                player.Inventory.RemoveAt(choice - 1);
                                Thread.Sleep(1500);
                                break;
                            }
                        }
                        // Breaks out of the loop if user decides to exit
                        else if (choice == player.Inventory.Count + 1)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine("Invalid input please try again.");
                            Thread.Sleep(600);
                        }
                    }
                    else
                    {
                        Console.WriteLine("\nYou have no items to use.\n");
                        Thread.Sleep(1500);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }
    }
}
