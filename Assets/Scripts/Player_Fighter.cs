using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player_Fighter : MonoBehaviour
{
    [Header("----- Player Stats -----")]
    public float moveSpeed = 5f; // Speed of the player movement
    public float projectileSpeed = 10f; // Speed of the projectile
    public float MaxHealth;

    [Header("----- Components -----")]
    public GameObject projectilePrefab; // Reference to your projectile prefab
    public Vector2 offset = new Vector2(1f, 1f); // Offset from the character
    public Texture2D cursorTexture; // Reference to your crosshair image
    public float HealthBarChipSpeed; // set to 2f
    public Image FrontHealthBar, BackHealthBar;

    [Header("-----Audio-----")]

    [SerializeField] AudioSource PlayerSounds;
    [SerializeField] AudioClip shoot;


    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private float health;
    private float lerpTimer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the Rigidbody2D component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);

        health = MaxHealth;
    }

    void Update()
    {
        Movement();
        fireProjectile();
        health = Mathf.Clamp(health, 0, MaxHealth); //prevents health from going above or below max and min values
        UpdateHealthUI();

        //for testing purposes
        if (Input.GetKeyDown(KeyCode.N)) 
        {
            TakeDamage(Random.Range(5, 10));
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            RestoreHealth(Random.Range(5, 10));
        }
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

    public void UpdateHealthUI()
    {
        float fillFront = FrontHealthBar.fillAmount;
        float fillBack = BackHealthBar.fillAmount;
        float healthFraction = health / MaxHealth; // Keeps the value between 0 and 1
        if(fillBack > healthFraction)
        {
            FrontHealthBar.fillAmount = healthFraction;
            BackHealthBar.color = Color.red;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / HealthBarChipSpeed;
            percentComplete = percentComplete * percentComplete; // starts the animation off slow then speeds up. IDK how. lol.
            BackHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if(fillFront < healthFraction)
        {
            BackHealthBar.color = Color.green;
            BackHealthBar.fillAmount = healthFraction;
            lerpTimer += Time.deltaTime;
            float percentComplete = lerpTimer / HealthBarChipSpeed;
            percentComplete = percentComplete * percentComplete; // starts the animation off slow then speeds up. IDK how. lol.
            FrontHealthBar.fillAmount = Mathf.Lerp(fillFront, BackHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        lerpTimer = 0f; // this has to do with the special effect on the hud
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        lerpTimer = 0f; // this has to do with the special effect on the hud
    }
}

