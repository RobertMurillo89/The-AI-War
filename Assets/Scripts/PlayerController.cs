using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [Header("----- Player Stats -----")]
    public float moveSpeed; // Speed of the player movement
    public float projectileSpeed; // Speed of the projectile
    public float MaxHealth;

    [Header("----- Components -----")]
    public GameObject projectilePrefab; // Reference to your projectile prefab
    public Vector2 offset = new Vector2(1f, 1f); // Offset from the character
    public Texture2D cursorTexture; // Reference to your crosshair image
    public float HealthBarChipSpeed; // set to 2f
    public Image FrontHealthBar, BackHealthBar;
    public Rigidbody2D rb2D;
    public Transform WeaponHolder;
    public SpriteRenderer spriteRenderer;

    [Header("-----Audio-----")]

    [SerializeField] AudioSource PlayerSounds;
    [SerializeField] AudioClip shoot;


    private Vector2 moveInput;
    private float health;
    private float healthLerpTimer;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        health = MaxHealth;
    }

    void Update()
    {

        Movement();
        UseWeapon();
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
        //get input from the player
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        moveInput.Normalize(); // this prevents the vectors from stacking when moving diagonally. 

        //move player
        rb2D.velocity = moveInput * moveSpeed;

        health = Mathf.Clamp(health, 0, MaxHealth); //prevents health from going above or below max and min values
        UpdateHealthUI();

        // Flip the sprite based on movement direction
        if (moveInput.y < 0)
        {
            spriteRenderer.flipX = true; // Flip the sprite when moving left
        }
        else if (moveInput.x > 0)
        {
            spriteRenderer.flipX = false; // Unflip the sprite when moving right
        }
    }

    void UseWeapon()
    {
        // Define mouse position early
        Vector3 mousePos = Input.mousePosition;
        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);

        // Flip weapon if necessary
        if (mousePos.x < screenPoint.x)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            WeaponHolder.localScale = new Vector3(-1f, -1f, 1f);
        }
        else
        {
            transform.localScale = Vector3.one;
            WeaponHolder.localScale = Vector3.one;
        }

        // Rotate weapon holder
        Vector2 offset = new Vector2(mousePos.x - screenPoint.x, mousePos.y - screenPoint.y);
        float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        WeaponHolder.rotation = Quaternion.Euler(0, 0, angle);

        // Handle firing
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
        if (fillBack > healthFraction)
        {
            FrontHealthBar.fillAmount = healthFraction;
            BackHealthBar.color = Color.red;
            healthLerpTimer += Time.deltaTime;
            float percentComplete = healthLerpTimer / HealthBarChipSpeed;
            percentComplete = percentComplete * percentComplete; // starts the animation off slow then speeds up. IDK how. lol.
            BackHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            BackHealthBar.color = Color.green;
            BackHealthBar.fillAmount = healthFraction;
            healthLerpTimer += Time.deltaTime;
            float percentComplete = healthLerpTimer / HealthBarChipSpeed;
            percentComplete = percentComplete * percentComplete; // starts the animation off slow then speeds up. IDK how. lol.
            FrontHealthBar.fillAmount = Mathf.Lerp(fillFront, BackHealthBar.fillAmount, percentComplete);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthLerpTimer = 0f; // this has to do with the special effect on the hud
    }

    public void RestoreHealth(float amount)
    {
        health += amount;
        healthLerpTimer = 0f; // this has to do with the special effect on the hud
    }
}



