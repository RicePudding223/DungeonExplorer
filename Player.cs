using System;
using System.Collections.Generic;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents the player class.
    /// It handles the player's properties and actions.
    /// </summary>
    public class Player
    {
        private RoomManager roomManager;
        private InventoryManager inventoryManager;
        private CombatManager combatManager;

        /// <summary>
        /// Gets and sets for the player's properties
        /// </summary>
        public string Name { get; set; }
        public int MaxHealth { get; set; }
        public int Health { get; set; }
        public int Strength { get; set; }
        public string Weapon { get; set; }
        public int EquippedWeaponDamage { get; set; }
        public List<string> Inventory { get; set; }
        public int PlayerX { get; set; }
        public int PlayerY { get; set; }
        public Room CurrentRoom { get; set; }
        public bool lastRoom = false;

        /// <summary>
        /// Initialises a new instance of the Player class.
        /// </summary>
        /// <param name="name"> The name of the player.</param>
        /// <param name="startX"> The starting X-coordinate of the player.</param>
        /// <param name="startY"> The starting Y-coordinate of the player.</param>
        /// <param name="currentRoom"> The starting room of the player.</param>
        public Player(string name, int startX, int startY, Room currentRoom)
        {
            roomManager = new RoomManager();
            inventoryManager = new InventoryManager(this);
            combatManager = new CombatManager(this);
            Name = name;
            MaxHealth = 100;
            Health = MaxHealth;
            Strength = 10;
            Inventory = new List<string>();
            PlayerX = startX;
            PlayerY = startY;
            CurrentRoom = currentRoom;
            Weapon = null;
        }

        /// <summary>
        /// Method to add an item to the player's inventory.
        /// </summary>
        /// <param name="item"> The item being added to the inventory.</param>
        public void AddItem(string item)
        {
            if (GameData.GetWeapons().ContainsKey(item))
            {
                inventoryManager.EquipWeapon(item);
            }
            Inventory.Add(item);
        }

        /// <summary>
        /// Method to display player information, including name, health, inventory, and equipped weapon.
        /// </summary>
        public void DisplayInfo()
        {
            Console.WriteLine($"\nName: {Name}");
            Console.WriteLine($"Health: {Health}/{MaxHealth}");
            Console.WriteLine($"Inventory: {string.Join(", ", Inventory)}");
            Console.WriteLine($"Equipped Weapon: {Weapon}\n");
        }

        /// <summary>
        /// Method to allow player to use an item from their inventory.
        /// </summary>
        public void UseItem()
        {
            inventoryManager.UseItem();
        }

        /// <summary>
        /// Method to move to another room on the grid.
        /// </summary>
        /// <param name="Grid"> The grid that represents the dungeon.</param>
        /// <param name="RoomCount"> The current count of rooms explored.</param>
        public void MoveToRoom(Room[,] Grid, int RoomCount)
        {
            // Loop to get a valid answer
            while (true)
            {
                try
                {
                    // Checks if there are enemies in the room
                    if (CurrentRoom.Enemies.Count > 0)
                    {
                        Console.WriteLine("\nYou must defeat all enemies in the room before moving to another room.\n");
                        Thread.Sleep(1500);
                        return;
                    }

                    // Prints out the exits and allows user to choose
                    Console.WriteLine("\nYou decide to move to another room, where do you want to go?");
                    int end = CurrentRoom.Exits.Count;
                    for (int i = 0; i < end; i++)
                    {
                        Console.WriteLine($"{i + 1}. {CurrentRoom.Exits[i]}");
                    }
                    Console.Write($"{end + 1}. Cancel\n: ");
                    string choiceS = Console.ReadLine();
                    int.TryParse(choiceS, out int choice);

                    // Make sure the choice is valid and move the player
                    if (choice > 0 && choice != (end + 1) && choice <= end)
                    {
                        RoomCount++;
                        string roomID = "Room " + RoomCount;
                        string direction = CurrentRoom.Exits[choice - 1];

                        MovePlayer(direction);
                        if (Game.IsGameOver)
                        {
                            return;
                        }

                        // Makes a last room check
                        if (RoomCount == 10)
                        {
                            lastRoom = true;
                        }

                        // Checks if the room the player is moving to already exists
                        if (Grid[PlayerX, PlayerY] != null)
                        {
                            CurrentRoom = Grid[PlayerX, PlayerY];
                        }
                        // Creates a new room if it doesn't exist
                        else
                        {
                            Room newRoom = roomManager.CreateNewRoom(roomID, direction, RoomCount, Grid, PlayerX, PlayerY, lastRoom);
                            Grid[PlayerX, PlayerY] = newRoom;
                            CurrentRoom = newRoom;

                            Console.WriteLine($"\nYou move {direction} into {roomID}.\n");
                            Thread.Sleep(600);
                            break;
                        }
                    }
                    // Breaks out of the loop if user decides to cancel
                    else if (choice == (end + 1))
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input please try again.");
                        Thread.Sleep(600);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"An error occurred: {ex.Message}");
                }
            }
        }

        /// <summary>
        /// Method to change the player's coordinate on the grid.
        /// </summary>
        /// <param name="direction"> The direction the player decided to move in.</param>
        private void MovePlayer(string direction)
        {
            switch (direction)
            {
                case "North":
                    PlayerY--;
                    break;
                case "South":
                    PlayerY++;
                    break;
                case "East":
                    PlayerX++;
                    break;
                case "West":
                    PlayerX--;
                    break;
                case "Exit":
                    Console.WriteLine("\nYou have reached the exit, congratulations!\n");
                    Game.IsGameOver = true;
                    break;
            }
        }

        /// <summary>
        /// Method to allow player to pick up an item from the current room.
        /// </summary>
        public void PickUpItem()
        {
            try
            {
                // If there are items in the room, display them and allow user to choose
                if (CurrentRoom.Items.Count > 0)
                {
                    Console.WriteLine("\nYou decide to pick up an item, what do you pick up?");
                    int end = CurrentRoom.Items.Count;
                    for (int i = 0; i < end; i++)
                    {
                        Console.WriteLine($"{i + 1}. {CurrentRoom.Items[i]}");
                    }
                    Console.Write($"{end + 1}. Cancel\n: ");

                    string choiceS = Console.ReadLine();
                    int.TryParse(choiceS, out int choice);
                    if (choice > 0 && choice <= end)
                    {
                        AddItem(CurrentRoom.Items[choice - 1]);
                        CurrentRoom.Items.RemoveAt(choice - 1);
                    }
                    // Breaks out of the loop if user decides to cancel
                    else if (choice == end + 1)
                    {
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Invalid input please try again.");
                        Thread.Sleep(600);
                        PickUpItem();
                    }
                }
                else
                {
                    Console.WriteLine("\nThere are no items to pick up in this room.\n");
                    Thread.Sleep(1500);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Method to display a map of the grid, showing the player's position and rooms explored.
        /// </summary>
        /// <param name="Grid"> The grid representing the dungeon.</param>
        public void DisplayMap(Room[,] Grid)
        {
            Console.WriteLine();
            for (int y = 0; y < Grid.GetLength(1); y++)
            {
                for (int x = 0; x < Grid.GetLength(0); x++)
                {
                    // Checks if the grid position has a room and if the player is currently there
                    if (Grid[x, y] != null)
                    {
                        if (x == PlayerX && y == PlayerY)
                        {
                            Console.Write("P ");
                        }
                        else
                        {
                            Console.Write("X ");
                        }
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
        /// Method to start a fight with an enemy in the current room.
        /// </summary>
        public void FightEnemy()
        {
            combatManager.FightEnemy();
        }

        /// <summary>
        /// Method to attack an enemy during the fight.
        /// </summary>
        /// <param name="enemy"> The enemy to attack.</param>
        /// <param name="choice"> The index of the enemy in the room's enemy list.</param>
        /// <param name="round"> The current ronud of the fight.</param>
        public void Attack(List<object> enemy, int choice, int round)
        {
            combatManager.Attack(enemy, choice, round);
        }
    }
}
