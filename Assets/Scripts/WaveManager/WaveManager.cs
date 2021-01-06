using UnityEngine;
using UnityEngine.Assertions;
using System.Collections;
using System.Collections.Generic;


public class WaveManager : MonoBehaviour
{
    [SerializeField, Range(0.0f, 60.0f)] private float timeBeforeFirstWave = 2.0f;
    [SerializeField, Range(0.0f, 60.0f)] private float timeBetweenWaves = 5.0f;
    [SerializeField] private GameObject fogEffectPrefab = null;
    [SerializeField] private Wave[] waves = null;
    [SerializeField] private BoxCollider[] spawnAreas = null;

    private List<GameObject> spawnedFogEffects = new List<GameObject>();
    private int enemiesRemaining = 0;
    private int actualWaveIndex = 0;


    public static event System.Action OnNewWave;
    public static event System.Action OnGameWon;


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

        Assert.IsNotNull(fogEffectPrefab);

        Assert.IsTrue(waves.Length > 0);
        foreach (var wave in waves)
            Assert.IsNotNull(wave);

        Enemy.OnAnyEnemyDeath += OnAnyEnemyDeath;
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
            StartCoroutine(SpawnWave());
        }
        else if (enemiesRemaining <= 0 && waves.Length == actualWaveIndex)
        {
            OnGameWon?.Invoke();
        }
    }

    private IEnumerator SpawnWave()
    {
        foreach (var area in spawnAreas)
        {
            spawnedFogEffects.Add(Instantiate(fogEffectPrefab, area.transform.position, area.transform.rotation));
        }

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

        foreach (var spawnedFogEffect in spawnedFogEffects)
        {
            //Destroy(spawnedFogEffect);
        }
        spawnedFogEffects.Clear();

        ++actualWaveIndex;
    }

    private void SpawnOneEnemy(Enemy enemyType)
    {
        if(enemyType != null)
        {
            var spawnPosition = Vector3.zero;
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
        Enemy.OnAnyEnemyDeath -= OnAnyEnemyDeath;
    }

    private void OnAnyEnemyDeath(Enemy deadEnemy)
    {
        ReduceRemainingEnemies();
        CheckIfShouldSpawnNewWave();
    }
}