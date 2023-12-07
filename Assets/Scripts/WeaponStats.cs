using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponStats : MonoBehaviour
{
    [Header("-------WeaponStats-------")]
    public float AttackRate;
    public int AttackDamage;
    public GameObject model;
    public ParticleSystem hitEffect;
    public AudioClip shootSound;

    [Header("-----IfRangedWeapon-----")]
    public int ammoCur;
    public int ammoMax;
    public Sprite projectileSprite;


}
