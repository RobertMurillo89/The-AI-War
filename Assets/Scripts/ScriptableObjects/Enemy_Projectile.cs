using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Projectile : MonoBehaviour
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
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            
            IDamage damageable = collision.gameObject.GetComponent<IDamage>();
            if (damageable != null)
            {
                
                damageable.takeDamage(Damage);
            }

            // Optionally add an impact effect or sound here

            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
