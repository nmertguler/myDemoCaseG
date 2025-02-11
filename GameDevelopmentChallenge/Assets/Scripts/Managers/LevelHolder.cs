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
    
}
