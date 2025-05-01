using DungeonExplorer;
using System.Net;

/// <summary>
/// Boss class represents a powerful enemy in the game.
/// </summary>
public class Boss : Monster, IBoss
{
    public bool secondPhase { get; set; } = false;

    /// <summary>
    /// Initializes a new instance of the Boss class with specified name, health, strength, and speed values.
    /// </summary>
    /// <param name="name"> The name of the monster.</param>
    /// <param name="health"> The health points of the monster.</param>
    /// <param name="strength"> The strength points of the monster.</param>
    /// <param name="speed"> The speed of the monster.</param>
    public Boss(string name, int health, int strength, int speed)
        : base(name, health, strength, speed)
    {
    }

    /// <summary>
    /// Sets the boss to its second phase.
    /// </summary>
    public void SecondPhase()
    {
        secondPhase = true;
        this.CurrentHealth = (int)(this.MaxHealth * 1.5); // Increase health
        this.MaxHealth = this.CurrentHealth;
        this.Strength = (int)(this.Strength * 1.2); // Make boss stronger
    }

    /// <summary>
    /// Handles the death of the boss.
    /// </summary>
    /// <param name="killer"> The creature object that is killing the boss.</param>
    public override void OnDeath(Creature killer)
    {
        base.OnDeath(killer);
        if (killer is Player player)
        {
            player.Score += 50 + Strength;
        }
    }
}

