using UnityEngine; // Importing the UnityEngine namespace

public class AI_Chess : EnemyAI
{

    [Header("Player Reference")]
    public Transform player; // Reference to the player's Transform

    [Header("Movement Variables")]
    [SerializeField] int movement; // Movement speed, exposed in the inspector
    [SerializeField] int moveTicks; // Move ticks, exposed in the inspector
    private Vector3 Direction; // Direction of movement
    private Vector3 otherDirection; // Alternative direction of movement
    private Vector3 retreat; // Direction to retreat
    private bool isMoving; // Check if the AI is moving
    private int ticks; // Ticks counter
    private int DiceRoll; // Dice roll for randomness

    [Header("Collision Detection")]
    [SerializeField] BoxCollider2D canIMove; // Check if the AI can move, exposed in the inspector
    [SerializeField] GameObject otherChecker; // Other checker object, exposed in the inspector
    private Vector2 DirectionCheackBox; // Direction of the main check box
    private Vector2 otherCheackBox; // Direction of the other check box
    private bool iCantMove = true; // Check if the AI can't move
    private bool foundOtherWall; // Check if another wall is found

    [Header("MoveType Types and Decision Making")]
    [SerializeField] byte typeOfMove; // Type of move, exposed in the inspector
    public enum MoveType { Rook = 1, Bishop, Queen } // Types of moves
    private bool randomBool; // Random boolean for decision making

    void Update() // Update method called once per frame
    {
        MoveTowardsPlayer(); // Calling the MoveTowardsPlayer method
    }
    protected override void MoveTowardsPlayer() // Overriding the MoveTowardsPlayer method from the EnemyAI class
    {
        if (ticks > 0) // If ticks is greater than 0
        {
            if (iCantMove) // If the AI can't move
            {
                foundOtherWall = false; // Set foundOtherWall to false
                transform.Translate(Direction * movement * Time.deltaTime); // Move the AI in the direction multiplied by the movement speed and the time since the last frame
                ticks--; // Decrement ticks by 1
            }
            else // If the AI can move
            {
                if (!foundOtherWall) transform.Translate(otherDirection * movement * Time.deltaTime); // If the AI hasn't found another wall, move the AI in the other direction multiplied by the movement speed and the time since the last frame
                else // If the AI has found another wall
                {
                    Direction = retreat; // Set the direction to the retreat direction
                    ticks = 100; // Set ticks to 100
                    foundOtherWall = false; // Set foundOtherWall to false
                    transform.Translate(Direction * movement * Time.deltaTime); // Move the AI in the direction multiplied by the movement speed and the time since the last frame
                    ticks--; // Decrement ticks by 1
                }
            }
        }
        else // If ticks is not greater than 0
        {
            switch (typeOfMove) // Switch statement based on the type of move
            {
                case 0: // Case 0
                    Direction = SetDirectionRook(); // Set the direction to the result of the SetDirectionRook method
                    break; // Break out of the switch statement
                case 1: // Case 1
                    Direction = SetDirectionBishop(); // Set the direction to the result of the SetDirectionBishop method
                    break; // Break out of the switch statement
                default: // Default case
                    Direction = SetDirectionQueen(); // Set the direction to the result of the SetDirectionQueen method
                    break; // Break out of the switch statement
            }
        }
    }

    public void Retreat() // Public method to make the AI retreat
    {
        foundOtherWall = true; // Set foundOtherWall to true
    }

    Vector3 SetDirectionRook()
    {
        Vector3 VDirection = Vector3.zero;
        Vector2 direction = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        DiceRoll = Random.Range(0, 2);
        if (angle >= -45f && angle < 45f)
        {
            VDirection = new Vector3(1, 0, 0).normalized;
            DirectionCheackBox = new Vector2(1, 0);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(0, 1, 0).normalized;
                otherCheackBox = new Vector2(0, 1);
                retreat = new Vector3(-1, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(0, -1, 0).normalized;
                otherCheackBox = new Vector2(0, -1);
                retreat = new Vector3(-1, 1, 0).normalized;
            }
        }
        else if (angle >= 45f && angle < 135f)
        {
            VDirection = new Vector3(0, 1, 0).normalized;
            DirectionCheackBox = new Vector2(0, 1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, 0, 0).normalized;
                otherCheackBox = new Vector2(-1, 0);
                retreat = new Vector3(1, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, 0, 0).normalized;
                otherCheackBox = new Vector2(1, 0);
                retreat = new Vector3(-1, -1, 0).normalized;
            }
        }
        else if (angle >= 135f || angle < -135f)
        {
            VDirection = new Vector3(-1, 0, 0).normalized;
            DirectionCheackBox = new Vector2(-1, 0);
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
        else if (angle >= -135f && angle < -45f)
        {
            VDirection = new Vector3(0, -1, 0).normalized;
            DirectionCheackBox = new Vector2(0, -1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, 0, 0).normalized;
                otherCheackBox = new Vector2(-1, 0);
                retreat = new Vector3(1, 1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, -0, 0).normalized;
                otherCheackBox = new Vector2(1, -0);
                retreat = new Vector3(-1, 1, 0).normalized;
            }
        }
        return VDirection;
    }
    Vector3 SetDirectionBishop()
    {
        Vector3 VDirection = Vector3.zero;
        Vector2 direction = playerTransform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        DiceRoll = Random.Range(0, 2);
        if (angle >= 0f && angle < 90f)
        {
            VDirection = new Vector3(1, 1, 0).normalized;
            DirectionCheackBox = new Vector2(1, 1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, 1, 0).normalized;
                otherCheackBox = new Vector2(-1, 1);
                retreat = new Vector3(0, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, -1, 0).normalized;
                otherCheackBox = new Vector2(1, -1);
                retreat = new Vector3(-1, 0, 0).normalized;
            }
        }
        else if (angle >= 90f && angle < 180f)
        {
            VDirection = new Vector3(-1, 1, 0).normalized;
            DirectionCheackBox = new Vector2(-1, 1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(-1, -1, 0).normalized;
                otherCheackBox = new Vector2(-1, -1);
                retreat = new Vector3(1, 0, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, 1, 0).normalized;
                otherCheackBox = new Vector2(1, 1);
                retreat = new Vector3(0, -1, 0).normalized;
            }
        }
        else if (angle >= -180f && angle < -90f)
        {
            VDirection = new Vector3(-1, -1, 0).normalized;
            DirectionCheackBox = new Vector2(-1, -1);
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
        else if (angle >= -90f && angle < 0f)
        {
            VDirection = new Vector3(1, -1, 0).normalized;
            DirectionCheackBox = new Vector2(1, -1);
            if (DiceRoll == 1)
            {
                otherDirection = new Vector3(1, 1, 0).normalized;
                otherCheackBox = new Vector2(1, 1);
                retreat = new Vector3(-1, 0, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(-1, -1, 0).normalized;
                otherCheackBox = new Vector2(-1, -1);
                retreat = new Vector3(0, 1, 0).normalized;
            }
        }
        return VDirection;
    }
    Vector3 SetDirectionQueen()
    {
        Vector3 VDirection = Vector3.zero;
        //transform.position = Vector2.MoveTowards(transform.position, playerTransform.position, Speed * Time.deltaTime);
        Vector2 direction = playerTransform.position - transform.position;

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
                retreat = new Vector3(-1, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(0, -1, 0).normalized;
                otherCheackBox = new Vector2(0, -1);
                retreat = new Vector3(-1, 1, 0).normalized;
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
                retreat = new Vector3(0, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, -1, 0).normalized;
                otherCheackBox = new Vector2(1, -1);
                retreat = new Vector3(-1, 0, 0).normalized;
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
                retreat = new Vector3(1, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, 0, 0).normalized;
                otherCheackBox = new Vector2(1, 0);
                retreat = new Vector3(-1, -1, 0).normalized;
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
                retreat = new Vector3(1, 0, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, 1, 0).normalized;
                otherCheackBox = new Vector2(1, 1);
                retreat = new Vector3(0, -1, 0).normalized;
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
                retreat = new Vector3(1, -1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(0, -1, 0).normalized;
                otherCheackBox = new Vector2(0, -1);
                retreat = new Vector3(1, 1, 0).normalized;
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
                retreat = new Vector3(1, 0, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, -1, 0).normalized;
                otherCheackBox = new Vector2(1, -1);
                retreat = new Vector3(0, 1, 0).normalized;
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
                retreat = new Vector3(1, 1, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(1, -0, 0).normalized;
                otherCheackBox = new Vector2(1, -0);
                retreat = new Vector3(-1, 1, 0).normalized;
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
                retreat = new Vector3(-1, 0, 0).normalized;
            }
            else
            {
                otherDirection = new Vector3(-1, -1, 0).normalized;
                otherCheackBox = new Vector2(-1, -1);
                retreat = new Vector3(0, 1, 0).normalized;
            }
        }
        canIMove.offset = DirectionCheackBox;
        otherChecker.GetComponent<BoxCollider2D>().offset = otherCheackBox;
        // Default direction if no angle condition is met
        return VDirection;
    }

    private void OnTriggerEnter2D(Collider2D other) // Method called when the AI enters a 2D trigger
    {
        Debug.Log("Collision with the wall!"); // Log a message to the console
        if (other.CompareTag("wall")) // If the other collider has the tag "wall"
        {
            iCantMove = false; // Set iCantMove to false
        }
    }
    private void OnTriggerExit2D(Collider2D other) // Method called when the AI exits a 2D trigger
    {
        if (other.CompareTag("wall")) // If the other collider has the tag "wall"
        {
            iCantMove = true; // Set iCantMove to true
        }
    }
}