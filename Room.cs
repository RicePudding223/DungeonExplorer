using System.Collections.Generic;
using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a room in the game world.
    /// </summary>
    public class Room
    {
        /// <summary>
        /// Gets and sets for the Room's properties.
        /// </summary>
        public string RoomID { get; set; }
        public int RoomCount { get; set; }
        public string Description { get; set; }
        public List<string> Exits { get; set; }
        public List<string> Items { get; set; }
        public List<Monster> Enemies { get; set; }

        /// <summary>
        /// Initialises a new instance of the Room class.
        /// </summary>
        /// <param name="roomID"> The unique ID for the room.</param>
        /// <param name="description"> The description of the room.</param>
        /// <param name="roomCount"> The number of rooms in the dungeon.</param>
        public Room(string roomID, string description, int roomCount)
        {
            RoomID = roomID;
            RoomCount = roomCount;
            Description = description;
            Exits = new List<string>();
            Items = new List<string>();
            Enemies = new List<Monster>();
        }

        /// <summary>
        /// Method to get the room's description, including items, enemies, and exits.
        /// </summary>
        public void GetDescription()
        {
            Console.WriteLine($"\n{RoomID}: {Description}\n");

            // Writes all the items in the room
            if (Items.Count > 0)
            {
                Console.WriteLine("Items here: " + string.Join(", ", Items) + "\n");
            }

            // Writes all the enemies in the room
            if (Enemies.Count > 0)
            {
                Console.Write("Enemies here: ");
                for (int i = 0; i < Enemies.Count; i++)
                {
                    // Access first element of each enemy item
                    Monster enemy = Enemies[i];
                    Console.Write(enemy.Name);

                    // Add a comma and space if it's not the last enemy
                    if (i < Enemies.Count - 1)
                    {
                        Console.Write(", ");
                    }
                }
                Console.WriteLine("\n");
            }

            // Writes all the exits in the room
            Console.WriteLine("Exits: " + string.Join(", ", Exits) + "\n");
        }

        /// <summary>
        /// Method to add an item to the room.
        /// </summary>
        /// <param name="item"> The item to be added to the room.</param>
        public void AddItem(string item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Method to add an enemy to the room.
        /// </summary>
        /// <param name="enemy"> The enemy to be added to the room.</param>
        public void AddEnemy(Monster enemy)
        {
            Enemies.Add(enemy);
        }

        /// <summary>
        /// Method to add an exit to the room.
        /// </summary>
        /// <param name="exit"> The exit to be added to the room.</param>
        public void AddExit(string exit)
        {
            Exits.Add(exit);
        }
    }
}
