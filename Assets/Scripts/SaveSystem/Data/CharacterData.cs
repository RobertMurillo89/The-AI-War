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
    [SerializeField] List<WeaponStats> Items = new List<WeaponStats>();
    private bool isDirty = false;

    public void AddItem(WeaponStats item)
    {
        Items.Add(item);
        isDirty = true;
    }

    public void RemoveItem(WeaponStats item)
    {
        Items.Remove(item);
        isDirty = true;
    }

    public bool NeedSave()
    {
        return isDirty;
    }

    public void ResetSaveFlag()
    {
        isDirty = false;
    }
}
