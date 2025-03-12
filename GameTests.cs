using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// A class to test the game's functionality.
    /// </summary>
    public class GameTests
    {
        /// <summary>
        /// Test the creation of a player object and verifies its properties are initialised correctly.
        /// </summary>
        public void TestPlayerCreation()
        {
            Console.WriteLine("Running TestPlayerCreation...");
            Player player = new Player("TestPlayer", 10, 10, new Room("Room 0", "Starting room", 0));

            Debug.Assert(player.Name == "TestPlayer", "Player name should be 'TestPlayer'.");
            Debug.Assert(player.Health == 100, "Player health should start at 100.");
            Debug.Assert(player.Strength == 10, "Player strength should start at 10.");
            Debug.Assert(player.Inventory.Count == 0, "Player inventory should be empty.");
            Debug.Assert(player.Weapon == null, "Player should not have a weapon equipped.");

            Console.WriteLine("TestPlayerCreation passed.\n");
        }

        /// <summary>
        /// Test the creation of a room object and verifies its properties are initialised correctly.
        /// </summary>
        public void TestRoomCreation()
        {
            Console.WriteLine("Running TestRoomCreation...");
            Room room = new Room("Room 1", "A dark room", 1);

            Debug.Assert(room.RoomID == "Room 1", "Room ID should be 'Room 1'.");
            Debug.Assert(room.Description == "A dark room", "Room description should match.");
            Debug.Assert(room.Exits.Count == 0, "Room should have no exits.");
            Debug.Assert(room.Items.Count == 0, "Room should have no items.");
            Debug.Assert(room.Enemies.Count == 0, "Room should have no enemies.");

            Console.WriteLine("TestRoomCreation passed.\n");
        }

        /// <summary>
        /// Tests the player's movement functionality by moving the player and verifying their new position.
        /// </summary>
        public void TestPlayerMovement()
        {
            Console.WriteLine("Running TestPlayerMovement...");
            Room[,] grid = new Room[20, 20];
            Room startRoom = new Room("Room 0", "Starting room", 0);
            grid[10, 10] = startRoom;
            Player player = new Player("TestPlayer", 10, 10, startRoom);

            // Add exits to the start room
            startRoom.AddExit("North");

            // Move player north
            do 
            {
                player.MoveToRoom(grid, 0);
            } while (player.PlayerX == 10 && player.PlayerY == 10);

            Debug.Assert(player.PlayerX == 10 && player.PlayerY == 9, "Player should move north to (10, 9).");

            Console.WriteLine("TestPlayerMovement passed.\n");
        }

        /// <summary>
        /// Tests the combat system by simulating a player attacking an enemy and verifying the enemy's health is reduced.
        /// </summary>
        public void TestCombat()
        {
            Console.WriteLine("Running TestCombat...");
            Player player = new Player("TestPlayer", 10, 10, new Room("Room 0", "Starting room", 0));
            List<object> enemy = new List<object> { "Goblin", 10, 50, 1 }; // Goblin with 10 damage, 50 health, and speed 1
            player.CurrentRoom.Enemies.Add(enemy);

            // Simulate player attacking the enemy
            player.EquippedWeaponDamage = 15; // Equip a weapon with 15 damage
            player.Attack(enemy, 1, 1);

            Debug.Assert(15 <= (int)enemy[2] && (int)enemy[2] <= 25, "Enemy health should be reduced to between 15-25 depending on multiplier.");

            Console.WriteLine("TestCombat passed.\n");
        }

        /// <summary>
        /// Tests inventory management by adding a weapon to the player's inventory and equipping it.
        /// </summary>
        public void TestInventory()
        {
            Console.WriteLine("Running TestInventory...");
            Player player = new Player("TestPlayer", 10, 10, new Room("Room 0", "Starting room", 0));

            // Add an item to the inventory
            do
            {
                player.Inventory.Clear();
                player.AddItem("Rusty Dagger");
            } while (player.Weapon == null);

            Debug.Assert(player.Inventory.Count == 1, "Inventory should have 1 item.");
            Debug.Assert(player.Inventory.Contains("Rusty Dagger"), "Inventory should contain 'Rusty Dagger'.");

            // Equip the weapon
            Debug.Assert(player.Weapon == "Rusty Dagger", "Player should have 'Rusty Dagger' equipped.");
            Debug.Assert(player.EquippedWeaponDamage == 5, "Equipped weapon damage should be 5.");

            Console.WriteLine("TestInventory passed.");
        }

        /// <summary>
        /// Method to run all tests and outputs a message.
        /// </summary>
        public void RunAllTests()
        {
            TestPlayerCreation();
            TestRoomCreation();
            TestPlayerMovement();
            TestCombat();
            TestInventory();

            Console.WriteLine("All tests passed!");
            Thread.Sleep(2000);
            Console.Clear();
        }
    }
}
