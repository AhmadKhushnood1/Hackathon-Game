using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab; // Reference to the enemy prefab
    public Transform SpawnPoint; // Location enemies will spawn
    public int EnemiesPerRound = 5; // How many enemies to spawn per round
    public float delayBetweenSpawn = 2f; // The time between each enemy spawn
    public int delayBetweenRound = 15; // Time to wait before spawning again
    public int enemiesSpawned; // Number of enemies spawned
    public RuntimeAnimatorController animatorController;  // Reference to the Animator Controller

    private void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    private IEnumerator SpawnEnemies()
    {
        enemiesSpawned = 0;
        while (true)
        {
            if (enemiesSpawned == EnemiesPerRound)
            {
                break;
            }
            yield return new WaitForSeconds(delayBetweenRound); // Pauses for a certain number of time
            //Spawning enemies
            SpawnEnemy();
            enemiesSpawned++;
            yield return new WaitForSeconds(delayBetweenSpawn); // Pauses for a certain number of time
        }
    }
    private void SpawnEnemy()
    {
        GameObject enemyClone = Instantiate(enemyPrefab, SpawnPoint.position, quaternion.identity); // Spawns enemy at the spawn point

        // Ensure Animator is attached to the enemy clones
        Animator animator = enemyClone.GetComponent<Animator>();
        if (animator == null)
        {
            animator = enemyClone.AddComponent<Animator>();  // Add the Animator component if not found
        }
        // Check if the RuntimeAnimatorController is assigned
        if (animatorController != null)
        {
            animator.runtimeAnimatorController = animatorController;  // Assign the RuntimeAnimatorController
        }

    }

}
