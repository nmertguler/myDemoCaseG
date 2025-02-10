using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    [SerializeField] ObjectPool soldierPool;

    public static PoolController Instance;
    private void Awake()
    {
        Instance = this;
    }

    public GameObject Soldier
    {
        get
        {
            return soldierPool.GetFromPool();
        }
        set
        {
            soldierPool.ReturnToPool(value);
        }
    }
}
