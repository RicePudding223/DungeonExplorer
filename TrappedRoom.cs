using System;

namespace DungeonExplorer
{
    /// <summary>
    /// TrappedRoom class represents a room in the dungeon that contains traps.
    /// </summary>
    internal class TrappedRoom : Room
    {
        /// <summary>
        /// Constructor for the TrappedRoom class.
        /// </summary>
        /// <param name="roomID"> The unique identifier for the room.</param>
        /// <param name="description"> The description of the room.</param>
        /// <param name="roomCount"> The total number of rooms in the dungeon.</param>
        public TrappedRoom(string roomID, string description, int roomCount) : base(roomID, description, roomCount)
        {
        }

        /// <summary>
        /// Method that is called when the player enters the room.
        /// </summary>
        /// <param name="player"> The player instance.</param>
        public override void OnEnter(Player player)
        {
            player.TakeDamage(10);
            Console.WriteLine("You have entered a trapped room! You take 10 damage.");
        }
    }
}
