using System;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents the main game class.
    /// It holds the main game loop that allows the user to interact with the game world.
    /// </summary>
    internal class Game
    {
        /// <summary>
        /// Properties for the Game class.
        /// </summary>
        public Player Player { get; set; }                 // The current player
        public static bool IsGameOver { get; set; }         // Flag to indicate if the game is over
        public Room[,] Grid { get; set; }                   // The grid of rooms representing the game world
        public int RoomCount { get; set; }                  // The total number of rooms in the game

        // Private manager instances for handling game functionalities
        private CombatManager _combatManager;
        private UIManager _uiManager; 
        private RoomMovement _roomMovement;  
        private RoomManager _roomManager;                    

        /// <summary>
        /// Initializes a new instance of the Game class.
        /// </summary>
        /// <param name="player">The player instance.</param>
        /// <param name="grid">The grid of rooms representing the game world.</param>
        public Game(Player player, Room[,] grid, int roomCount)
        {
            Player = player;
            Grid = grid;
            IsGameOver = false;

            // Initialize managers responsible for UI, room movement, and combat
            _uiManager = new UIManager();
            _roomManager = new RoomManager();
            _roomMovement = new RoomMovement(player, _roomManager, _uiManager, roomCount);
            _combatManager = new CombatManager(player, _uiManager);
        }

        /// <summary>
        /// Starts the game loop, which runs until the game is over.
        /// </summary>
        public void Start()
        {
            bool displayInfo = false;
            bool displayMap = false;

            Console.WriteLine("\nStarting Game...\n");
            Thread.Sleep(200);  // Adding a small delay before starting the game

            // Main game loop
            while (!IsGameOver)
            {
                // Clear screen for different displays
                if (displayInfo || displayMap)
                {
                    Console.Clear();  // Does not wait for user input after they select an option
                }
                else
                {
                    _uiManager.WaitForInput();  // Wait for user input before proceeding
                }

                // Display the current room and its details
                _uiManager.DisplayRoom(Player.CurrentRoom);

                // Display player information or map if required
                if (displayInfo)
                {
                    _uiManager.DisplayPlayerInfo(Player);
                    displayInfo = false;  // Reset flag after displaying info
                }

                if (displayMap)
                {
                    _uiManager.DisplayMap(Grid, Player);
                    displayMap = false;  // Reset flag after displaying map
                }

                // Show the main menu to the player
                int choice = _uiManager.ShowMainMenu();

                // Handle the player's choice based on the menu option selected
                switch (choice)
                {
                    case 1:
                        _roomMovement.MoveToRoom(Grid, Player.CurrentRoom.RoomCount);  // Move to another room
                        break;
                    case 2:
                        _combatManager.FightEnemy();  // Start combat with an enemy
                        break;
                    case 3:
                        Player.PickUpItem(_uiManager);  // Player picks up an item
                        break;
                    case 4:
                        Player.UseItem(_uiManager);  // Player uses an item
                        break;
                    case 5:
                        displayInfo = true;  // Flag to display player info
                        break;
                    case 6:
                        displayMap = true;  // Flag to display the game map
                        break;
                    case 7:
                        IsGameOver = true;  // Set game over flag and end the game
                        Console.WriteLine("\nThanks for playing!\n");
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Try again.\n");  // Handle invalid menu option
                        break;
                }
            }
        }
    }
}
