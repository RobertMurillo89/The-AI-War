using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour, IDamage
{
    [Header("----- Player Stats -----")]
    [SerializeField] float currentMovementSpeed;
    public float MoveSpeed; // Speed of the player movement
    [SerializeField] float currentHealth;
    public float MaxHealth;
    [SerializeField] float currentStamina;
    public float MaxStamina;
    public float StaminaDrainRate;
    public float StaminaRegenRate;
    public float SprintSpeedMultiplier;
    public float IdleTimeForStaminaRegen;

    [Header("-----Weapon Stats-----")]
    public PlayerInventory inventory;
    private WeaponStats CurrentWeapon;
    public WeaponStats DefaultWeapon;
    public float AttackRate;
    public int Damage;
    private float nextFireTime;
    public int selectedWeapon;
    public float ProjectileSpeed;
    public Sprite WeaponSprite;
    public Sprite ProjectileSprite;
    public Transform projectileSpawnPoint;
    public Projectile projectilePrefab; // Reference to your projectile prefab



    [Header("----- Components -----")]
    public Vector2 offset = new Vector2(1f, 1f); // Offset from the character
    public Texture2D cursorTexture; // Reference to your crosshair image
    public float HealthStamBarChipSpeed; // set to 2f
    public Image FrontHealthBar, BackHealthBar, FrontStaminaBar, BackStaminaBar;
    public Rigidbody2D rb2D;
    public Transform WeaponHolder;
    public Transform EquipedWeapon;
    public SpriteRenderer spriteRenderer;

    [Header("-----Audio-----")]

    [SerializeField] AudioSource PlayerSounds;
    [SerializeField] AudioClip AttackSound;
    [SerializeField] AudioClip ReloadSound;


    private Vector2 moveInput;
    private float healthLerpTimer;
    private float stamLerpTimer;
    private bool isSprinting;
    private float idleTimer;

    void Start()
    {
        Cursor.SetCursor(cursorTexture, Vector2.zero, CursorMode.Auto);
        currentHealth = MaxHealth;
        currentStamina = MaxStamina;

        if (CurrentWeapon == null)
        {
            WeaponPickUp(DefaultWeapon);

        }
    }

    void Update()
    {
        UpdateHealthUI();
        Movement();
        UseWeapon();
        //for testing purposes
        if (Input.GetKeyDown(KeyCode.N))
        {
            takeDamage(Random.Range(5, 10));
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

        // Check if the player is moving to activate sprint
        if (moveInput != Vector2.zero)
        {
            isSprinting = currentStamina > 0;
            idleTimer = 0; // Reset idle timer as player is moving
        }
        else
        {
            isSprinting = false; // Stop sprinting when not moving
            idleTimer += Time.deltaTime; // Increment idle timer when player is not moving
        }

        // Modify speed if sprinting
        float speed = isSprinting ? MoveSpeed * SprintSpeedMultiplier : MoveSpeed;
        rb2D.velocity = moveInput * speed;
        currentMovementSpeed = speed;

        HandleStamina();

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

    void HandleStamina()
    {
        if (isSprinting)
        {
            currentStamina -= StaminaDrainRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);
            stamLerpTimer = 0; // Reset lerp timer for stamina UI chipping
        }
        else if ( !isSprinting && idleTimer >= IdleTimeForStaminaRegen)
        {
            currentStamina += StaminaRegenRate * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, MaxStamina);
            stamLerpTimer = 0; // Reset lerp timer for stamina UI chipping
        }

        UpdateStaminaUI();
    }


    void UseWeapon()
    {
        if (CurrentWeapon == null) return;
        HandleWeaponOrientation();
        switch (CurrentWeapon.weaponType)
        {
            case WeaponStats.WeaponType.Melee:
                break;
            case WeaponStats.WeaponType.Ranged:

                if (Input.GetButton("Fire1") && Time.time >= nextFireTime)
                {
                    Shoot(); // Fire ranged weapon
                }

                break;
            case WeaponStats.WeaponType.MeleeWithAmmo: 
                break;
            case WeaponStats.WeaponType.Grenadier: 
                break;
        }
 
    }
    void HandleWeaponOrientation()
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
    }
    void Shoot()
    {
        nextFireTime = Time.time + 1f / AttackRate;
        PlayerSounds.PlayOneShot(AttackSound);

        // Instantiate the projectile
        Projectile newProjectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, Quaternion.identity);

        // Calculate shooting direction
        Vector3 targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        targetPos.z = 0f; // Ensure it's on the same Z plane as your character
        Vector3 shootingDirection = (targetPos - transform.position).normalized;
        // Set the projectile properties
        newProjectile.SetProperties(Damage, ProjectileSpeed, shootingDirection, ProjectileSprite);
        newProjectile.SetLayer(7); // 7 is playerprojectile layer. If this is changed this needs to be updated. 
                                   // Apply offset
        targetPos += new Vector3(offset.x, offset.y, 0f);

        // Optionally rotate the projectile to align with the shooting direction
        float projectileAngle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        newProjectile.transform.rotation = Quaternion.Euler(0, 0, projectileAngle);

    }
    public void UpdateHealthUI()
    {

        currentHealth = Mathf.Clamp(currentHealth, 0, MaxHealth); //prevents health from going above or below max and min values
        float fillFront = FrontHealthBar.fillAmount;
        float fillBack = BackHealthBar.fillAmount;
        float healthFraction = currentHealth / MaxHealth; // Keeps the value between 0 and 1
        if (fillBack > healthFraction)
        {
            FrontHealthBar.fillAmount = healthFraction;
            BackHealthBar.color = Color.red;
            healthLerpTimer += Time.deltaTime;
            float percentComplete = healthLerpTimer / HealthStamBarChipSpeed;
            percentComplete = percentComplete * percentComplete; // starts the animation off slow then speeds up. IDK how. lol.
            BackHealthBar.fillAmount = Mathf.Lerp(fillBack, healthFraction, percentComplete);
        }

        if (fillFront < healthFraction)
        {
            BackHealthBar.color = Color.green;
            BackHealthBar.fillAmount = healthFraction;
            healthLerpTimer += Time.deltaTime;
            float percentComplete = healthLerpTimer / HealthStamBarChipSpeed;
            percentComplete = percentComplete * percentComplete; // starts the animation off slow then speeds up. IDK how. lol.
            FrontHealthBar.fillAmount = Mathf.Lerp(fillFront, BackHealthBar.fillAmount, percentComplete);
        }
    }
    public void UpdateStaminaUI()
    {
        //stamina = Mathf.Clamp(stamina, 0, MaxStamina); //prevents health from going above or below max and min values
        float fillFront = FrontStaminaBar.fillAmount;
        float fillBack = BackStaminaBar.fillAmount;
        float staminaFraction = currentStamina / MaxStamina; // Keeps the value between 0 and 1

        if (fillBack > staminaFraction)
        {
            FrontStaminaBar.fillAmount = staminaFraction;
            BackStaminaBar.color = Color.red;
            stamLerpTimer += Time.deltaTime;
            float percentComplete = stamLerpTimer / HealthStamBarChipSpeed;
            percentComplete = percentComplete * percentComplete; // starts the animation off slow then speeds up. IDK how. lol.
            BackStaminaBar.fillAmount = Mathf.Lerp(fillBack, staminaFraction, percentComplete);
        }

        if (fillFront < staminaFraction)
        {
            // Directly fill up the stamina bar when increasing (no lerp)
            BackStaminaBar.color = Color.green;
            BackStaminaBar.fillAmount = staminaFraction;
            FrontStaminaBar.fillAmount = staminaFraction;
            stamLerpTimer = 0; // Reset the lerp timer
        }
    }

    public void RestoreHealth(float amount)
    {
        currentHealth += amount;
        healthLerpTimer = 0f; // this has to do with the special effect on the hud
    }

    public void takeDamage(int amount)
    {
        currentHealth -= amount;
        if(currentHealth <= 0)
        {
            GameManager.Instance.LoseGame();
        }
        healthLerpTimer = 0f; // this has to do with the special effect on the hud
    }

    public void WeaponPickUp(WeaponStats weapon)
    {
        inventory.AddItem(weapon);
        CurrentWeapon = weapon;
        Damage = weapon.AttackDamage;
        AttackRate = weapon.AttackRate;
        PlayerSounds.clip = weapon.AttackSound;
        WeaponSprite = weapon.WeaponModel;
        ProjectileSpeed = weapon.ProjectileSpeed;
        ProjectileSprite = weapon.projectileSprite;
        projectilePrefab = weapon.ProjectilePrefab;

        EquipedWeapon.GetComponent<SpriteRenderer>().sprite = weapon.WeaponModel;

        //Changes the size of the weapon by scaling the equiped weapon object transform under the weapon holder in the heiarchy.
        EquipedWeapon.transform.localScale = new Vector3(weapon.WeaponScaleX, weapon.WeaponScaleY, 1f);

        // Assuming 'currentWeapon' is the currently equipped weapon's WeaponStats
        Vector3 shootPosition = WeaponHolder.transform.position + weapon.ProjectileSpawnPoint;

    }

}



