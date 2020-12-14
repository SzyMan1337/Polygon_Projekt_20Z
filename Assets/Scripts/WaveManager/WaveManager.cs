using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;


public class WaveManager : MonoBehaviour
{
    [SerializeField, Range(0.0f, 60.0f)] private float timeBeforeFirstWave = 2.0f;
    [SerializeField, Range(0.0f, 60.0f)] private float timeBetweenWaves = 5.0f;
    [SerializeField] private Wave[] waves = null;
    [SerializeField] private BoxCollider[] spawnAreas = null;

    private AudioSource audioSource;
    private const float SPAWN_HEIGHT = 1.0f;
    private int enemiesRemaining = 0;
    private int actualWaveIndex = 0;

    public static event System.Action OnNewWave;
    public static event System.Action OnWinningGame;

    private void Awake()
    {
        Assert.IsTrue(spawnAreas.Length > 0);
        var spawnAreasSet = new HashSet<BoxCollider>();
        foreach(var spawnArea in spawnAreas)
        {
            Assert.IsNotNull(spawnArea);
            Assert.IsFalse(spawnAreasSet.Contains(spawnArea));
            spawnAreasSet.Add(spawnArea);
        }

        Assert.IsTrue(waves.Length > 0);
        foreach (var wave in waves)
            Assert.IsNotNull(wave);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);

        Enemy.OnAnyEnemyDeath += ReduceRemainingEnemies;
        Enemy.OnAnyEnemyDeath += CheckIfShouldSpawnNewWave;
    }

    private void Start()
    {
        CheckIfShouldSpawnNewWave();
    }
       
    private void CheckIfShouldSpawnNewWave()
    {
        if (enemiesRemaining <= 0 && waves.Length > actualWaveIndex)
        {
            OnNewWave?.Invoke();
            audioSource.Play();
            StartCoroutine(SpawnWave());
        }
        else if (enemiesRemaining <= 0 && waves.Length == actualWaveIndex)
        {
            OnWinningGame?.Invoke();
        }
    }

    private IEnumerator SpawnWave()
    {
        if (actualWaveIndex != 0)
            yield return new WaitForSeconds(timeBetweenWaves);
        else
            yield return new WaitForSeconds(timeBeforeFirstWave);

        enemiesRemaining = waves[actualWaveIndex].NumberOfEnemiesToSpawn();

        while (waves[actualWaveIndex].NumberOfEnemiesToSpawn() > 0)
        {
            var enemyType = waves[actualWaveIndex].ChooseEnemyType();
            SpawnOneEnemy(enemyType);
            yield return new WaitForSeconds(waves[actualWaveIndex].TimeBetweenSpawns);
        }

        ++actualWaveIndex;
    }

    private void SpawnOneEnemy(Enemy enemyType)
    {
        if(enemyType != null)
        {
            var spawnPosition = new Vector3(0.0f, SPAWN_HEIGHT, 0.0f);
            var spawningArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
            spawnPosition.x = spawningArea.bounds.extents.x * Random.Range(-1.0f, 1.0f) + spawningArea.transform.position.x;
            spawnPosition.z = spawningArea.bounds.extents.z * Random.Range(-1.0f, 1.0f) + spawningArea.transform.position.z;
            Instantiate(enemyType, spawnPosition, Quaternion.identity);
        }
    }

    private void ReduceRemainingEnemies()
    {
        --enemiesRemaining;
    }

    private void OnDestroy()
    {
        Enemy.OnAnyEnemyDeath -= ReduceRemainingEnemies;
        Enemy.OnAnyEnemyDeath -= CheckIfShouldSpawnNewWave;
    }
}