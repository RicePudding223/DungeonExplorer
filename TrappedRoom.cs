using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
{
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

        public override void OnEnter(Player player)
        {
            player.TakeDamage(10);
            Console.WriteLine("You have entered a trapped room! You take 10 damage.");
        }
    }
}
