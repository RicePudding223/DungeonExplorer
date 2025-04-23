using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    /// <summary>
    /// Handles player movement between rooms in the dungeon.
    /// </summary>
    public class RoomMovement
    {
        private readonly Player _player; // Reference to the player object.
        private readonly RoomManager _roomManager; // Manages room creation and interactions.
        private bool LastRoom = false; // Tracks whether the player has reached the last room.

        /// <summary>
        /// Initializes the RoomMovement class with the player and room manager.
        /// </summary>
        /// <param name="player"> The player instance.</param>
        /// <param name="roomManager"> The room manager instance.</param>
        public RoomMovement(Player player, RoomManager roomManager)
        {
            _player = player;
            _roomManager = roomManager;
        }

        /// <summary>
        /// Checks if there are enemies in the current room that must be defeated before moving.
        /// </summary>
        /// <returns> True if enemies are present, otherwise false.</returns>
        private bool CheckForEnemies()
        {
            if (_player.CurrentRoom.Enemies.Count > 0)
            {
                Console.WriteLine("\nYou must defeat all enemies in the room before moving to another room.\n");
                return true;
            }
            return false;
        }

        /// <summary>
        /// Prompts the player to choose a direction to move.
        /// </summary>
        /// <returns> The player's choice as an integer.</returns>
        private int GetRoomChoice()
        {
            Console.WriteLine("\nYou decide to move to another room, where do you want to go?");
            for (int i = 0; i < _player.CurrentRoom.Exits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_player.CurrentRoom.Exits[i]}");
            }
            Console.Write($"{_player.CurrentRoom.Exits.Count + 1}. Cancel\n: ");
            return int.TryParse(Console.ReadLine(), out int choice) ? choice : -1;
        }

        /// <summary>
        /// Handles room movement logic, including room creation if needed.
        /// </summary>
        /// <param name="direction"> The direction the player chooses to move.</param>
        /// <param name="Grid"> The grid representation of the dungeon.</param>
        /// <param name="RoomCount"> The current number of rooms explored.</param>
        private void ProcessRoomMovement(string direction, Room[,] Grid, int RoomCount)
        {
            MovePlayer(direction); // Updates the player's position.
            if (Game.IsGameOver) // Checks if the game is over.
            {
                return;
            }

            if (RoomCount == 10) // Determines if the player has reached the last room.
            {
                LastRoom = true;
            }

            if (Grid[_player.PlayerX, _player.PlayerY] != null) // Checks if the room already exists.
            {
                _player.CurrentRoom = Grid[_player.PlayerX, _player.PlayerY];
            }
            else
            {
                // Creates a new room and updates the grid.
                _player.CurrentRoom = _roomManager.CreateNewRoom($"Room {RoomCount}", direction, RoomCount, Grid, _player.PlayerX, _player.PlayerY, LastRoom);
                Grid[_player.PlayerX, _player.PlayerY] = _player.CurrentRoom;
            }
            Console.WriteLine($"\nYou move {direction} into Room {RoomCount}.\n");
        }

        /// <summary>
        /// Initiates room movement for the player, validating choices and updating room count.
        /// </summary>
        /// <param name="Grid"> The grid representation of the dungeon.</param>
        /// <param name="RoomCount"> The current number of rooms explored.</param>
        public void MoveToRoom(Room[,] Grid, int RoomCount)
        {
            if (CheckForEnemies()) // Prevents movement if enemies are present.
                return;

            int choice = GetRoomChoice(); // Gets the player's choice.
            if (choice > 0 && choice <= _player.CurrentRoom.Exits.Count)
            {
                RoomCount++; // Increments room count for each valid movement.
                ProcessRoomMovement(_player.CurrentRoom.Exits[choice - 1], Grid, RoomCount);
            }
            else if (choice != _player.CurrentRoom.Exits.Count + 1)
            {
                // Handles invalid input.
                Console.WriteLine("Invalid input, please try again.");
                Thread.Sleep(600);
            }
        }

        /// <summary>
        /// Updates the player's coordinates based on the chosen direction.
        /// </summary>
        /// <param name="direction"> The direction the player decides to move in.</param>
        private void MovePlayer(string direction)
        {
            switch (direction)
            {
                case "North":
                    _player.PlayerY--;
                    break;
                case "South":
                    _player.PlayerY++;
                    break;
                case "East":
                    _player.PlayerX++; 
                    break;
                case "West":
                    _player.PlayerX--; 
                    break;
                case "Exit":
                    // Ends the game when the player reaches the exit.
                    Console.WriteLine("\nYou have reached the exit, congratulations!\n");
                    Game.IsGameOver = true;
                    break;
            }
        }
    }
}
