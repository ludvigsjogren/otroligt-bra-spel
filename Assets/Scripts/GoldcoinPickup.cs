using System;
using UnityEngine;

public class GoldcoinPickup : MonoBehaviour
{
    [SerializeField] private GameObject coinParticleSystem;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerQuest>().AddGoldcoin();
            Instantiate(coinParticleSystem, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
    }
}
