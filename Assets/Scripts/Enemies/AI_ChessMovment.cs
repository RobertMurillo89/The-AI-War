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
    Vector2 otherCheackBox;
    bool isMoving;
    [SerializeField] int movement;
    [SerializeField] int moveTicks;
    [SerializeField] BoxCollider2D canIMove;
    [SerializeField] BoxCollider2D otherChecker;
    bool iCantMave;
    bool foundOtherWall;
    int ticks;
    int DiceRoll;

    // Update is called once per frame
    // Start is called before the first frame update
    void Start()
    {
        //ticks = moveTicks;
    }


    // Update is called once per frame
    void Update()
    {
        if (ticks > 0)
        {
            transform.Translate(Direction * movement * Time.deltaTime);
            ticks--;
        }
        else
        {
            ticks = moveTicks;
            Direction = SetDirection();
        }
    }
    protected override void MoveTowardsPlayer()
    {
        if (ticks > 0)
        {
            if (!isMoving)
            {
            transform.Translate(Direction * movement * Time.deltaTime);
            ticks--;
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

        if (angle >= -22.5f && angle < 22.5f)
        {
            VDirection = new Vector3(1, 0, 0).normalized;
            DirectionCheackBox = new Vector2(1, 0);
        }
        else if (angle >= 22.5f && angle < 67.5f)
        {
            VDirection = new Vector3(1, 1, 0).normalized;
            DirectionCheackBox = new Vector2(1, 1);
        }
        else if (angle >= 67.5f && angle < 112.5f)
        {
            VDirection = new Vector3(0, 1, 0).normalized;
            DirectionCheackBox = new Vector2(0, 1);
        }
        else if (angle >= 112.5f && angle < 157.5f)
        {
            VDirection = new Vector3(-1, 1, 0).normalized;
            DirectionCheackBox = new Vector2(-1, 1);
        }
        else if (angle >= 157.5f || angle < -157.5f)
        {
            VDirection = new Vector3(-1, 0, 0).normalized;
            DirectionCheackBox = new Vector2(-1, 0);
        }
        else if (angle >= -157.5f && angle < -112.5f)
        {
            VDirection = new Vector3(-1, -1, 0).normalized;
            DirectionCheackBox = new Vector2(-1, -1);
        }
        else if (angle >= -112.5f && angle < -67.5f)
        {
            VDirection = new Vector3(0, -1, 0).normalized;
            DirectionCheackBox = new Vector2(0, -1);
        }
        else if (angle >= -67.5f && angle < -22.5f)
        {
            VDirection = new Vector3(1, -1, 0).normalized;
            DirectionCheackBox = new Vector2(1, -1);
        }
            canIMove.offset = DirectionCheackBox;

        // Default direction if no angle condition is met
        return VDirection;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            iCantMave = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("wall"))
        {
            iCantMave = false;
        }
    }
}