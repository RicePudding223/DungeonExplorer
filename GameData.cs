using System;
using System.Collections.Generic;
using System.Linq;

namespace DungeonExplorer
{
    /// <summary>
    /// Class to hold the game data, such as weapons, potions, enemies, and room descriptions.
    /// </summary>
    class GameData
    {
        private static Random random = new Random();

        /// <summary>
        /// List holding different instances of the Weapon class.
        /// </summary>
        private static List<Weapon> weapons = new List<Weapon>()
            {
                new Weapon("Rusty Dagger", 5),
                new Weapon("Wooden Club", 8),
                new Weapon("Stone Axe", 12),
                new Weapon("Bronze Sword", 15),
                new Weapon("Iron Spear", 18),
                new Weapon("Steel Mace", 22),
                new Weapon("Silver Rapier", 25),
                new Weapon("Obsidian Knife", 28),
                new Weapon("Elven Bow", 30),
                new Weapon("Dwarven Warhammer", 35),
                new Weapon("Dark Steel Katana", 40),
                new Weapon("Dragonbone Greatsword", 45),
                new Weapon("Phoenix Fire Staff", 50),
                new Weapon("Elder Wand", 55),
                new Weapon("Godslayer Blade", 60)
            };


        /// <summary>
        /// List holding different instances of the Potion class.
        /// </summary>
        private static List<Potion> potions = new List<Potion>()
            {
                new Potion("Lesser Health Potion", 10),
                new Potion("Health Potion", 20),
                new Potion("Greater Health Potion", 30),
            };

        /// <summary>
        /// List holding different instances of the Monster class.
        /// Sets each monster's name, max health, strength, and speed.
        /// </summary>
        private static List<Monster> enemies = new List<Monster>()
            {
                new Monster("Goblin", 50, 10, 1),
                new Monster("Orc", 60, 20, 1),
                new Monster("Troll", 90, 30, 2),
                new Monster("Giant", 110, 40, 3),
                new Monster("Wizard", 100, 35, 2)
            };

        /// <summary>
        /// A list of room descriptions.
        /// </summary>
        private static List<string> roomDescriptions = new List<string>()
        {
            "A damp, moss-covered chamber with a faint dripping sound echoing from the walls.",
            "A narrow corridor lit by flickering torches, casting long shadows on the rough stone walls.",
            "A large hall with towering pillars, each carved with intricate runes that glow faintly in the dark.",
            "A circular room with a shattered altar in the center, surrounded by broken statues of ancient gods.",
            "A cold, dark cell with iron bars and a single, rusted chain hanging from the ceiling.",
            "A treasure vault filled with glittering gold coins, precious gems, and ancient artifacts.",
            "A room filled with strange, glowing fungi that emit an eerie, pulsating light.",
            "A chamber with a deep, dark pit in the center, surrounded by a narrow walkway.",
            "A library filled with dusty tomes and scrolls, some of which seem to move on their own.",
            "A room with walls covered in strange, pulsating veins that seem to breathe.",
            "A grand hall with a massive chandelier hanging from the ceiling, its candles still burning brightly.",
            "A room filled with the bones of long-dead adventurers, their equipment scattered among the remains.",
            "A chamber with a large, ornate mirror that reflects a distorted version of the room.",
            "A room with a bubbling cauldron in the center, emitting a foul-smelling smoke.",
            "A narrow passageway with walls that seem to close in as you walk further.",
            "A room with a massive, ancient tree growing in the center, its roots breaking through the stone floor.",
            "A chamber with a pool of still, black water that reflects nothing but darkness.",
            "A room with a massive, locked door covered in strange symbols and glowing runes.",
            "A chamber with a large, circular platform in the center, surrounded by a deep chasm.",
            "A room with walls covered in ancient murals depicting a long-forgotten battle.",
            "A chamber with a massive, glowing crystal in the center, pulsating with energy.",
            "A room filled with the sound of distant whispers, though no one is there.",
            "A chamber with a large, stone throne covered in cobwebs and dust.",
            "A room with a massive, iron gate that creaks open as you approach.",
        };

        /// <summary>
        /// Method to return a random weapon from the weapons list within a specific range.
        /// </summary>
        /// <param name="min"> The minimum index in the weapons dictionary.</param>
        /// <param name="max"> The maximum index in the weapons dictionary.</param>
        /// <returns> A weapon instance.</returns>
        public static Weapon GetRandomWeapon(int min, int max)
        {
            if (min >= weapons.Count || max >= weapons.Count)
            {
                min = weapons.Count - 1;
                max = weapons.Count;
            }
            int index = random.Next(min, max);
            return weapons.ElementAt(index);
        }

        /// <summary>
        /// Method to return a random potion from the potions list within a specific range.
        /// </summary>
        /// <param name="min"> The minimum index in the weapons dictionary.</param>
        /// <param name="max"> The maximum index in the weapons dictionary.</param>
        /// <returns> A potion instance.</returns>
        public static Potion GetRandomPotion(int min, int max)
        {
            if (min >= potions.Count || max >= potions.Count)
            {
                min = potions.Count - 1;
                max = potions.Count;
            }
            int index = random.Next(min, max);
            return potions.ElementAt(index);
        }

        /// <summary>
        /// Method to return a random enemy from the enemies list within a specific range.
        /// </summary>
        /// <param name="min"> The minimum index in the weapons dictionary.</param>
        /// <param name="max"> The maximum index in the weapons dictionary.</param>
        /// <returns> A new instance of the selected monster.</returns>
        public static Monster GetRandomEnemy(int min, int max)
        {
            if (min >= enemies.Count || max >= enemies.Count )
            {
                min = enemies.Count - 1;
                max = enemies.Count;
            }

            int index = random.Next(min, max);
            Monster template = enemies[index];
            return new Monster(template.Name, template.MaxHealth, template.Strength, template.Speed);
        }

        /// <summary>
        /// Method to return a random room description from the roomDescriptions list.
        /// </summary>
        /// <returns> A string that holds a random room description.</returns>
        public static string GetRandomRoomDescription()
        {
            int index = random.Next(roomDescriptions.Count);
            return roomDescriptions[index];
        }

        /// <summary>
        /// Methods to return the weapons and potions lists.
        /// </summary>
        /// <returns> A list of their respective objects.</returns>
        public static List<Weapon> GetWeapons() => weapons.ToList();
        public static List<Potion> GetPotions() => potions.ToList();
    }
}
