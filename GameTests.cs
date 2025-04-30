using System;
using System.Diagnostics;
using System.Threading;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// A comprehensive testing class for DungeonExplorer game components
    /// </summary>
    public class GameTests
    {
        private ConsoleColor _successColor = ConsoleColor.Green;
        private ConsoleColor _testColor = ConsoleColor.Cyan;
        private ConsoleColor _highlightColor = ConsoleColor.Yellow;

        /// <summary>
        /// Test the Boss class and its second phase functionality
        /// </summary>
        public void TestBoss()
        {
            ColorWriteLine("Running TestBoss...", _testColor);
            var boss = new Boss("Dragon", 100, 30, 2);

            Debug.Assert(boss.Name == "Dragon", $"Boss name should be 'Dragon' (Actual: {boss.Name})");
            Debug.Assert(boss.CurrentHealth == 100, $"Boss health should be 100 (Actual: {boss.CurrentHealth})");
            Debug.Assert(!boss.secondPhase, "Boss should not be in second phase initially");

            boss.SecondPhase();

            Debug.Assert(boss.secondPhase, "Boss should be in second phase after activation");
            Debug.Assert(boss.CurrentHealth == 150, $"Boss health should be 150 in second phase (Actual: {boss.CurrentHealth})");
            Debug.Assert(boss.Strength == 36, $"Boss strength should be 36 in second phase (Actual: {boss.Strength})");

            ColorWriteLine("TestBoss passed.\n", _successColor);
        }

        /// <summary>
        /// Test the CombatManager class functionality
        /// </summary>
        public void TestCombatManager()
        {
            ColorWriteLine("Running TestCombatManager...", _testColor);
            var player = new Player("TestPlayer", 10, 10, new Room("Room 0", "Test Room", 0));
            var uiManager = new UIManager();
            var combatManager = new CombatManager(player, uiManager);

            player.CurrentHealth = 0;
            Debug.Assert(player.CheckAlive() == false, "Player should be dead when health is 0");
            player.CurrentHealth = 100;

            var enemy = new Monster("Goblin", 50, 10, 1);
            player.CurrentRoom.Enemies.Add(enemy);
            player.EquippedWeaponDamage = 15;

            int initialHealth = enemy.CurrentHealth;
            combatManager.Attack(enemy);
            Debug.Assert(enemy.CurrentHealth < initialHealth,
                $"Enemy should take damage (Expected: <{initialHealth}, Actual: {enemy.CurrentHealth})");

            ColorWriteLine("TestCombatManager passed.\n", _successColor);
        }

        /// <summary>
        /// Test the Creature base class functionality
        /// </summary>
        public void TestCreature()
        {
            ColorWriteLine("Running TestCreature...", _testColor);
            var creature = new Monster("TestCreature", 100, 20, 2);

            Debug.Assert(creature.Name == "TestCreature", $"Creature name should match (Actual: {creature.Name})");
            Debug.Assert(creature.CurrentHealth == 100, $"Creature health should be 100 (Actual: {creature.CurrentHealth})");
            Debug.Assert(creature.CheckAlive(), "Creature should be alive initially");

            creature.TakeDamage(50);
            Debug.Assert(creature.CurrentHealth == 50, $"Creature health should be 50 (Actual: {creature.CurrentHealth})");
            Debug.Assert(creature.CheckAlive(), "Creature should still be alive");

            creature.TakeDamage(60);
            Debug.Assert(!creature.CheckAlive(), $"Creature should be dead (Health: {creature.CurrentHealth})");

            ColorWriteLine("TestCreature passed.\n", _successColor);
        }

        /// <summary>
        /// Test the Game class initialization
        /// </summary>
        public void TestGame()
        {
            ColorWriteLine("Running TestGame...", _testColor);
            var player = new Player("TestPlayer", 10, 10, new Room("Room 0", "Test Room", 0));
            var grid = new Room[20, 20];
            grid[10, 10] = player.CurrentRoom;
            var game = new Game(player, grid, 10);

            Debug.Assert(game.Player == player, "Game should reference the player");
            Debug.Assert(game.Grid == grid, "Game should reference the grid");
            Debug.Assert(!Game.IsGameOver, "Game should not be over initially");

            ColorWriteLine("TestGame passed.\n", _successColor);
        }

        /// <summary>
        /// Test the InventoryManager functionality
        /// </summary>
        public void TestInventoryManager()
        {
            ColorWriteLine("Running TestInventoryManager...", _testColor);
            var player = new Player("TestPlayer", 10, 10, new Room("Room 0", "Test Room", 0));
            var uiManager = new UIManager();
            var inventoryManager = new InventoryManager(player, uiManager);

            var weapon = new Weapon("Test Sword", 20);
            inventoryManager.EquipWeapon(weapon, true);

            Debug.Assert(player.Weapon == weapon, $"Player weapon should be {weapon.Name}");
            Debug.Assert(player.EquippedWeaponDamage == 20, $"Weapon damage should be 20 (Actual: {player.EquippedWeaponDamage})");

            player.Inventory.Add(new Potion("Health Potion", 20));
            player.Inventory.Add(new Weapon("Weak Dagger", 5));
            player.Inventory.Add(new Weapon("Strong Axe", 30));
            var sorted = inventoryManager.GetSortedInventory();

            Debug.Assert(sorted.Count == 3, $"Inventory should have 3 items (Actual: {sorted.Count})");
            Debug.Assert(sorted[0] is Weapon, "First item should be a weapon");

            ColorWriteLine("TestInventoryManager passed.\n", _successColor);
        }

        /// <summary>
        /// Test the Item class hierarchy
        /// </summary>
        public void TestItems()
        {
            ColorWriteLine("Running TestItems...", _testColor);
            var weapon = new Weapon("Test Weapon", 15);
            var potion = new Potion("Test Potion", 25);

            Debug.Assert(weapon is Item, "Weapon should inherit from Item");
            Debug.Assert(potion is Item, "Potion should inherit from Item");
            Debug.Assert(weapon.Name == "Test Weapon", $"Weapon name should be 'Test Weapon' (Actual: {weapon.Name})");

            var player = new Player("TestPlayer", 10, 10, new Room("Room 0", "Test Room", 0));
            player.CurrentHealth = 50;
            potion.Use(player);

            Debug.Assert(player.CurrentHealth == 75, $"Player health should be 75 (Actual: {player.CurrentHealth})");

            ColorWriteLine("TestItems passed.\n", _successColor);
        }

        /// <summary>
        /// Test the Monster class
        /// </summary>
        public void TestMonster()
        {
            ColorWriteLine("Running TestMonster...", _testColor);
            var monster = new Monster("Goblin", 50, 10, 1);

            Debug.Assert(monster.Name == "Goblin", $"Monster name should be 'Goblin' (Actual: {monster.Name})");
            Debug.Assert(monster.CurrentHealth == 50, $"Monster health should be 50 (Actual: {monster.CurrentHealth})");
            Debug.Assert(monster.Strength == 10, $"Monster strength should be 10 (Actual: {monster.Strength})");

            ColorWriteLine("TestMonster passed.\n", _successColor);
        }

        /// <summary>
        /// Test the Player class functionality
        /// </summary>
        public void TestPlayer()
        {
            ColorWriteLine("Running TestPlayer...", _testColor);
            var room = new Room("Room 0", "Test Room", 0);
            var player = new Player("TestPlayer", 10, 10, room);

            Debug.Assert(player.Name == "TestPlayer", $"Player name should match (Actual: {player.Name})");
            Debug.Assert(player.CurrentHealth == 100, $"Player health should be 100 (Actual: {player.CurrentHealth})");

            var potion = new Potion("Health Potion", 20);
            room.Items.Add(potion);
            player.PickUpItem();

            Debug.Assert(player.Inventory.Count == 1, $"Inventory should have 1 item (Actual: {player.Inventory.Count})");
            Debug.Assert(player.Inventory[0] == potion, $"Inventory should contain the potion");

            ColorWriteLine("TestPlayer passed.\n", _successColor);
        }

        /// <summary>
        /// Test the Room class functionality
        /// </summary>
        public void TestRoom()
        {
            ColorWriteLine("Running TestRoom...", _testColor);
            var room = new Room("Room 1", "Test Room", 1);

            Debug.Assert(room.RoomID == "Room 1", $"Room ID should match (Actual: {room.RoomID})");
            Debug.Assert(room.Description == "Test Room", $"Description should match (Actual: {room.Description})");

            room.AddExit("North");
            var weapon = new Weapon("Test Sword", 15);
            room.AddItem(weapon);
            var enemy = new Monster("Goblin", 50, 10, 1);
            room.AddEnemy(enemy);

            Debug.Assert(room.Exits.Count == 1, $"Should have 1 exit (Actual: {room.Exits.Count})");
            Debug.Assert(room.Items.Count == 1, $"Should have 1 item (Actual: {room.Items.Count})");

            ColorWriteLine("TestRoom passed.\n", _successColor);
        }

        /// <summary>
        /// Test the RoomManager class
        /// </summary>
        public void TestRoomManager()
        {
            ColorWriteLine("Running TestRoomManager...", _testColor);
            var roomManager = new RoomManager();
            var grid = new Room[20, 20];
            var startRoom = new Room("Room 0", "Start Room", 0);
            grid[10, 10] = startRoom;
            startRoom.AddExit("North");

            var newRoom = roomManager.CreateNewRoom("Room 1", "North", 100, grid, 10, 9, false);

            Debug.Assert(newRoom != null, "New room should be created");
            Debug.Assert(newRoom.Exits.Contains("South"), "Should have exit back to previous room");
            Debug.Assert(newRoom.Enemies.Count > 0, $"Should have enemies (Actual: {newRoom.Enemies.Count})");

            ColorWriteLine("TestRoomManager passed.\n", _successColor);
        }

        /// <summary>
        /// Helper method to write colored text to the console.
        /// </summary>
        /// <param name="text"> The string of text being changed.</param>
        /// <param name="color"> The colour being used</param>
        private void ColorWriteLine(string text, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }

        /// <summary>
        /// Run all tests
        /// </summary>
        public void RunAllTests()
        {
            Console.ForegroundColor = _highlightColor;
            Console.WriteLine("=== Starting Test Suite ===");
            Console.ResetColor();

            TestBoss();
            TestCombatManager();
            TestCreature();
            TestGame();
            TestInventoryManager();
            TestItems();
            TestMonster();
            TestPlayer();
            TestRoom();
            TestRoomManager();

            Console.ForegroundColor = _successColor;
            Console.WriteLine("\nAll tests passed successfully!");
            Console.ResetColor();
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
            Console.Clear();
        }
    }
}