using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DevTool : MonoBehaviour
{
    public PlayerController player;

    public void GoFaster()
    {
        player.MoveSpeed +=1f;
    }
    public void GoSlower()
    {
        player.MoveSpeed -=1f;
    }
}
