using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// Handles all user interface rendering and input collection.
    /// </summary>
    public class UIManager
    {
        // Colours
        private const ConsoleColor HighlightColor = ConsoleColor.Yellow;
        private const ConsoleColor DangerColor = ConsoleColor.Red;

        /// <summary>
        /// Displays the main game menu and returns the player's choice.
        /// </summary>
        public int ShowMainMenu()
        {
            Console.WriteLine("\nWhat do you want to do?");
            Console.WriteLine("1. Move to another room");
            Console.WriteLine("2. Fight enemies");
            Console.WriteLine("3. Pick up items");
            Console.WriteLine("4. Use items");
            Console.WriteLine("5. View player stats");
            Console.WriteLine("6. Display map");
            Console.WriteLine("7. Quit");

            return GetValidChoice(1, 7);
        }

        /// <summary>
        /// Displays the current room description and contents.
        /// </summary>
        /// <param name="room"> The current room the player is in.</param>
        public void DisplayRoom(Room room)
        {
            Console.WriteLine($"\n{room.RoomID}: {room.Description}\n");

            if (room.Items.Count > 0)
            {
                Console.WriteLine("Items here: " + string.Join(", ", room.Items.Select(item => item.Name)) + "\n");
            }

            if (room.Enemies.Count > 0)
            {
                Console.Write("Enemies here: ");
                for (int i = 0; i < room.Enemies.Count; i++)
                {
                    Console.Write(room.Enemies[i].Name);
                    if (i < room.Enemies.Count - 1) Console.Write(", ");
                }
                Console.WriteLine("\n");
            }

            Console.WriteLine("Exits: " + string.Join(", ", room.Exits) + "\n");
        }

        /// <summary>
        /// Displays player stats (health, inventory, equipped weapon).
        /// </summary>
        /// <param name="player"> The player instance.</param>
        public void DisplayPlayerInfo(Player player)
        {
            Console.WriteLine($"\nName: {player.Name}");
            Console.WriteLine($"Health: {player.CurrentHealth}/{player.MaxHealth}");
            Console.WriteLine($"Inventory: {string.Join(", ", player.Inventory.Select(item => item.Name))}");
            Console.WriteLine($"Equipped Weapon: {(player.Weapon != null ? player.Weapon.Name : "None")}\n");
        }

        /// <summary>
        /// Displays the dungeon map with player position.
        /// </summary>
        /// <param name="grid"> The grid of rooms that represent the map.</param>
        /// <param name="player"> The player instance.</param>
        public void DisplayMap(Room[,] grid, Player player)
        {
            Console.WriteLine();
            for (int y = 0; y < grid.GetLength(1); y++)
            {
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    if (x == player.PlayerX && y == player.PlayerY)
                    {
                        Console.Write("P ");
                    }
                    else if (grid[x, y] != null)
                    {
                        Console.Write("X ");
                    }
                    else
                    {
                        Console.Write("- ");
                    }
                }
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        /// <summary>
        /// Shows enemy selection menu.
        /// </summary>
        /// <param name="enemies"> A list of ememies.</param>
        /// <returns> An integer choice for which enemy the player wants.</returns>
        public int ShowEnemySelection(List<Monster> enemies)
        {
            Console.WriteLine("\nChoose an enemy to fight:");
            for (int i = 0; i < enemies.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {enemies[i].Name} (HP: {enemies[i].CurrentHealth})");
            }
            Console.WriteLine($"{enemies.Count + 1}. Cancel");

            return GetValidChoice(1, enemies.Count + 1);
        }

        /// <summary>
        /// Displays current combat status.
        /// </summary>
        /// <param name="player"> The player instance.</param>
        /// <param name="enemy"> The enemy the player is fighting.</param>
        /// <param name="round"> The round number the fight is on.</param>
        public void ShowCombatStatus(Player player, Monster enemy, int round)
        {
            Console.WriteLine($"\n===== Round {round} =====");
        }

        /// <summary>
        /// Displays combat interface and gets player's action choice.
        /// </summary>
        /// <param name="player"> The player instance.</param>
        /// <param name="enemy"> The enemy the player is fighting.</param>
        /// <returns> An integer choice on what the player wants to do.</returns>
        public int ShowCombatMenu(Player player, Monster enemy)
        {
            Console.WriteLine($"\n===== Fighting {enemy.Name} =====");
            Console.WriteLine($"{player.Name}'s Health: {player.CurrentHealth}/{player.MaxHealth}");
            Console.WriteLine($"{enemy.Name}'s Health: {enemy.CurrentHealth}/{enemy.MaxHealth}\n");

            Console.WriteLine("1. Attack");
            Console.WriteLine("2. Use item");
            Console.WriteLine("3. Run");

            return GetValidChoice(1, 3);
        }

        /// <summary>
        /// Prompts player to select an item from a list.
        /// </summary>
        /// <param name="items"> A list of items.</param>
        /// <param name="prompt"> A string telling the user what to do.</param>
        /// <returns> An integer choice on what item the user wants.</returns>
        public int ShowItemSelection(List<Item> items, string prompt)
        {
            Console.WriteLine($"\n{prompt}");
            for (int i = 0; i < items.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {items[i].Name}");
            }
            Console.WriteLine($"{items.Count + 1}. Cancel");

            return GetValidChoice(1, items.Count + 1);
        }

        /// <summary>
        /// Shows a yes/no prompt.
        /// </summary>
        /// <param name="question"> The question the user is answering.</param>
        /// <returns></returns>
        public bool ShowYesNoPrompt(string question)
        {
            Console.WriteLine($"\n{question} (Y/N)");
            while (true)
            {
                Console.Write(": ");
                string input = Console.ReadLine()?.Trim().ToUpper();

                if (input == "Y") return true;
                if (input == "N") return false;

                ShowMessage("Please enter Y or N.", true);
            }
        }

        /// <summary>
        /// Displays a message in a highlighted colour.
        /// </summary>
        /// <param name="message"> The message being highlighted.</param>
        /// <param name="isWarning"> A flag determining the colour.</param>
        public void ShowMessage(string message, bool isWarning = false)
        {
            Console.ForegroundColor = isWarning ? DangerColor : HighlightColor;
            Console.WriteLine($"\n{message}\n");
            Console.ResetColor();
        }

        /// <summary>
        /// Gets a valid integer choice from the player within a specified range.
        /// </summary>
        /// <param name="min"> The minimum value.</param>
        /// <param name="max"> The maximum value.</param>
        /// <returns></returns>
        public int GetValidChoice(int min, int max)
        {
            while (true)
            {
                Console.Write(": ");
                if (int.TryParse(Console.ReadLine(), out int choice) && choice >= min && choice <= max)
                {
                    return choice;
                }
                ShowMessage($"Invalid input. Please enter a number between {min}-{max}.", true);
            }
        }

        /// <summary>
        /// Clears the console screen with a small delay for smooth transitions.
        /// </summary>
        public void ClearScreen()
        {
            Thread.Sleep(200);
            Console.Clear();
        }

        /// <summary>
        /// Prompts the player to enter their name.
        /// </summary>
        public string GetPlayerName()
        {
            string name;
            do
            {
                Console.Write("Enter your name: ");
                name = Console.ReadLine()?.Trim();
                if (string.IsNullOrEmpty(name))
                {
                    ShowMessage("Please enter a valid name.", true);
                }
            } while (string.IsNullOrEmpty(name));

            return name;
        }

        /// <summary>
        /// Waits for player to press any key before continuing
        /// </summary>
        public void WaitForInput()
        {
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(true);
            ClearScreen();
        }
    }
}