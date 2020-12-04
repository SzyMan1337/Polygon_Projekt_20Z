using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;



[System.Serializable]
public class enemyType
{
    [SerializeField] Enemy prefab;
    [SerializeField] int numberToSpawn = 0;


    public int NumberToSpawn => numberToSpawn;
    public Enemy Prefab => prefab;
}


public class WaveManager : MonoBehaviour
{
    //[SerializeField] private Enemy enemyPrefab = null; //stare - pierwowzór
   // [SerializeField, Range(0.0f, 1000.0f)] private int numberOfEnemiesToSpawn = 0; // rowniez
    [SerializeField] enemyType[] enemiesToSpawn = new enemyType[3]; 
    [SerializeField] private BoxCollider[] spawnAreas;
    
 
    [SerializeField, Range(0.0f, 1000.0f)] private int numberOfShotgunsToSpawn = 0;
    private const float SPAWN_HEIGHT = 1.0f;


    private void Awake()
    {
        //Assert.IsNotNull(enemyPrefab); // piewowzor

        foreach (var enemy in enemiesToSpawn)
        {
            Assert.IsNotNull(enemy.Prefab);
        }
            Assert.IsTrue(spawnAreas.Length > 0);
        var spawnAreasSet = new HashSet<BoxCollider>();
        foreach(var spawnArea in spawnAreas)
        {
            Assert.IsNotNull(spawnArea);
            Assert.IsFalse(spawnAreasSet.Contains(spawnArea));
            spawnAreasSet.Add(spawnArea);
        }
    }

    private void Start()
    {
        var spawnPosition = new Vector3(0.0f, SPAWN_HEIGHT, 0.0f);

        foreach (var type in enemiesToSpawn)
        {
            var numberToSpawn = type.NumberToSpawn;
            while (numberToSpawn > 0)
            {
                var spawningArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
                spawnPosition.x = spawningArea.bounds.extents.x * Random.Range(-1.0f, 1.0f) + spawningArea.transform.position.x;
                spawnPosition.z = spawningArea.bounds.extents.z * Random.Range(-1.0f, 1.0f) + spawningArea.transform.position.z;
                var newEnemy = Instantiate(type.Prefab, spawnPosition, Quaternion.identity);
                --numberToSpawn;
            }
        }
    }
}
