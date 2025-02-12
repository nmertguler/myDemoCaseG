using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    [SerializeField] ObjectPool soldierPool;
    [SerializeField] ObjectPool arrowPool;
    [SerializeField] ObjectPool xpPool;

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

    public GameObject Arrow
    {
        get
        {
            return arrowPool.GetFromPool();
        }
        set
        {
            arrowPool.ReturnToPool(value);
        }
    }

    public GameObject Xp
    {
        get
        {
            return xpPool.GetFromPool();
        }
        set
        {
            xpPool.ReturnToPool(value);
        }
    }
}
