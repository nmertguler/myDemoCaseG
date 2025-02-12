using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int poolSize = 10;

    [SerializeField] bool levelLoadingReset;

    private Queue<GameObject> pool = new Queue<GameObject>();

    private void Awake()
    {
        // pooldaki nesneleri olustur
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab, transform);
            obj.name += i.ToString();
            obj.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    private void OnEnable()
    {
        EventManager.ActionLevelLoading += PoolReset;
    }
    private void OnDisable()
    {
        EventManager.ActionLevelLoading -= PoolReset;

    }

    void PoolReset()
    {
        
    }

    public GameObject GetFromPool()
    {
        if (pool.Count > 0)
        {
            GameObject obj = pool.Dequeue();

            while (pool.Count > 0 && obj is null)
            {
                obj = pool.Dequeue();
            }


            obj.SetActive(true);
            return obj;
        }
        else
        {
            // eger pooler bos ise yeni bir nesne olustur
            GameObject obj = Instantiate(prefab);
            obj.SetActive(true);
            poolSize++;
            return obj;
        }
    }

    public void ReturnToPool(GameObject obj)
    {
        if(obj.TryGetComponent<SoldierController>(out SoldierController soldierController))
        {
            soldierController.SoldierReset();
        }

        obj.transform.SetParent(transform);
        obj.SetActive(false);
        pool.Enqueue(obj);
    }
}
