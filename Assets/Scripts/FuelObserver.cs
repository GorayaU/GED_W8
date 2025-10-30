using UnityEngine;

public class FuelObserver : MonoBehaviour
{
    [SerializeField] private Fuel fuelSource;        
    [SerializeField] private GameObject prefab;      
    
    private int _accumulator = 0;
    private void OnEnable()
    {
        if (fuelSource != null) fuelSource.OnFuelUsed += HandleFuelUsed;
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

    private void Spawn()
    {
        
        Vector3 pos = new Vector3(Random.Range(-3, 3), 7.5f, -0.7f);
        GameObject spawned = Instantiate(prefab, pos, Quaternion.identity);
        
        Destroy(spawned, 3f);
    }
}