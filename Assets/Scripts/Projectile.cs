using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int Damage;
    public float Speed;
    public Rigidbody2D rb;
    public SpriteRenderer spriteRenderer;



    public void SetProperties(int damage, float speed, Vector3 direction, Sprite skin)
    {
        this.Damage = damage;
        this.Speed = speed;
        spriteRenderer.sprite = skin;

        rb.velocity = direction * speed;
    }

    public void SetLayer(int layer)
    {
        gameObject.layer = layer;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {


        if (hitInfo.gameObject.CompareTag("Enemy"))
        {

            IDamage damageable = hitInfo.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(Damage);
            }

            Destroy(gameObject);
        }


    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
