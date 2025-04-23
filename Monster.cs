namespace DungeonExplorer
{
    /// <summary>
    /// Represents a monster in the dungeon with its name, health, strength, and speed.
    /// Inherits from the Creature class.
    /// </summary>
    public class Monster : Creature
    {
        public int Speed { get; set; }  // Speed property unique to the Monster class

        /// <summary>
        /// Initializes a new instance of the Monster class.
        /// </summary>
        /// <param name="name">The name of the monster.</param>
        /// <param name="health">The health points of the monster.</param>
        /// <param name="strength">The strength points of the monster.</param>
        /// <param name="speed">The speed of the monster.</param>
        public Monster(string name, int health, int strength, int speed)
             : base(name, health, strength)  // Call the base class constructor
        {
            Speed = speed;
        }
    }
}
