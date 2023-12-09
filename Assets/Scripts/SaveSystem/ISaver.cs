using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISaver
{
    void LoadData(CharacterData characterData);
    
    void SaveData(ref  CharacterData characterData);
}
