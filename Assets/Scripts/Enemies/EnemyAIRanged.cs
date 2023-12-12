using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIRanged : EnemyAIMelee
{
    public Enemy_Projectile projectilePrefab;
    public float ProjectileSpeed;
    public Transform projectileSpawnPoint;
    public Sprite ProjectileSprite;
    public float stopDistance;

    protected override void MoveTowardsPlayer()
    {
        float distanceToPlayer = Vector2.Distance(transform.position, playerTransform.position);
        Vector2 currentPosition = transform.position;
        Vector2 targetPosition = playerTransform.position;
        // Move towards the player if outside stopDistance
        if (distanceToPlayer > stopDistance)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, Speed * Time.deltaTime);
        }
        Vector2 movementDirection = targetPosition - currentPosition;
        if (movementDirection.x < 0)
        {
            sprite.flipX = false; // Flip the sprite when moving left
        }
        else if (movementDirection.x > 0)
        {
            sprite.flipX = true; // Unflip the sprite when moving right
        }
    }
    private bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, playerTransform.position) <= Range;
    }
    protected override void Attack()
    {
        // Ensure enough time has passed since the last attack
        if (Time.time < lastAttackTime + AttackCooldown)
        {
            return;
        }
        // Check if the target is still within range and line of sight, if necessary
        if (IsPlayerInRange())
        {
            Shoot();
        }
        lastAttackTime = Time.time; // Reset attack cooldown
        
    }

    private void Shoot()
    {
        Enemy_Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);
        // Calculate shooting direction towards the player
        Vector3 targetPos = playerTransform.position;
        targetPos.z = 0f; // Ensure it's on the same Z plane as your enemy
        Vector3 shootingDirection = (targetPos - transform.position).normalized;

        // Set the projectile properties
        newProjectile.SetProperties(Damage, ProjectileSpeed, shootingDirection, ProjectileSprite);
        newProjectile.SetLayer(9); // 9 is enemyprojectile layer. Update as needed.

        // Optionally rotate the projectile to align with the shooting direction
        float projectileAngle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        newProjectile.transform.rotation = Quaternion.Euler(0, 0, projectileAngle);
    }


}
