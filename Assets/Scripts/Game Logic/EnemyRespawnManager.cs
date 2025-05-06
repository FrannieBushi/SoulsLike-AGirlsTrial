using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnManager : MonoBehaviour
{
    public static EnemyRespawnManager instance;

    public List<Enemy> enemies = new List<Enemy>();

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemy.isBoss && !enemies.Contains(enemy))
            enemies.Add(enemy);
    }

    public void RespawnEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (!enemy.isBoss)
                enemy.Respawn();
        }
    }
}
