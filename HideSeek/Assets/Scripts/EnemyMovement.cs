using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


// Modified from LlamAcademy's video https://www.youtube.com/watch?v=uAGjKxH4sDQ and "Feed My Kids"'s video https://www.youtube.com/watch?v=RGjMBEGhd7Y
[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] int chanceOfChasing = 3;
    [SerializeField] Transform patrolRoute;
    [SerializeField] List<Transform> locations;
    [SerializeField] Transform player;
    [SerializeField] Camera playerCam;
    [SerializeField] Camera enemyCam;
    [SerializeField] ParticleSystem playerSmoke;
    [SerializeField] ParticleSystem enemySmoke;
    ParticleSystem.EmissionModule playerSmokeEm;
    ParticleSystem.EmissionModule enemySmokeEm;
    int locationIdx = 0;

    float displacementDist = 200f;
    NavMeshAgent agent;
    Animator animator;
    Vector2 velocity;
    Vector2 smoothDeltaPosition;

    float speedLimit = 2f;
    int velocityHash;
    int isMovingHash;

    //bool isChasing = false;

    private bool _isChasing = false;

    private bool IsChasing
    {
        get => _isChasing;
        set
        {
            playerCam.enabled = !value;
            enemyCam.enabled = value; // if enemy is chasing, switch to enemy cam
            playerSmokeEm.enabled = !value;
            enemySmokeEm.enabled = value;
            if (value != _isChasing)
            {
                _isChasing = value;
                if (value)
                {
                    //speedLimit = 1f;
                    StartCoroutine(SpeedUp());
                }
                else
                {
                    speedLimit = 2f;
                }
            }
        }
    }

    bool inCoroutine = false;

    // The solution for avoiding obstacles comes from Daniel Eordogh's comment under this video: https://www.youtube.com/watch?v=Zjlg9F3FRJs
    //// We will check if enemy can flee to the direction opposite from the player, we will check if there are obstacles
    //bool isDirSafe = false;

    // We will need to rotate the direction away from the player if straight to the opposite of the player is a wall
    float vRotation = 0;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = agent.GetComponent<Animator>();

        animator.applyRootMotion = true;
        agent.updatePosition = false;
        agent.updateRotation = false;
    }

    void Start()
    {
        playerSmokeEm = playerSmoke.emission;
        enemySmokeEm = enemySmoke.emission;
        InitializePatrolRoute();
        MoveToNextPatrolLocation();

        velocityHash = Animator.StringToHash("velocity");
        isMovingHash = Animator.StringToHash("isMoving");
    }

    // Update is called once per frame
    void Update()
    {
        //if (!isDirSafe)
        //{


        //    //Rotate the direction of the Enemy to move
        //    newPos = Quaternion.Euler(0, vRotation, 0) * newPos;

        //    //Shoot a Raycast out to the new direction with 5f length (as example raycast length) and see if it hits an obstacle
        //    bool isHit = Physics.Raycast(transform.position, newPos, out RaycastHit hit, 3f, layerMask);

        //    if (hit.transform == null)
        //    {
        //        //If the Raycast to the flee direction doesn't hit a wall then the Enemy is good to go to this direction
        //        nvAgent.SetDestination(newPos);
        //        isDirSafe = true;
        //    }

        //    //Change the direction of fleeing is it hits a wall by 20 degrees
        //    if (isHit && hit.transform.CompareTag("Wall"))
        //    {
        //        vRotation += 20;
        //        isDirSafe = false;
        //    }
        //    else
        //    {
        //        //If the Raycast to the flee direction doesn't hit a wall then the Enemy is good to go to this direction
        //        nvAgent.SetDestination(newPos);
        //        isDirSafe = true;
        //    }



        //if (!inCoroutine)
        //{
        //    StartCoroutine(SetChasingState());
        //}
        Vector3 distance = player.position - transform.position;
        
        float mag = distance.magnitude;
        Vector3 normDir = distance.normalized;
        if (!IsChasing && mag > 149f && mag < 150f)
        {
            Debug.Log("CLOSE");
            IsChasing = Random.Range(0, 10) < 1;
        }
        else if (IsChasing)
        {
            if (mag > 800f)
            {
                IsChasing = false;
            }
            else if (mag > 200f && mag < 201f)
            {
                Debug.Log("FAR");
                IsChasing = Random.Range(0, 10) < 1;
            }
        }
        //switch (distance.magnitude)
        //{
        //    case < 200f:

        //}


        //normDir = Quaternion.AngleAxis(vRotation, Vector3.up) * normDir;

        Vector3 destination = IsChasing ? transform.position + (normDir * displacementDist)
            : transform.position - (normDir * displacementDist);
        //Vector3 destination = locations[locationIdx].position;
        //if (agent.remainingDistance < 100f && !agent.pathPending)
        //    destination = locations[locationIdx].position;
        //bool isHit = Physics.Raycast(transform.position, destination, out RaycastHit hit, 50f);
        bool frontIsHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit frontHit, 200f);
        bool rightIsHit = Physics.Raycast(transform.position, transform.right, out RaycastHit rightHit, 100f);
        transform.rotation = Quaternion.AngleAxis(vRotation, Vector3.up);


        if (frontIsHit && frontHit.transform.CompareTag("Obstacle"))
        {
            //agent.SetDestination(transform.position); // Stop the agent from moving if turning
            if (rightIsHit && rightHit.transform.CompareTag("Obstacle"))
            {
                vRotation -= 10f;
            }
            else
            {
                vRotation += 10f;
            }
            //vRotation += Random.Range(0, 2) == 0 ? -10f : 10f;
            //isDirSafe = false;
        }

        //if (agent.remainingDistance < 100f && !agent.pathPending)


        //MoveToNextPatrolLocation();

        if (!agent.pathPending)
        {
            if (IsChasing)
            {
                transform.LookAt(player);
            }
            //else
            //{
                agent.SetDestination(destination);
                //transform.LookAt(destination);
            //}
        }
        




        //Debug.Log(destination);


        //Debug.Log(agent.destination);
        //else
        //{
        //    Debug.Log(agent.remainingDistance);
        //    Debug.Log(agent.pathPending);
        //}
        //agent.destination = destination;


        //if (agent.remainingDistance < 100f && !agent.pathPending)
        //{
        //    //Debug.Log("Before:");
        //    //Debug.Log(normDir);
        //    normDir = Quaternion.AngleAxis(45f, Vector3.up) * normDir;
        //    //Debug.Log("After:");
        //    //Debug.Log(normDir);
        //}
        //if (isChasing)
        //{
        //    //transform.LookAt(player);
        //    agent.SetDestination(transform.position + (normDir * displacementDist));
        //}
        //else
        //{
        //    agent.SetDestination(transform.position - (normDir * displacementDist));
        //}

        SyncAnimatorAndAgent();
    }

    //IEnumerator SetChasingState()
    //{
    //    inCoroutine = true;
    //    bool old = isChasing;
    //    isChasing = Random.Range(0, 10) < chanceOfChasing; // 30% chance chasing, 70% chance running
    //    playerCam.enabled = !isChasing;
    //    enemyCam.enabled = isChasing; // if enemy is chasing, switch to enemy cam
    //    playerSmokeEm.enabled = !isChasing;
    //    enemySmokeEm.enabled = isChasing;
    //    if (!old && isChasing)
    //    {
    //        StartCoroutine(SpeedUp());
    //    }
    //    else if (old && !isChasing)
    //    {
    //        speedLimit = 2f;
    //    }
    //    Debug.Log("isChasing is" + isChasing);
    //    yield return new WaitForSeconds(30f);
    //    inCoroutine = false;
    //}


    IEnumerator SpeedUp(float initSpeed = 0f, float targetSpeed = 1f)
    {
        speedLimit = initSpeed;
        yield return new WaitForSeconds(1f);
        float currTime = 0f;
        float accelDur = 2f;
        while (currTime < accelDur)
        {
            speedLimit = Mathf.Lerp(initSpeed, targetSpeed, currTime / accelDur);
            currTime += Time.deltaTime;
            yield return null;
        }
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

    void GetRandLocation()
    {
        int tmp;
        do
        {
            tmp = Random.Range(0, locations.Count);
        } while (tmp == locationIdx);
        locationIdx = tmp;
        Debug.Log(locationIdx);
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
        float speed = IsChasing ? 1f : 2f;
        velocity = Vector3.ClampMagnitude(velocity, speedLimit);
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
        if (deltaMagnitude > agent.radius)
        {
            transform.position = Vector3.Lerp(
                animator.rootPosition,
                agent.nextPosition,
                smooth);
        }
    }
}
