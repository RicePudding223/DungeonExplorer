namespace DungeonExplorer
{
    /// <summary>
    /// Represents a potion item that can be consumed to restore health.
    /// Inherits from Item and implements the IConsumable interface.
    /// </summary>
    public class Potion : Item, IConsumable
    {
        public int HealthRecovery { get; set; }

        /// <summary>
        /// Initializes a new instance of the Potion class.
        /// </summary>
        /// <param name="name"> The name of the potion.</param>
        /// <param name="healthRecovery"> The amount of health the potion restores when used.</param>
        public Potion(string name, int healthRecovery) : base(name)
        {
            HealthRecovery = healthRecovery;
        }

        /// <summary>
        /// Uses the potion on the specified player, restoring health.
        /// If the player's health exceeds the maximum health, it is capped at the maximum.
        /// </summary>
        /// <param name="player"> The player who will use the potion.</param>
        public void Use(Player player)
        {
            player.CurrentHealth += HealthRecovery;

            // Ensure the player's health doesn't exceed the maximum health.
            if (player.CurrentHealth > player.MaxHealth)
            {
                player.CurrentHealth = player.MaxHealth;
            }
        }
    }
}
