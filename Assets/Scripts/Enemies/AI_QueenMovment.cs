using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_QueenMovment : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    int movement;


    // Update is called once per frame
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update is called once per frame
    void Update()
    {
        if (player != null) // Check if the player is available
        {
            // Calculate the horizontal and vertical distances between enemy and player
            float horizontalDistance = player.position.x - transform.position.x;
            float verticalDistance = player.position.y - transform.position.y;

            // Decide movement direction based on distances
            if (Mathf.Abs(horizontalDistance) > Mathf.Abs(verticalDistance))
            {
                if (horizontalDistance > 0)
                {
                    // Move towards right
                    // Code to move enemy to the right
                }
                else
                {
                    // Move towards left
                    // Code to move enemy to the left
                }
            }
            else
            {
                if (verticalDistance > 0)
                {
                    // Move upwards
                    // Code to move enemy upwards
                }
                else
                {
                    // Move downwards
                    // Code to move enemy downwards
                }
            }
        }
    }
}



