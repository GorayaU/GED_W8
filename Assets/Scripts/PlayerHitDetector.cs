using UnityEngine;
using System;

public class PlayerHitDetector : MonoBehaviour
{
    public event Action OnHitEnemy;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Enemy"))
            OnHitEnemy?.Invoke();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
            OnHitEnemy?.Invoke();
    }
}