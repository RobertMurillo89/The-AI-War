using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Fighter : MonoBehaviour
{
    public float moveSpeed = 5f; // Speed of the player movement

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public GameObject projectilePrefab; // Reference to your projectile prefab
    public float projectileSpeed = 10f; // Speed of the projectile
    public Vector2 offset = new Vector2(1f, 1f); // Offset from the character
    public Texture2D cursorTexture; // Reference to your crosshair image
    [SerializeField] AudioSource PlayerSounds;
    [SerializeField] AudioClip shoot;
     


    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
    }

    void Update()
    {
        Movement();
        fireProjectile();
    }
    void Movement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Calculate the movement direction
        Vector2 movement = new Vector2(moveHorizontal, moveVertical) * moveSpeed * Time.deltaTime;

        // Apply movement to the rigidbody
        rb.MovePosition(rb.position + movement);

        // Flip the sprite based on movement direction
        if (moveHorizontal < 0)
        {
            spriteRenderer.flipX = true; // Flip the sprite when moving left
        }
        else if (moveHorizontal > 0)
        {
            spriteRenderer.flipX = false; // Unflip the sprite when moving right
        }
    }
    void fireProjectile()
    {
        if (Input.GetButtonDown("Fire1")) // Change to your preferred input
        {
            PlayerSounds.PlayOneShot(shoot);
            Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0f; // Ensure it's on the same Z plane as your character

            // Apply offset
            targetPos += new Vector3(offset.x, offset.y, 0f);

            Vector3 shootingDirection = (targetPos - transform.position).normalized;

            GameObject newProjectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = newProjectile.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = shootingDirection * projectileSpeed;
            }
        }
    }
}

