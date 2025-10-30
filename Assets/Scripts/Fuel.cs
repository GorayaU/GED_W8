using UnityEngine;
using System;
using System.Collections;
using TMPro;

public class Fuel : MonoBehaviour
{
    public event Action<int> OnFuelUsed;
    public event Action OnFuelUsedUp;
    
    [SerializeField] private TextMeshProUGUI fuelText;
    [SerializeField] private int startingFuel = 500;
    [SerializeField] private float burnRate = 1f;

    private int _fuel;

    private void Start()
    {
        _fuel = startingFuel;
        StartCoroutine(BurnFuel());
    }

    private IEnumerator BurnFuel()
    {
        var wait = new WaitForSeconds(burnRate);
        while (_fuel > 0)
        {
            yield return wait;
            Consume(1);
        }
    }

    public void Consume(int amount)
    {
        if (amount <= 0 || _fuel <= 0) return;

        int consumed = Mathf.Min(amount, _fuel);
        _fuel -= consumed;
        fuelText.text = "Fuel: " + _fuel;
        
        OnFuelUsed?.Invoke(consumed);

        if (_fuel == 0)
        {
            OnFuelUsedUp?.Invoke();
        }
        
    }
}