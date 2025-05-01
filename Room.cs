using System.Collections.Generic;

namespace DungeonExplorer
{
    public class Room
    {
        private readonly UIManager _uiManager;

        // Room properties
        public string RoomID { get; set; }  // The unique ID for the room
        public int RoomCount { get; set; }  // The total number of rooms in the dungeon
        public string Description { get; set; }  // The description of the room
        public List<string> Exits { get; set; }  // List of exits available in the room
        public List<Item> Items { get; set; }  // List of items present in the room
        public List<Monster> Enemies { get; set; }  // List of enemies present in the room

        // Constructor to initialize a new room with specified properties
        public Room(string roomID, string description, int roomCount)
        {
            RoomID = roomID;  
            RoomCount = roomCount; 
            Description = description;
            Exits = new List<string>(); 
            Items = new List<Item>(); 
            Enemies = new List<Monster>();
            _uiManager = new UIManager();
        }

        /// <summary>
        /// Method to handle player entering the room.
        /// </summary>
        /// <param name="player"> The current player instance.</param>
        public virtual void OnEnter(Player player)
        {
        }

        /// <summary>
        /// Method to add an item to the room's list of items
        /// </summary>
        /// <param name="item"> The item being added to the room.</param>
        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Method to add an enemy to the room's list of enemies
        /// </summary>
        /// <param name="enemy"> The enemy being added to the room.</param>
        public void AddEnemy(Monster enemy)
        {
            Enemies.Add(enemy); 
        }

        /// <summary>
        /// Method to add an exit to the room's list of exits
        /// </summary>
        /// <param name="exit"> The exit being added to the room.</param>
        public void AddExit(string exit)
        {
            Exits.Add(exit);
        }
    }
}
