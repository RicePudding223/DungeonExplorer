using System;

namespace DungeonExplorer
{
    /// <summary>
    /// The entry point of the program.
    /// It creates a new instance of the room, player, and game classes and starts the game.
    /// </summary>
    internal class Program
    {
        // Constants for grid size and weapon level range
        private const int GridSize = 10;            // The size of the grid (10x10)
        private const int MinWeaponLevel = 0;       // Minimum weapon level for random weapon selection
        private const int MaxWeaponLevel = 3;       // Maximum weapon level for random weapon selection

        /// <summary>
        /// Main method of the program.
        /// Creates a new instance of the room, player, and game classes.
        /// Then starts the game.
        /// </summary>
        /// <param name="args"> Command-line arguments. </param>
        static void Main(string[] args)
        {
            // Test code can be uncommented for unit testing purposes
            // GameTests tests = new GameTests();
            // tests.RunAllTests();

            // Output the initial game message
            Console.WriteLine("===== Dungeon Crawler =====\n");

            // Creates an empty grid of rooms
            var grid = new Room[GridSize, GridSize];

            // Determine the starting room coordinates (center of the grid)
            int startX = GridSize / 2;
            int startY = GridSize / 2;

            // Create the starting room and places it in the grid
            var startRoom = CreateStartingRoom();
            grid[startX, startY] = startRoom;

            // Create a new player and place them in the starting room
            var player = CreatePlayer(startX, startY, startRoom);

            // Set the number of rooms to explore
            int RoomCount = 1;

            // Create and start a new game
            var game = new Game(player, grid, RoomCount);
            game.Start();

            // Wait for the user to press any key to exit
            Console.Write("Press any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Creates the starting room for the game.
        /// This room contains a weapon and an exit to the North.
        /// </summary>
        /// <returns> A Room object that represents the starting room. </returns>
        private static Room CreateStartingRoom()
        {
            // Room description with initial scenario
            string roomDescription = "You wake up in a dark room, you read a sign on the wall \"You must survive 10 rooms to leave\"" +
                " it seems that there is a weapon on the ground and one door to the North.";

            // Create the starting room with the description
            var startRoom = new Room("Room 0", roomDescription, 0);

            // Get a random weapon within the defined level range and add it to the room
            var startingWeapon = GameData.GetRandomWeapon(MinWeaponLevel, MaxWeaponLevel);
            startRoom.AddItem(startingWeapon);

            // Add an exit to the North
            startRoom.AddExit("North");

            return startRoom;
        }

        /// <summary>
        /// Creates a new player for the game.
        /// Prompts the user for their name and sets the initial coordinates and room.
        /// </summary>
        /// <param name="startX"> The starting X-coordinate of the player on the grid. </param>
        /// <param name="startY"> The starting Y-coordinate of the player on the grid. </param>
        /// <param name="startRoom"> The starting room of the player. </param>
        /// <returns> A Player object representing the character the user will play. </returns>
        private static Player CreatePlayer(int startX, int startY, Room startRoom)
        {
            // Instantiate the UIManager to interact with the user
            var uiManager = new UIManager();

            // Prompt the user for their player name
            string playerName = uiManager.GetPlayerName();

            // Return a new Player object with the provided details
            return new Player(playerName, startX, startY, startRoom);
        }
    }
}
