using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public GameObject targetObject;
    public AIState aiState = AIState.Idle;
    // If target seen within pursueDistance, AI will start to pursue the target.
    public float pursueDistance = 1500;
    // If target is within attackDistance, AI will attempt to make an attack.
    public float attackDistance = 1000;
    // View angle that AI can see in front of itself
    public float visionAngle = 360;
    // Speed that AI should move when patrolling
    public float patrolSpeed = 1000;
    // Speed that AI should move when pursuing player
    public float pursueSpeed = 2000;
    // Path of waypoints to patrol
    public GameObject[] patrolPath;
    // Time that AI should idle at each waypoint once reached
    public float[] patrolPathIdleTimes;

    private NavMeshAgent agent;
    private Animator anim;
    private Rigidbody rb;
    // Create a layer mask that ignores enemies and the player, may need to change if player hides behind an enemy
    private LayerMask visMask;

    // Current idle timer
    private float idleTime = 1;
    // The current patrol waypoint, starts at -1 so when setNextWaypoint is first called it will go to first waypoint
    private int currWaypoint = -1;


    public enum AIState
    {
        Idle,
        Patrol,
        Pursue,
        Attack
    }

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        rb = gameObject.GetComponent<Rigidbody>();
        // Creates a layermask that ignores the player and enemies.
        visMask = ~LayerMask.GetMask("Player", "Enemy");

        agent.speed = patrolSpeed;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    // Update is called once per frame
    void Update()
    {
        float targetDist = Mathf.Abs(Vector3.Magnitude(targetObject.transform.position - rb.position));

        switch (aiState)
        {
            case AIState.Idle:
                // Update current idling timer
                idleTime -= Time.deltaTime;

                // Transition to Pursue state if target is visible
                if (CanSee(pursueDistance, visionAngle))
                {
                    agent.speed = pursueSpeed;
                    aiState = AIState.Pursue;
                } // Transition to moving to next waypoint once idle time elapsed
                else if (idleTime <= 0)
                {
                    SetNextWaypoint();
                    agent.speed = patrolSpeed;
                    aiState = AIState.Patrol;
                }
                break;
            case AIState.Patrol:

                // Transition to Pursue state if target is visible within check distance
                if (CanSee(pursueDistance, visionAngle))
                {
                    agent.speed = pursueSpeed;
                    aiState = AIState.Pursue;
                } // Transition to idle state once current waypoint reached
                else if (agent.remainingDistance < 1 && !agent.pathPending)
                {
                    aiState = AIState.Idle;
                }
                break;
            case AIState.Pursue:
                agent.SetDestination(targetObject.transform.position);

                // Transition to attacking state
                if (targetDist <= attackDistance)
                {
                    anim.SetBool("Attack", true);
                    aiState = AIState.Attack;
                }
                break;
            case AIState.Attack:
                agent.SetDestination(targetObject.transform.position);
                // Transition back to pursue state if target moves out of attack range
                if (targetDist > attackDistance)
                {
                    anim.SetBool("Attack", false);
                    aiState = AIState.Pursue;
                }
                break;
            default:

                break;

        }
        rb.velocity = agent.velocity;
        anim.SetFloat("forwardVel", Vector3.Magnitude(rb.velocity));
        Debug.Log(rb.velocity);
        Debug.Log(agent.speed);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.CompareTag("Player") && aiState == AIState.Attack)
        {
            // TODO: change this to an event that tells some event manager to respawn the player at the last checkpoint
            c.gameObject.transform.position = (new Vector3(64.5f, 0.258f, 18.659f));
            // Need to use this to force the character controller to not override the new position.
            Physics.SyncTransforms();
        }
    }

    // Checks if AI can see RigidBody target object within the provided distance visDistance and angle visAngle
    private bool CanSee(float visDistance, float visAngle)
    {
        Vector3 targetDir = targetObject.transform.position - rb.position;

        float targetDist = Mathf.Abs(Vector3.Magnitude(targetDir));
        float targetAngle = Vector3.Angle(transform.forward, Vector3.Normalize(targetDir));



        if (targetDist <= visDistance && targetAngle <= visAngle)
        {

            // Use a raycast to make secondary check whether target is hidden behind something
            RaycastHit hit;
            bool isHit = Physics.Raycast(rb.position, targetDir, out hit, visDistance, visMask);

            // Return true if nothing blocking vision to target, or if raycast hit target itself
            return !isHit || hit.collider.gameObject == targetObject;

        }
        else
        {
            return false;
        }
    }

    private void SetNextWaypoint()
    {
        if (patrolPath.Length != patrolPathIdleTimes.Length)
        {
            throw new MissingReferenceException("Number of patrol waypoint objects does not match number of patrol idle times.");
        }

        currWaypoint++;
        if (currWaypoint >= patrolPath.Length)
        {
            currWaypoint = 0;
        }
        agent.SetDestination(patrolPath[currWaypoint].transform.position);
        idleTime = patrolPathIdleTimes[currWaypoint];
    }
}
