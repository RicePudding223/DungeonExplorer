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
        /// The Game class's properties. 
        /// Gets and sets for the Player, IsGameOver, and Grid properties.
        /// </summary>
        public Player Player { get; set; }
        public static bool IsGameOver { get; set; }
        public Room[,] Grid { get; set; }

        /// <summary>
        /// Initialises a new instance of the Game class.
        /// </summary>
        /// <param name="player"> The player instance </param>
        /// <param name="currentRoom"> The starting room for the player </param>
        /// <param name="grid"> The grid of rooms representing the game world </param>
        public Game(Player player, Room currentRoom, Room[,] grid)
        {
            Player = player;
            IsGameOver = false;
            Grid = grid;
        }

        /// <summary>
        /// Starts the game loop.
        /// </summary>
        public void Start()
        {
            bool displayInfo = false;
            bool displayMap = false;
            Console.WriteLine("\nStarting Game...\n");
            Thread.Sleep(400);

            // Main game loop
            while (!IsGameOver)
            {
                Thread.Sleep(100);
                Console.Clear();

                // Gets the room information
                Player.CurrentRoom.GetDescription();

                // Displays relavant information if needed
                if (displayInfo)
                {
                    Player.DisplayInfo();
                   displayInfo = false;
                }
                if (displayMap)
                {
                    Player.DisplayMap(Grid);
                    displayMap = false;
                }

                // Asks the player what they want to do
                Console.Write("\nWhat do you want to do?\n\n1. Move to another room\t\t" +
                    "2. Fight!\n3. Pick up items\t\t4. Use Items\n5. View player stats\t\t6. Display the Map\n7. Quit\n: ");
                string choice = Console.ReadLine();

                // Switch statement to handle the player's choice
                switch (choice)
                {
                    case "1":
                        Player.MoveToRoom(Grid, Player.CurrentRoom.RoomCount);
                        break;
                    case "2":
                        Player.FightEnemy();
                        break;
                    case "3":
                        Player.PickUpItem();
                        break;
                    case "4":
                        Player.UseItem();
                        break;
                    case "5":
                        displayInfo = true;
                        break;
                    case "6":
                        displayMap = true;
                        break;
                    case "7":
                        IsGameOver = true;
                        Console.WriteLine("\nThanks for playing!\n");
                        break;
                    default:
                        Console.WriteLine("\nInvalid choice. Try again.\n");
                        Thread.Sleep(600);
                        break;
                }
            }
        }
    }
}
