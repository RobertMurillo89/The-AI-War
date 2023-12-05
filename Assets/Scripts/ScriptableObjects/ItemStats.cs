using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]

public class ItemStats : ScriptableObject
{
    public string itemName;
    public int id;
    public int amount;

    public enum itemType { Health = 1, grenade, Armor, Damage, Speed, FireRate};
    public itemType type;
   
}
