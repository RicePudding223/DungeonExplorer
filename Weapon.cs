namespace DungeonExplorer
{
    /// <summary>
    /// Represents a weapon item that can be equipped by the player.
    /// Inherits from Item and stores the weapon's damage value.
    /// </summary>
    public class Weapon : Item
    {
        public int Damage { get; set; }

        /// <summary>
        /// Initializes a new instance of the Weapon class.
        /// </summary>
        /// <param name="name"> The name of the weapon.</param>
        /// <param name="damage"> The amount of damage the weapon deals when used in combat.</param>
        public Weapon(string name, int damage) : base(name)
        {
            Damage = damage;
        }

        /// <summary>
        /// Retrieves the damage value of the weapon.
        /// </summary>
        /// <returns> The damage value of the weapon.</returns>
        public int GetDamage()
        {
            return Damage;
        }
    }
}
