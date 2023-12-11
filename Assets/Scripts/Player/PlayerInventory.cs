using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, ISaver
{
    public List<WeaponStats> WeaponList = new List<WeaponStats>();
    public CharacterData CharacterData;

    public void AddItem(WeaponStats item)
    {
        WeaponList.Add(item);
        SaveManager.Instance.RequestSave();
    }

    public void LoadData(CharacterData characterData)
    {
        this.WeaponList = characterData.Items;
    }

    public void SaveData(ref CharacterData characterData)
    {
        characterData.Items = this.WeaponList;
    }
}
