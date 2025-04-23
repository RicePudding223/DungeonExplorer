using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Handles player movement between rooms in the dungeon.
    /// </summary>
    public class RoomMovement
    {
        private readonly Player _player; // Reference to the player object.
        private readonly RoomManager _roomManager; // Manages room creation and interactions.
        private readonly UIManager _uiManager; // Manages user interface interactions.
        private bool LastRoom = false; // Tracks whether the player has reached the last room.

        /// <summary>
        /// Initializes the RoomMovement class with the player, room manager, and UI manager.
        /// </summary>
        /// <param name="player"> The player instance.</param>
        /// <param name="roomManager"> The room manager instance.</param>
        /// <param name="uiManager"> The UI manager instance.</param>
        public RoomMovement(Player player, RoomManager roomManager, UIManager uiManager)
        {
            _player = player;
            _roomManager = roomManager;
            _uiManager = uiManager;
        }

        /// <summary>
        /// Checks if there are enemies in the current room that must be defeated before moving.
        /// </summary>
        /// <returns> True if enemies are present, otherwise false.</returns>
        private bool CheckForEnemies()
        {
            if (_player.CurrentRoom.Enemies.Count > 0)
            {
                _uiManager.ShowMessage("You must defeat all enemies in the room before moving to another room.", true);
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
            _uiManager.ShowMessage("You decide to move to another room, where do you want to go?");
            for (int i = 0; i < _player.CurrentRoom.Exits.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {_player.CurrentRoom.Exits[i]}");
            }
            Console.WriteLine($"{_player.CurrentRoom.Exits.Count + 1}. Cancel");
            return _uiManager.GetValidChoice(1, _player.CurrentRoom.Exits.Count + 1);
        }

        /// <summary>
        /// Handles room movement logic, including room creation if needed.
        /// </summary>
        /// <param name="direction"> The direction the player chooses to move.</param>
        /// <param name="Grid"> The grid representation of the dungeon.</param>
        /// <param name="RoomCount"> The current number of rooms explored.</param>
        private void ProcessRoomMovement(string direction, Room[,] Grid, int RoomCount)
        {
            MovePlayer(direction);
            if (Game.IsGameOver) return;

            if (RoomCount == 10) LastRoom = true;

            if (Grid[_player.PlayerX, _player.PlayerY] != null)
            {
                _player.CurrentRoom = Grid[_player.PlayerX, _player.PlayerY];
            }
            else
            {
                _player.CurrentRoom = _roomManager.CreateNewRoom($"Room {RoomCount}", direction, RoomCount, Grid, _player.PlayerX, _player.PlayerY, LastRoom);
                Grid[_player.PlayerX, _player.PlayerY] = _player.CurrentRoom;
            }

            _uiManager.ShowMessage($"You move {direction} into Room {RoomCount}.");
        }

        /// <summary>
        /// Initiates room movement for the player, validating choices and updating room count.
        /// </summary>
        /// <param name="Grid"> The grid representation of the dungeon.</param>
        /// <param name="RoomCount"> The current number of rooms explored.</param>
        public void MoveToRoom(Room[,] Grid, int RoomCount)
        {
            if (CheckForEnemies()) return;

            int choice = GetRoomChoice();
            if (choice > 0 && choice <= _player.CurrentRoom.Exits.Count)
            {
                RoomCount++;
                ProcessRoomMovement(_player.CurrentRoom.Exits[choice - 1], Grid, RoomCount);
            }
            else if (choice != _player.CurrentRoom.Exits.Count + 1)
            {
                _uiManager.ShowMessage("Invalid input, please try again.", true);
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
                case "North": _player.PlayerY--; break;
                case "South": _player.PlayerY++; break;
                case "East": _player.PlayerX++; break;
                case "West": _player.PlayerX--; break;
                case "Exit":
                    _uiManager.ShowMessage("You have reached the exit, congratulations!");
                    Game.IsGameOver = true;
                    break;
            }
        }
    }
}
