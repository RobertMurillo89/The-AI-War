using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.Burst.Intrinsics;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class CharacterData
{
    public List<WeaponStats> Items;
    public string Name;

    public CharacterData()
    {
        Items = new List<WeaponStats>();
        
    }

}
