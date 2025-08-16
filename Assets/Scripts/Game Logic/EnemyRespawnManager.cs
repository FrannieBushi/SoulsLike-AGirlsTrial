using System.Collections.Generic;
using UnityEngine;

public class EnemyRespawnManager : MonoBehaviour
{
    private List<Enemy> enemies = new List<Enemy>();

    void Start()
    {
        
        Enemy[] sceneEnemies = Object.FindObjectsByType<Enemy>(FindObjectsSortMode.None);
        foreach (Enemy e in sceneEnemies)
        {
            RegisterEnemy(e);
        }
    }

    public void RegisterEnemy(Enemy enemy)
    {
        if (!enemy.isBoss && !enemies.Contains(enemy))
        {
            enemies.Add(enemy);
        }
    }

    public void RespawnEnemies()
    {
        foreach (var enemy in enemies)
        {
            if (enemy != null)
            {
                enemy.Respawn();
            }
        }
    }
}