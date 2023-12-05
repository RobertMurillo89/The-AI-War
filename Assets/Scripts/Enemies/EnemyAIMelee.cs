using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIMelee : EnemyAI
{
    protected override void PerformAction()
    {
        if (IsPlayerInRange())
        {
            Attack();
        }
    }

    private bool IsPlayerInRange()
    {
        return Vector2.Distance(transform.position, playerTransform.position) <= Range;
    }

    protected virtual void Attack()
    {
        // Ensure enough time has passed since the last attack
        if (Time.time < lastAttackTime + AttackCooldown)
        {
            return;
        }

        // Check if the target is still within range and line of sight, if necessary
        if (IsPlayerInRange())
        {
            // Perform the attack
            IDamage target = playerTransform.GetComponent<IDamage>();
            if (target != null)
            {
                target.takeDamage(Damage);
            }
        }
        lastAttackTime = Time.time; // Reset attack cooldown
    }
}
