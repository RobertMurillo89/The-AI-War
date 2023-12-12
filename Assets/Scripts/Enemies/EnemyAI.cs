using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour, IDamage, IEnemy
{

    [Header("--------EnemyStats-------")]
    public float Speed;
    public int Health;

    [Header("-----Attack-----")]
    public float Range;
    public int Damage;
    public float AttackCooldown;
    protected float lastAttackTime;

    [Header("-------Components-------")]
    public AudioSource EmoteSource, WeaponSource;

    protected Transform playerTransform;

    // Start is called before the first frame update
    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if(player != null)
        {
            playerTransform = player.transform;
        }
    }



    // Update is called once per frame
    protected virtual void Update()
    {
        if (playerTransform != null)
        {
            MoveTowardsPlayer();
            PerformAction();
        }
    }

    protected virtual void MoveTowardsPlayer()
    {
        // Move towards the player's position
        transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, Speed * Time.deltaTime);
    }
    
    protected virtual void PerformAction()
    {
        // Default implementation is empty
        // Subclasses like MeleeEnemy or RangedEnemy will override this
    }

    public void takeDamage(int amount)
    {
        Health -= amount;
        if (Health <= 0)
        {
            Destroy(gameObject);
            GameManager.Instance.DecrementEnemyCount();

        }
    }

    public void OnEnemySpawned()
    {
        GameManager.Instance.IncrementEnemyCount();
    }
}
