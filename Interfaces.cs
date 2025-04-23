namespace DungeonExplorer
{
    /// <summary>
    /// Interface for entities that can take damage (player, monsters).
    /// </summary>
    public interface IDamageable
    {
        int CurrentHealth { get; } // The current health of the entity.

        /// <summary>
        /// Applies damage to the entity.
        /// </summary>
        /// <param name="amount"> The amount of damage to apply.</param>
        void TakeDamage(int amount);
    }

    /// <summary>
    /// Interface for entities that can be healed (player).
    /// </summary>
    public interface IHealable
    {
        int CurrentHealth { get; } // The current health of the entity.

        /// <summary>
        /// Heals the entity using a potion.
        /// </summary>
        /// <param name="potion"> The potion to use for healing.</param>
        void Heal(Potion potion);
    }

    /// <summary>
    /// Interface for consumable items (potions).
    /// </summary>
    public interface IConsumable
    {
        /// <summary>
        /// Uses the consumable item, affecting the player.
        /// </summary>
        /// <param name="player"> The player who is using the consumable item.</param>
        void Use(Player player);
    }
}
