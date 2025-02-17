using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    public int levelCount;

    [Space(5)]
    public TowerController playerTower;
    public TowerController enemyTower1;
    public TowerController enemyTower2;

    public static LevelHolder Instance;
    private void Awake()
    {
        Instance = this;
    }

    public Vector3 RandomEnemyPos(TowerController tower)
    {
        Vector3 randEnemyPos = new Vector3(-10,0,0);

        if(tower == enemyTower1)
        {
            if(Random.value > .5F)
            {
                randEnemyPos= enemyTower2.transform.position;
            }
            else
            {
                randEnemyPos = playerTower.transform.position;
            }
        }

        if(tower == enemyTower2)
        {
            if (Random.value > .5F)
            {
                randEnemyPos = enemyTower1.transform.position;
            }
            else
            {
                randEnemyPos = playerTower.transform.position;
            }
        }

        if(tower == playerTower)
        {
            // player tower shouldnt oto attack
        }

        return randEnemyPos;
    }
    
}
