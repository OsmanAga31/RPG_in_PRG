// This script manages the fight system in the game. It handles fight initiation, 
// fight logic, and cleanup after the fight ends.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class FightManager : MonoBehaviour
{
    // Singleton instance of the FightManager to ensure only one instance exists.
    public static FightManager Instance { get; private set; }

    // The chance (in percentage) for a fight to occur when checked.
    [Range(0, 100), SerializeField] private int chanceToEncounter;

    // Reference to the fight UI canvas that will be displayed during a fight.
    [SerializeField] GameObject fightCanvas;
    [SerializeField] Image fightBackgroundSprite;
    [SerializeField] AudioSource fightMusic;

    // Tracks whether a fight is currently active.
    private bool isFightActive;

    // Reference to the player's character controller.
    private BaseCharacterController characterController;

    private List<BattleCharacter> spawnedEnemies;
    private List<BattleCharacter> spawnedCharacters;

    // Buttons for the fight UI.
    [SerializeField] Button attackButton;
    [SerializeField] Button skillButton;
    [SerializeField] Button itemButton;
    [SerializeField] Button fleeButton;

    // Called when the script is initialized. Ensures the Singleton pattern is enforced.
    void Start()
    {
        if (Instance == null)
        {
            // Set this instance as the Singleton instance.
            Instance = this;
        }
        else if (Instance != this)
        {
            // Destroy duplicate instances of the FightManager.
            Destroy(gameObject);
        }

        // Initialize the fight state as inactive.
        isFightActive = false;
        spawnedCharacters = new List<BattleCharacter>();
        spawnedEnemies = new List<BattleCharacter>();
    }

    // Checks if a fight should start based on the encounter chance.
    public bool CheckForEncounter(BaseCharacterController characterController)
    {
        // Store the reference to the player's character controller.
        this.characterController = characterController;

        // Generate a random number and compare it to the chanceToEncounter.
        if (Random.Range(0, 100) < chanceToEncounter)
        {
            // If the random number is less than the chance, start the fight coroutine.
            StartCoroutine(FightCoroutine());
        }

        // Return whether a fight is currently active.
        return isFightActive;
    }

    // Coroutine that handles the fight logic.
    private IEnumerator FightCoroutine()
    {
        // Set the fight state to active.
        isFightActive = true;

        // Enable the fight UI canvas.
        fightCanvas.SetActive(isFightActive);

        // Load the player's characters into the fight.
        LoadCharacter();

        // Load Random Enemies
        SpawnRandomEnemy();
        // Load BackgroundImages
        LoadBackground();

        // Load Music
        LoadBattleMusic();
        // Load UI
        // Load Items

        /* Example of a transition phase:
         * while(transition){
         *     DoStuff();
         *     yield return new WaitForEndOfFrame();
         * }
         */

        // Main fight loop. Runs as long as the fight is active.
        while (isFightActive)
        {
            // Placeholder for fight logic:
            // - Determine whose turn it is.
            // - Execute player/enemy actions.
            // - Check for fight end conditions (e.g., player or enemy defeat).

            // Wait for 3 seconds before the next iteration (placeholder logic).
            yield return new WaitForSeconds(3f);

            // End the fight
            var battleOverType = CheckForEndFight();
            isFightActive = battleOverType == BattleEntityType.None; // Fight ends here for now.
        }

        // After the fight ends:
        // - Grant rewards like XP and gold.
        // - Check for level-ups.
        // - Save progress in the StatsManager.
        // - Clean up all battle-related assets.
        UnloadFightUI();

        // Disable the fight UI canvas.
        fightCanvas.SetActive(isFightActive);

        // Resume player movement or other gameplay mechanics.
        characterController.PausePlayer(isFightActive);
    }

    // Loads the player's characters into the fight.
    private void LoadCharacter()
    {
        foreach (var character in CharacterStatsManager.Instance.characterData)
        {
            // Load the character's prefab into the fight scene.
            spawnedCharacters.Add(SpawnManager.instance.SpawnBattleEntity(character));
        }
    }

    private void SpawnRandomEnemy()
    {
        List<int> enemyLevels = new List<int>();
        List<Health> enemyHealth = new List<Health>();
        var enemies = FindObjectOfType<SceneFightDataHolder>().GetBattleEnemies(out enemyLevels, out enemyHealth);

        for (int i = 0; i < enemies.Count; i++)
        {
            spawnedEnemies.Add(SpawnManager.instance.SpawnBattleEntity(enemies[i], enemyLevels[i], enemyHealth[i]));
        }

    }

    private BattleEntityType CheckForEndFight()
    {
        // Check if all enemies are dead using a lambda expression.
        bool allEnemiesDead = spawnedEnemies.TrueForAll(enemy => enemy.isCharacterDeath);

        // Check if all players are dead using a lambda expression.
        bool allPlayersDead = spawnedCharacters.TrueForAll(character => character.isCharacterDeath);

        // The fight continues as long as not all players or all enemies are dead.
        if(allEnemiesDead)
            return BattleEntityType.Enemy;
        if(allPlayersDead)
            return BattleEntityType.Player;
        return BattleEntityType.None;
    }

    private void UnloadFightUI()
    {
        spawnedCharacters.Clear();
        spawnedEnemies.Clear();
        SpawnManager.instance.Unload();
    }

    private void LoadBackground()
    {
        // Get a random background sprite from the FightBackgroundDataHolder.
        var backgroundSprite = FindObjectOfType<SceneFightDataHolder>().GetFightBackgroundSprite();

        // Set the background image in the fight canvas.
        if (backgroundSprite == null) fightBackgroundSprite.color = new Color(0, 0, 0, 0);
        else 
        { 
            fightBackgroundSprite.color = new Color(.7f, .7f, .7f, 1); //<-- Set the color to gray
            fightBackgroundSprite.sprite = backgroundSprite;
        }
    }

    private void LoadBattleMusic()
    {
        fightMusic.clip = FindObjectOfType<SceneFightDataHolder>().GetBattleMusic();
        fightMusic.Play();
    }

    // Called when the player presses the attack button.
    public void OnAttackButtonClicked()
    {
        // Add logic here for when the attack button is clicked.
        Debug.Log("Player attacks!");

        // Reduce the health of a random enemy for demonstration purposes.
        if (spawnedEnemies.Count > 0)
        {
            int randomEnemyIndex = Random.Range(0, spawnedEnemies.Count);
            BattleCharacter enemy = spawnedEnemies[randomEnemyIndex];
            enemy.TakeDamage(10); // Assuming 10 damage for demonstration
            Debug.Log($"Enemy health: {enemy.health.health}");
        }
        RemoveDefeatedEnemies();
    }

    // Called when the player presses the skill button.
    public void OnSkillButtonClicked()
    {
        // Add logic here for when the skill button is clicked.
        Debug.Log("Player uses a skill!");
    }

    // Called when the player presses the item button.
    public void OnItemButtonClicked()
    {
        // Add logic here for when the item button is clicked.
        Debug.Log("Player uses an item!");
    }

    // Called when the player presses the flee button.
    public void OnFleeButtonClicked()
    {
        // Add logic here for when the flee button is clicked.
        Debug.Log("Player tries to flee!");
        isFightActive = false; // End the fight
    }

    public void RemoveDefeatedEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--) // Rückwärts iterieren, um sicher zu entfernen
        {
            Health enemyHealth = spawnedEnemies[i].health;
            if (enemyHealth.health <= 0)
            {
                Destroy(spawnedEnemies[i]); // Entfernt das GameObject aus der Szene
                spawnedEnemies.RemoveAt(i); // Entfernt den Gegner aus der Liste
            }
        }
    }

}
