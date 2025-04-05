using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DungeonExplorer
{
    public abstract class Creature
    {
        public string Name { get; set; }
        public int Health { get; set; }
        public int MaxHealth { get; protected set; }
        public int Strength { get; set; }

        protected Creature(string name, int health, int strength)
        {
            Name = name;
            Health = health;
            MaxHealth = health;
            Strength = strength;
        }

        public abstract void TakeDamage(int damage);

        public bool CheckAlive()
        {
            return Health > 0;
        }
    }
}
