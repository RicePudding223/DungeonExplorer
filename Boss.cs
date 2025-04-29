using DungeonExplorer;
using System.Net;

public class Boss : Monster, IBoss
{
    public bool secondPhase { get; set; } = false;

    public Boss(string name, int health, int strength, int speed)
        : base(name, health, strength, speed)
    {
    }

    public void SecondPhase()
    {
        secondPhase = true;
        this.CurrentHealth = (int)(this.MaxHealth * 1.5); // Increase health
        this.MaxHealth = this.CurrentHealth;
        this.Strength = (int)(this.Strength * 1.2); // Make boss stronger
    }
}
