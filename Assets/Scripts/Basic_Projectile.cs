using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Projectile : MonoBehaviour
{
    public float DestroyTime;
    private void Start()
    {
        Destroy(gameObject, DestroyTime);
    }
    [SerializeField] int damage;
    private void OnTriggerEnter(Collider other)
    {
     
        if (other.isTrigger) return;

        //if (other.gameObject.layer == 0)
        //{
        //    Destroy(gameObject);
        //}

        IDamage damagable = other.GetComponent<IDamage>();

        if (damagable != null)
        {
            damagable.takeDamage(damage);
        }
        Destroy(gameObject);
    }
}
