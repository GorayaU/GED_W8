using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FuelObserver : MonoBehaviour
{
    [SerializeField] private Fuel fuelSource;    
    
    [SerializeField] private GameObject prefab;
    private const int InstantiateCount = 10;

    private readonly List<GameObject> _objectPool = new List<GameObject>();
    
    private int _accumulator;
    private void OnEnable()
    {
        if (fuelSource != null) fuelSource.OnFuelUsed += HandleFuelUsed;
        
        for (int i = 0; i < InstantiateCount; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            _objectPool.Add(obj);
        }
    }
    private void OnDisable()
    {
        if (fuelSource != null) fuelSource.OnFuelUsed -= HandleFuelUsed;
    }
    private void HandleFuelUsed(int amount)
    {
        _accumulator += amount;
        
        while (_accumulator >= 2)
        {
            Spawn();
            _accumulator -= 2;
        }
    }


    // ReSharper disable Unity.PerformanceAnalysis
    [Obsolete("Obsolete")]
    private GameObject GetFromPool(Vector3 position, Quaternion rotation)
    {
        foreach (GameObject obj in _objectPool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.SetPositionAndRotation(position, rotation);

                Rigidbody rb = obj.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                }

                Rigidbody2D rb2d = obj.GetComponent<Rigidbody2D>();
                if (rb2d != null)
                {
                    rb2d.velocity = Vector2.zero;
                    rb2d.angularVelocity = 0f;
                }

                obj.SetActive(true);
                return obj;
            }
        }
        return null;
    }

    private void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
    }
    [Obsolete("Obsolete")]
    private void Spawn()
    {
        Vector3 pos = new Vector3(Random.Range(-3f, 3f), 7.5f, -0.7f);

        GameObject spawned = GetFromPool(pos, Quaternion.identity);
        if (!spawned) return;

        StartCoroutine(DespawnAfterTime(spawned, 3f));
    }
    
    IEnumerator DespawnAfterTime(GameObject obj, float time)
    {
        yield return new WaitForSeconds(time);

        ReturnToPool(obj);
    }
}