using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DungeonExplorer
{
    public class Monster : Creature
    {
        public int Speed { get; set; }

        public Monster(string name, int health, int strength, int speed) 
             : base(name, health, strength)
        {
            Speed = speed;
        }

        public override void TakeDamage(int damage)
        {
            Health -= damage;
        }
    }
}
