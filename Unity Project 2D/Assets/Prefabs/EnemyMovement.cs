using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public int speed = 3;
    public float rotationSpeed = 5f;

    private int currentWaypointIndex = 0;
    private Animator animator;  // Reference to the Animator component
    void Start()
    {
        // Get the Animator component on the enemy
        animator = GetComponent<Animator>();
        // This will now run for each enemy clone
        if (waypoints == null || waypoints.Length == 0 || waypoints.Length == 1)
        {
            GameObject[] waypointObjects = GameObject.FindGameObjectsWithTag("Waypoint");
            waypoints = new Transform[waypointObjects.Length];

            for (int i = 0; i < waypointObjects.Length; i++)
            {
                waypoints[i] = waypointObjects[i].transform;
            }

            System.Array.Sort(waypoints, (a, b) => a.name.CompareTo(b.name));
        }
    }

    void Update()
    {
        if (waypoints.Length == 0)
        {
            return; // Dont move
        }
        MoveAlongPath();
    }
    void MoveAlongPath()
    {
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Calculate the direction to the next waypoint
        Vector2 direction = targetWaypoint.position - transform.position;

        // Prevent rotation
        //transform.rotation = Quaternion.identity;

        // Moves enemy toward waypoint
        float step = Time.deltaTime * speed;
        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, step);

        // After rotating, update the animation direction
        UpdateAnimationDirection(direction);

        // If enemy has reached the waypoint
        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++; // Move to next waypoint
            // If enemy has reached the last waypoint it destroys the enemy
            if (currentWaypointIndex >= waypoints.Length)
            {
                Destroy(gameObject);
            }
        }
    }
    // Update the animation direction based on the movement direction
    void UpdateAnimationDirection(Vector2 direction)
    {
        // Reset all animation states to false before switching
        animator.SetBool("isWalkingRight", false);
        animator.SetBool("isWalkingLeft", false);
        animator.SetBool("isWalkingUp", false);
        animator.SetBool("isWalkingDown", false);

        // Check movement direction
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))  // Moving mainly horizontally
        {
            if (direction.x > 0)  // Moving Right
            {
                animator.SetBool("isWalkingRight", true);
            }
            else if (direction.x < 0)  // Moving Left
            {
                animator.SetBool("isWalkingLeft", true);
            }
        }
        else  // Moving mainly vertically
        {
            if (direction.y > 0)  // Moving Up
            {
                animator.SetBool("isWalkingUp", true);
            }
            else if (direction.y < 0)  // Moving Down
            {
                animator.SetBool("isWalkingDown", true);
            }
        }
    }
}
