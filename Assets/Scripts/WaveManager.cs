using UnityEngine;
using UnityEngine.Assertions;


public class WaveManager : MonoBehaviour
{
    [SerializeField] private Enemy enemyPrefab;
    [SerializeField] private int numberOfEnemiesToSpawn = 0;
    [SerializeField] private BoxCollider[] spawnAreas;
    [SerializeField] private Transform enemyMovementPoint;
    private const float SPAWN_HEIGHT = 1.0f;


    private void Awake()
    {
        Assert.IsNotNull(enemyPrefab); 
        Assert.IsTrue(spawnAreas.Length > 0);
        foreach(var spawnArea in spawnAreas)
        {
            Assert.IsNotNull(spawnArea);
        }
        //TODO: sprawdzanie duplikatów
    }

    private void Start()
    {
        var spawnPosition = new Vector3(0.0f, SPAWN_HEIGHT, 0.0f);

        while (numberOfEnemiesToSpawn > 0)
        {
            var spawningArea = spawnAreas[Random.Range(0, spawnAreas.Length)];
            spawnPosition.x = spawningArea.bounds.extents.x * Random.Range(-1.0f, 1.0f) + spawningArea.transform.position.x;
            spawnPosition.z = spawningArea.bounds.extents.z * Random.Range(-1.0f, 1.0f) + spawningArea.transform.position.z;
            var newEnemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            newEnemy.Destination = new Vector3(enemyMovementPoint.position.x, SPAWN_HEIGHT, enemyMovementPoint.position.z);
            Debug.Log("Enemy has spawned"); //TODO: do wywalenia potem
            --numberOfEnemiesToSpawn;
        }
    }
}
