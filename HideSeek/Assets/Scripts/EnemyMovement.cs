using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Modified from LlamAcademy's video https://www.youtube.com/watch?v=uAGjKxH4sDQ and "Feed My Kids"'s video https://www.youtube.com/watch?v=RGjMBEGhd7Y
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] Transform patrolRoute;
    [SerializeField] List<Transform> locations;
    [SerializeField] Transform player;
    int locationIdx = 0;

    float displacementDist = 200f;
    NavMeshAgent agent;
    Animator animator;
    Vector2 velocity;
    Vector2 smoothDeltaPosition;
    int velocityHash;
    int isMovingHash;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();

        animator.applyRootMotion = true;
        agent.updatePosition = false;
        agent.updateRotation = true;
    }

    void Start()
    {
        InitializePatrolRoute();
        MoveToNextPatrolLocation();
        
        velocityHash = Animator.StringToHash("velocity");
        isMovingHash = Animator.StringToHash("isMoving");

    }

    // Update is called once per frame
    void Update()
    {
        //Vector3 normDir = (player.position - transform.position).normalized;
        //normDir = Quaternion.AngleAxis(45f, Vector3.up) * normDir;
        //if (agent.remainingDistance < 100f && !agent.pathPending)
        //{
        //    Debug.Log("Before:");
        //    Debug.Log(normDir);
        //    normDir = Quaternion.AngleAxis(45f, Vector3.up) * normDir;
        //    Debug.Log("After:");
        //    Debug.Log(normDir);
        //}
        //agent.SetDestination(transform.position - (normDir * displacementDist));
        if (agent.remainingDistance < 100f && !agent.pathPending)
        {
            MoveToNextPatrolLocation();
        }
        SyncAnimatorAndAgent();
    }

    void InitializePatrolRoute()
    {
        foreach (Transform child in patrolRoute)
        {
            locations.Add(child);
        }
    }

    void MoveToNextPatrolLocation()
    {
        if (locations.Count == 0)
            return;
        agent.destination = locations[locationIdx].position;
        int tmp;
        do
        {
            tmp = Random.Range(0, locations.Count);
        } while (tmp == locationIdx);
        locationIdx = tmp;
    }

    void OnAnimatorMove()
    {
        Vector3 rootPosition = animator.rootPosition;
        rootPosition.y = agent.nextPosition.y;
        transform.position = rootPosition;
        agent.nextPosition = rootPosition;
    }

    void SyncAnimatorAndAgent()
    {
        Vector3 worldDeltaPosition = agent.nextPosition - transform.position; 
        worldDeltaPosition.y = 0;
        // Map 'worldDeltaPosition' to local space
        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new(dx, dy);

        // Low-pass filter the deltaMove
        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        smoothDeltaPosition = Vector2.Lerp(smoothDeltaPosition, deltaPosition, smooth);

        velocity = smoothDeltaPosition / Time.deltaTime;
        velocity = Vector3.ClampMagnitude(velocity, 2f);
        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            velocity = Vector2.Lerp(
                Vector2.zero,
                velocity,
                agent.remainingDistance / agent.stoppingDistance);
            
        }
        bool shouldMove = velocity.magnitude >= 0.5f && agent.remainingDistance > agent.stoppingDistance;


        animator.SetBool(isMovingHash, shouldMove);
        animator.SetFloat(velocityHash, velocity.magnitude);

        float deltaMagnitude = worldDeltaPosition.magnitude;
        // this is used to Reconcile Difference in Animation and Agent Positions (12:26 in LlamAcademy video)
        if (deltaMagnitude > agent.radius * 0.5f)
        {
            transform.position = Vector3.Lerp(
                animator.rootPosition,
                agent.nextPosition,
                smooth);
        }
    }
}
