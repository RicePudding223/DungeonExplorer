using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    /// <summary>
    /// Manages the creation of new rooms in the dungeon, including adding exits, enemies, and items.
    /// </summary>
    public class RoomManager
    {
        /// <summary>
        /// A dictionary mapping each direction to its opposite. Used for managing room exits.
        /// </summary>
        private static readonly Dictionary<string, string> OppositeDirections = new Dictionary<string, string>
        {
            { "North", "South" },
            { "South", "North" },
            { "East", "West" },
            { "West", "East" }
        };

        /// <summary>
        /// Creates a new room based on the player's movement direction and other parameters.
        /// It sets up exits, adds enemies and items (unless it’s the last room), and updates the player's position.
        /// </summary>
        /// <param name="roomID"> The unique identifier for the room being created.</param>
        /// <param name="direction"> The direction in which the player is moving.</param>
        /// <param name="roomCount"> The total number of rooms in the dungeon.</param>
        /// <param name="grid"> The 2D grid representing the dungeon layout.</param>
        /// <param name="playerX"> The new X-coordinate of the player's position in the dungeon.</param>
        /// <param name="playerY"> The new Y-coordinate of the player's position in the dungeon.</param>
        /// <param name="lastRoom"> Indicates whether this is the last room in the dungeon.</param>
        /// <returns> A new Room object with exits, enemies, and items.</returns>
        public Room CreateNewRoom(string roomID, string direction, int roomCount, Room[,] grid, int playerX, int playerY, bool lastRoom)
        {
            Room newRoom = new Room(roomID, GameData.GetRandomRoomDescription(), roomCount);

            // Add an exit leading back to the previous room (opposite direction).
            string oppositeDirection = OppositeDirections[direction];
            newRoom.AddExit(oppositeDirection);

            // Add additional exits based on the player's position and the existing grid.
            AddNewExits(newRoom, grid, playerX, playerY);

            // If this is not the last room, add random enemies and items to the room.
            if (!lastRoom)
            {
                AddRandomEnemiesAndItems(newRoom, roomCount);
            }
            else
            {
                // If it's the last room, modify the description, limit exits to a single "Exit" and add a final enemy.
                newRoom.Description = "You have reached the final room, be careful.";
                newRoom.Exits.RemoveRange(1, newRoom.Exits.Count - 1);  // Keep only the final exit
                newRoom.AddExit("Exit");
                newRoom.AddEnemy(new Boss("Dragon", 120, 50, 4));
            }

            return newRoom;
        }

        /// <summary>
        /// Adds new exits to the room based on the player's position and the state of the dungeon grid.
        /// Ensures valid exit connections and avoids overlaps with existing rooms.
        /// </summary>
        /// <param name="newRoom"> The room being created, to which exits will be added.</param>
        /// <param name="grid"> The 2D grid representing the dungeon layout.</param>
        /// <param name="playerX"> The new X-coordinate of the player's position.</param>
        /// <param name="playerY"> The new Y-coordinate of the player's position.</param>
        private void AddNewExits(Room newRoom, Room[,] grid, int playerX, int playerY)
        {
            bool doorAdded = false;
            Random random = new Random();

            // Attempt to add at least one valid exit based on the player's position.
            while (!doorAdded)
            {
                foreach (string key in OppositeDirections.Keys)
                {
                    int newX = playerX, newY = playerY;
                    string oppositeExit = "";

                    // Determine new coordinates based on the current direction.
                    switch (key)
                    {
                        case "North": newY--; oppositeExit = "South"; break;
                        case "South": newY++; oppositeExit = "North"; break;
                        case "East": newX++; oppositeExit = "West"; break;
                        case "West": newX--; oppositeExit = "East"; break;
                    }

                    // Skip if the room already contains the exit or if the new room's position is out of bounds.
                    if (newRoom.Exits.Contains(key) || newX < 0 || newY < 0 || newX >= grid.GetLength(1) || newY >= grid.GetLength(0))
                        continue;

                    // Skip if there is a room in the way and it doesn't have the opposite exit.
                    if (grid[newX, newY] != null && !grid[newX, newY].Exits.Contains(oppositeExit))
                        continue;

                    // If a room exists and has the opposite exit, add the new exit to the current room
                    if (grid[newX, newY] != null && grid[newX, newY].Exits.Contains(oppositeExit))
                    {
                        newRoom.AddExit(key);
                        continue;
                    }

                    // Randomly decide whether to add an exit
                    int chance = random.Next(1, 5);
                    if (chance == 1)
                    {
                        newRoom.AddExit(key);
                        doorAdded = true;  // Exit found and added
                    }
                }
            }
        }

        /// <summary>
        /// Adds a random number of enemies and items to the room, based on the total number of rooms in the dungeon.
        /// </summary>
        /// <param name="newRoom"> The room being created.</param>
        /// <param name="roomCount"> The total number of rooms in the dungeon, used to scale the number of enemies and items.</param>
        private void AddRandomEnemiesAndItems(Room newRoom, int roomCount)
        {
            // Add a random number of enemies, scaling with the room count
            int amountOfEnemies = 1 + (roomCount / 3);
            for (int i = 0; i < amountOfEnemies; i++)
            {
                newRoom.AddEnemy(GameData.GetRandomEnemy(0, 1 + (roomCount / 2)));
            }

            // Add a random number of items, scaling with the room count
            int amountOfItems = 0 + (roomCount / 2);
            Random randomItem = new Random();
            for (int i = 0; i < amountOfItems; i++)
            {
                // Randomly decide whether the item is a potion or a weapon
                int chance = randomItem.Next(1, 3);
                if (chance == 1)
                {
                    newRoom.AddItem(GameData.GetRandomPotion((0 + roomCount / 5), (1 + roomCount / 3)));
                }
                else
                {
                    newRoom.AddItem(GameData.GetRandomWeapon((2 + roomCount), (5 + roomCount)));
                }
            }
        }
    }
}
