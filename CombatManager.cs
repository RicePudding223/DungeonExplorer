using System;
using System.Collections.Generic;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// Class to manage the combat system.
    /// </summary>
    public class CombatManager
    {
        private Player player;

        /// <summary>
        /// Initialises a new instance of the CombatManager class.
        /// </summary>
        /// <param name="player"> The player involved in the combat.</param>
        public CombatManager(Player player)
        {
            this.player = player;
        }

        /// <summary>
        /// Method to start a fight with an enemy in the current room.
        /// </summary>
        public void FightEnemy()
        {
            try
            {
                // Checks if there are enemies in the room then asks the player to choose an enemy to fight
                if (player.CurrentRoom.Enemies.Count > 0)
                {
                    Console.WriteLine("\nYou decide to fight an enemy, which enemy do you want to fight?");
                    int end = player.CurrentRoom.Enemies.Count;
                    for (int i = 0; i < end; i++)
                    {
                        Monster enemy = player.CurrentRoom.Enemies[i];
                        Console.WriteLine($"{i + 1}. {enemy.Name}");
                    }
                    Console.Write($"{end + 1}. Cancel\n: ");
                    string choiceS = Console.ReadLine();
                    int.TryParse(choiceS, out int choice);

                    // If the player chooses a valid enemy, start the fight
                    if (choice > 0 && choice != (end + 1) && choice <= end)
                    {
                        Thread.Sleep(100);
                        Console.Clear();
                        Monster enemy = player.CurrentRoom.Enemies[choice - 1];
                        int round = 1;

                        // Main fight loop
                        while (true)
                        {
                            round++;
                            Console.WriteLine($"\n===== Fighting {enemy.Name} =====\n");
                            Console.WriteLine($"{player.Name}'s Health: {player.Health}/{player.MaxHealth}");
                            Console.WriteLine($"{enemy.Name} Health: {enemy.Health}/{enemy.MaxHealth}");
                            Console.WriteLine("\nWhat do you want to do?");
                            Console.WriteLine("1. Attack\t\t 2. Use item\t\t 3. Run");
                            Console.Write(": ");
                            string choice2 = Console.ReadLine();

                            switch (choice2)
                            {
                                case "1":
                                    Attack(enemy, choice, round);
                                    if (enemy.CheckAlive())
                                    {
                                        return;
                                    }
                                    break;

                                case "2":
                                    player.UseItem();
                                    break;

                                case "3":
                                    Console.WriteLine("\nYou run away from the fight.\n");
                                    Thread.Sleep(1500);
                                    return;

                                default:
                                    Console.WriteLine("\nInvalid choice. Try again.\n");
                                    Thread.Sleep(600);
                                    break;
                            }
                            Thread.Sleep(1500);
                            Console.Clear();
                        }
                    }
                    // If the player chooses to cancel, return to the main game loop
                    if (choice == (end + 1))
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input please try again.");
                        Thread.Sleep(600);
                        FightEnemy();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Method to attack an enemy during the fight.
        /// </summary>
        /// <param name="enemy"> The enemy to attack.</param>
        /// <param name="choice"> The index of the enemy in the room's enemy list.</param>
        /// <param name="round"> The current ronud of the fight.</param>
        public void Attack(Monster enemy, int choice, int round)
        {
            // Random multiplier to add some randomness to the damage
            Random random = new Random();
            double multiplier = Math.Round(random.NextDouble() + 1, 1);

            // Calculate the player's damage and subtract it from the enemy's health
            int playerDamage = (int)(player.EquippedWeaponDamage + (player.Strength * multiplier));
            enemy.TakeDamage(playerDamage);
            Console.WriteLine($"\nYou deal {playerDamage} damage to {enemy.Name}.\n");

            // Check if the enemy has been defeated
            if (!enemy.CheckAlive())
            {
                Console.WriteLine($"You have defeated the {enemy.Name}.\n");
                player.CurrentRoom.Enemies.RemoveAt(choice - 1);
                Thread.Sleep(1000);
                return;
            }

            // Calculate the enemy's damage and subtract it from the player's health if they can attack this round
            if (round % enemy.Speed == 0)
            {
                player.Health -= enemy.Strength;
                Console.WriteLine($"{enemy.Name} deals {enemy.Strength} damage to you.\n");
            }

            // Check if the player has been defeated
            if (player.Health <= 0)
            {
                Console.WriteLine("You have died.\n");
                Game.IsGameOver = true;
                Thread.Sleep(1000);
                return;
            }
        }
    }
}
