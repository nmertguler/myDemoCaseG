using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelHolder : MonoBehaviour
{
    public int levelCount;

    public static LevelHolder Instance;
    private void Awake()
    {
        Instance = this;
    }
    
}
