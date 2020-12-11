using UnityEngine;
using UnityEngine.Assertions;
using System.Collections.Generic;


public class Wave : MonoBehaviour
{
    [System.Serializable]
    private class EnemyTypeAndAmount
    {
        [SerializeField] private Enemy enemyPrefab = null;
        [SerializeField, Range(0, 1000)] private int amount = 0;

        public Enemy EnemyPrefab => enemyPrefab;
        public int Amount => amount;

        public void VariablesCheck()
        {
            Assert.IsNotNull(enemyPrefab);
            Assert.IsTrue(amount > 0);
        }

        public void ReduceAmount()
        {
            --amount;
        }
    }


    [SerializeField, Range(0.0f, 10.0f)] private float timeBetweenSpawns;
    [SerializeField] private List<EnemyTypeAndAmount> enemies;

    public float TimeBetweenSpawns => timeBetweenSpawns;


    private void Awake()
    {
        Assert.IsTrue(enemies.Count > 0);
        foreach (var enemy in enemies)
        {
            Assert.IsNotNull(enemy);
            enemy.VariablesCheck();
        }
    }

    public Enemy ChooseEnemyType()
    {
        if(enemies.Count > 0)
        {
            int indexOfEnemy = Random.Range(0, enemies.Count);
            var enemy = enemies[indexOfEnemy];
            var enemyType = enemy.EnemyPrefab;
            enemy.ReduceAmount();
            if (enemy.Amount <= 0)
                enemies.RemoveAt(indexOfEnemy);
            return enemyType;
        }

        return null;
    }

    public int NumberOfEnemiesToSpawn()
    {
        int sum = 0;
        foreach(var enemy in enemies)
        {
            sum += enemy.Amount;
        }
        return sum;
    }
}