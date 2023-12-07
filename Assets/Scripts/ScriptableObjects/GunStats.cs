using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class GunStats : ScriptableObject
{
    public float baseShootRate;
    public int baseShootDamage;
    public int baseShootdist;
    public Vector3 ShootPos;
    public GameObject projectile;
    public float baseProjectileSpeed;

    public GameObject model;
    public ParticleSystem hitEffect;
    public ParticleSystem hitEffectEnemy;
    public AudioClip shootSound;
    [Range(0, 1)] public float shootSoundVol;
    public int ID;
    //public bool IsRaycast;

}
