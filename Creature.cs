using System;

namespace DungeonExplorer
{
    /// <summary>
    /// Represents a base class for creatures (e.g., players, monsters) that are damageable.
    /// Implements the IDamageable interface to handle health and damage-related operations.
    /// </summary>
    public abstract class Creature : IDamageable
    {
        public string Name { get; set; }
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }
        public int Strength { get; set; }

        /// <summary>
        /// Initializes a new instance of the Creature class with specified name, health, and strength values.
        /// </summary>
        /// <param name="name"> The name of the creature.</param>
        /// <param name="health"> The maximum and initial health of the creature.</param>
        /// <param name="strength"> The strength of the creature, used for calculating damage.</param>
        protected Creature(string name, int health, int strength)
        {
            Name = name;
            CurrentHealth = health;
            MaxHealth = health;
            Strength = strength;
        }

        /// <summary>
        /// Checks if the creature is still alive.
        /// </summary>
        /// <returns> True if the creature has more than 0 health, otherwise false.</returns>
        public bool CheckAlive()
        {
            return CurrentHealth > 0;
        }

        /// <summary>
        /// Reduces the creature's health by the specified damage amount.
        /// </summary>
        /// <param name="damage"> The amount of damage to deal to the creature.</param>
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
        }

        public virtual void OnDeath(Creature killer)
        {
            Console.WriteLine($"{Name} has been defeated by {killer.Name}");
        }
    }
}
