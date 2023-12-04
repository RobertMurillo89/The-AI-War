using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Enemy : MonoBehaviour, IDamage
{
    public float moveSpeed = 5f; // Speed of the enemy movement
    private Transform player; // Reference to the player's transform
    [SerializeField] int HP;

    void Start()
    {
        // Find the player GameObject (Assuming the player has a "Player" tag)
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the direction towards the player
            Vector3 direction = (player.position - transform.position).normalized;

            // Move towards the player
            transform.Translate(direction * moveSpeed * Time.deltaTime);
        }
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        if (HP > 0)
        {
            Destroy(gameObject);
        }
    }
        private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerProjectile")) playerInRange = true;
    }
}

