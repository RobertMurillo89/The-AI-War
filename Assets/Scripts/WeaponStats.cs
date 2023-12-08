using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class WeaponStats : ScriptableObject
{
    [Header("-------WeaponStats-------")]
    public WeaponType weaponType;
    public float AttackRate;
    public int AttackDamage;
    public Sprite WeaponModel;
    public ParticleSystem hitEffect;
    public AudioClip AttackSound;
    public AudioClip ReloadSound;

    [Header("-----IfRangedWeapon-----")]
    public int ammoCur;
    public int ammoMax;
    public Sprite projectileSprite;
    public int ProjectileSpeed;
    public Projectile ProjectilePrefab;
    //public Transform ProjectileSpawnPoint;
    public Vector3 ProjectileSpawnPoint;

    public enum WeaponType
    {
        Melee,
        MeleeWithAmmo, // chainsaw or something like that
        Ranged,
        Grenadier //grenade launcher or RPG..
    }
}
