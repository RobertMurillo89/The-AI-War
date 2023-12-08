using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponPickup : MonoBehaviour
{
    [SerializeField] WeaponStats Weapon;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.PlayerScript.WeaponPickUp(Weapon);
            Destroy(gameObject);
        }
    }
}
