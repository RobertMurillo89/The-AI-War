using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : Projectile
{
    protected override void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.CompareTag("Player"))
        {

            IDamage damageable = hitInfo.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(Damage);
            }

            Destroy(gameObject);
        }
    }
}
