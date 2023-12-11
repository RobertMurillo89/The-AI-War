using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class AI_Chess : EnemyAI
{
    public Transform player; // Reference to the player's Transform
    Vector3 Direction;
    Vector2 DirectionCheackBox;
    Vector3 otherDirection;
    Vector3 retreat;
    Vector2 otherCheackBox;
    bool isMoving;
    [SerializeField] int movement;
    [SerializeField] int moveTicks;
    [SerializeField] BoxCollider2D canIMove;
    [SerializeField] BoxCollider2D otherChecker;
    bool iCantMove = true;
    bool foundOtherWall;
    int ticks;
    int DiceRoll;
    bool randomBool;

    // Update is called once per frame
    // Start is called before the first frame update
    void Start()
    {
        //ticks = moveTicks;
    }


    // Update is called once per frame
    void Update()
    {
        MoveTowardsPlayer();
    }
    protected override void MoveTowardsPlayer()
    {
        if (ticks > 0)
        {
            if (iCantMove)
            {
                transform.Translate(Direction * movement * Time.deltaTime);
                ticks--;
            }
            else
            {
                canIMove.offset = otherCheackBox;
                transform.Translate(otherDirection * movement * Time.deltaTime);
               //if
               // {
                   //Direction = retreat;
                    //ticks = 5;
                    //transform.Translate(Direction * movement * Time.deltaTime);
                //}
                canIMove.offset = DirectionCheackBox;
            }
        }
        else
        {
            ticks = moveTicks;
            Direction = SetDirection();
        }
    }
    Vector3 SetDirection()
    {
        Vector3 VDirection = Vector3.zero;
        Vector2 direction = player.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        DiceRoll = Random.Range(0, 2);
        if (angle >= -22.5f && angle < 22.5f)
        {
            VDirection = new Vector3(1, 0, 0).normalized;
            DirectionCheackBox = new Vector2(1, 0);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(0, 1, 0).normalized;
                otherCheackBox = new Vector2(0, 1);
                retreat = new Vector3(1, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(0, -1, 0).normalized;
                otherCheackBox = new Vector2(0, -1);
                retreat = new Vector3(1, 1, 0).normalized;
            }
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            VDirection = new Vector3(1, 1, 0).normalized;
            DirectionCheackBox = new Vector2(1, 1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, 1, 0).normalized;
                otherCheackBox = new Vector2(-1, 1);
                retreat = new Vector3(1, 0, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, -1, 0).normalized;
                otherCheackBox = new Vector2(1, -1);
                retreat = new Vector3(0, 1, 0).normalized;
            }
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            VDirection = new Vector3(0, 1, 0).normalized;
            DirectionCheackBox = new Vector2(0, 1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, 0, 0).normalized;
                otherCheackBox = new Vector2(-1, 0);
                retreat = new Vector3(1, 1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, 0, 0).normalized;
                otherCheackBox = new Vector2(1, 0);
                retreat = new Vector3(-1, 1, 0).normalized;
            }
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            VDirection = new Vector3(-1, 1, 0).normalized;
            DirectionCheackBox = new Vector2(-1, 1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, -1, 0).normalized;
                otherCheackBox = new Vector2(-1, -1);
                retreat = new Vector3(0, 1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, 1, 0).normalized;
                otherCheackBox = new Vector2(1, 1);
                retreat = new Vector3(-1, 0, 0).normalized;
            }
        }
        else if (angle >= 157.5f || angle < -157.5f)
        {
            VDirection = new Vector3(-1, 0, 0).normalized;
            DirectionCheackBox = new Vector2(-1, 0);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(0, 1, 0).normalized;
                otherCheackBox = new Vector2(0, 1);
                retreat = new Vector3(1, 1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(0, -1, 0).normalized;
                otherCheackBox = new Vector2(0, -1);
                retreat = new Vector3(1, -1, 0).normalized;
            }
        }
        else if (angle >= -157.5f && angle < -112.5f)
        {
            VDirection = new Vector3(-1, -1, 0).normalized;
            DirectionCheackBox = new Vector2(-1, -1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, 1, 0).normalized;
                otherCheackBox = new Vector2(-1, 1);
                retreat = new Vector3(-1, 0, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, -1, 0).normalized;
                otherCheackBox = new Vector2(1, -1);
                retreat = new Vector3(0, -1, 0).normalized;
            }
        }
        else if (angle >= -112.5f && angle < -67.5f)
        {
            VDirection = new Vector3(0, -1, 0).normalized;
            DirectionCheackBox = new Vector2(0, -1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, 0, 0).normalized;
                otherCheackBox = new Vector2(-1, 0);
                retreat = new Vector3(-1, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, -0, 0).normalized;
                otherCheackBox = new Vector2(1, -0);
                retreat = new Vector3(1, -1, 0).normalized;
            }
        }
        else if (angle >= -67.5f && angle < -22.5f)
        {
            VDirection = new Vector3(1, -1, 0).normalized;
            DirectionCheackBox = new Vector2(1, -1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(1, 1, 0).normalized;
                otherCheackBox = new Vector2(1, 1);
                retreat = new Vector3(1, 0, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(-1, -1, 0).normalized;
                otherCheackBox = new Vector2(-1, -1);
                retreat = new Vector3(0, -1, 0).normalized;
            }
        }
        canIMove.offset = DirectionCheackBox;

        // Default direction if no angle condition is met
        return VDirection;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Collision with the wall!");
        if (other.CompareTag("wall"))
        {
            iCantMove = false;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            iCantMove = true;
        }
    }
}