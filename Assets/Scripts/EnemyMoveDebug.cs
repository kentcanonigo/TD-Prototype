using UnityEngine;

public class EnemyMoveDebug : MonoBehaviour
{
    public Vector3 centerPoint = Vector3.zero; // The point around which the GameObject will rotate
    public float speed = 5f; // Speed of the rotation
    public float radius = 2f; // Radius for circular movement

    public enum MovementPattern { Circle, Square }
    public MovementPattern movementPattern = MovementPattern.Circle;

    private float angle;

    void Start()
    {
        // Set the initial position
        transform.position = centerPoint + new Vector3(radius, 0f, 0f);
    }

    void Update()
    {
        switch (movementPattern)
        {
            case MovementPattern.Circle:
                MoveInCircle();
                break;
            case MovementPattern.Square:
                MoveInSquare();
                break;
        }
    }

    private void MoveInCircle()
    {
        // Increment the angle over time
        angle += speed * Time.deltaTime;

        // Calculate the offset from the center using trigonometry
        float x = Mathf.Cos(angle) * radius;
        float y = Mathf.Sin(angle) * radius;

        // Calculate the new position relative to the current position
        Vector3 newPosition = new Vector3(x, y, 0f);

        // Translate the object relative to its current position
        transform.position = centerPoint + newPosition;
    }

    private void MoveInSquare()
    {
        float timeInCurrentSegment = (Time.time * speed) % (4 * radius); // Time in the current segment of the square
        Vector3 newPosition = Vector3.zero;

        if (timeInCurrentSegment < radius) // Move right
        {
            newPosition = Vector3.right * speed * Time.deltaTime;
        }
        else if (timeInCurrentSegment < 2 * radius) // Move down
        {
            newPosition = Vector3.down * speed * Time.deltaTime;
        }
        else if (timeInCurrentSegment < 3 * radius) // Move left
        {
            newPosition = Vector3.left * speed * Time.deltaTime;
        }
        else // Move up
        {
            newPosition = Vector3.up * speed * Time.deltaTime;
        }

        // Translate the object relative to its current position
        transform.Translate(newPosition);
    }
}
