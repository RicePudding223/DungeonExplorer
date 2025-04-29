using System;
using System.Threading;

namespace DungeonExplorer
{
    /// <summary>
    /// Class to manage the combat system in the dungeon, handling player and enemy interactions.
    /// </summary>
    public class CombatManager
    {
        private readonly Player _player;
        private readonly UIManager _uiManager;

        // Constructor to initialize the CombatManager with the player and UIManager
        public CombatManager(Player player, UIManager uiManager)
        {
            _player = player;
            _uiManager = uiManager;
        }

        /// <summary>
        /// Starts a fight with an enemy in the current room.
        /// </summary>
        public void FightEnemy()
        {
            try
            {
                // If there are no enemies in the room, notify the player
                if (_player.CurrentRoom.Enemies.Count == 0)
                {
                    _uiManager.ShowMessage("There are no enemies to fight here.");
                    return;
                }

                // Prompt the player to select an enemy to fight
                int enemyChoice = _uiManager.ShowEnemySelection(_player.CurrentRoom.Enemies);
                if (enemyChoice == _player.CurrentRoom.Enemies.Count + 1) // If the player cancels
                {
                    return;
                }

                // Select the chosen enemy from the room
                Monster enemy = _player.CurrentRoom.Enemies[enemyChoice - 1];
                int round = 1;

                // Main combat loop: alternates between player actions and enemy actions
                while (true)
                {
                    // Clear the screen and display combat status
                    _uiManager.ClearScreen();
                    _uiManager.ShowCombatStatus(_player, enemy, round);

                    // Display combat menu and let the player choose an action
                    int action = _uiManager.ShowCombatMenu(_player, enemy);
                    switch (action)
                    {
                        case 1: // Attack action
                            Attack(enemy);
                            if (!enemy.CheckAlive() && enemy is Boss bossEnemy && !bossEnemy.secondPhase)
                            {
                                _uiManager.ShowMessage("Looks like the boss is preparing for a second phase!", true);
                                bossEnemy.SecondPhase();
                            }
                            else if (!enemy.CheckAlive()) // If the enemy is defeated
                            {
                                _player.CurrentRoom.Enemies.RemoveAt(enemyChoice - 1);
                                _uiManager.ShowMessage($"You defeated the {enemy.Name}!", false);
                                return;
                            }
                            break;

                        case 2: // Use item action
                            _player.UseItem(_uiManager);
                            break;

                        case 3: // Run action
                            _uiManager.ShowMessage("You ran away from the fight!", false);
                            return;
                    }

                    // Enemy attacks if still alive and based on its speed
                    if (enemy.CheckAlive() && round % enemy.Speed == 0)
                    {
                        EnemyAttack(enemy);
                        if (_player.CurrentHealth <= 0) // If the player is dead
                        {
                            _uiManager.ShowMessage("You have died!", true);
                            Game.IsGameOver = true;
                            return;
                        }
                    }

                    round++; // Increment round counter
                    _uiManager.WaitForInput();
                }
            }
            catch (Exception ex)
            {
                // Handle any unexpected errors gracefully
                _uiManager.ShowMessage($"An error occurred: {ex.Message}", true);
            }
        }

        /// <summary>
        /// Method to perform an attack on the enemy by the player.
        /// </summary>
        /// <param name="enemy"> The enemy being attacked.</param>
        public void Attack(Monster enemy)
        {
            Random random = new Random();
            double multiplier = Math.Round(random.NextDouble() + 1, 1); // Random multiplier for attack damage
            int playerDamage = (int)(_player.EquippedWeaponDamage + (_player.Strength * multiplier)); // Calculate damage

            enemy.TakeDamage(playerDamage); // Deal damage to the enemy
            _uiManager.ShowMessage($"You deal {playerDamage} damage to {enemy.Name}.", false); // Display message
        }

        /// <summary>
        /// Method for the enemy to attack the player.
        /// </summary>
        /// <param name="enemy"> The enemy performing the attack.</param>
        private void EnemyAttack(Monster enemy)
        {
            _player.TakeDamage(enemy.Strength); // Player takes damage from enemy
            _uiManager.ShowMessage($"{enemy.Name} deals {enemy.Strength} damage to you.", true); // Show damage message
        }
    }
}
