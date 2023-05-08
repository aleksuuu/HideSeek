using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController), typeof(Animator))]
public class PlayerMovement : MonoBehaviour
{
    Animator animator;
    CharacterController controller;
    Transform camera;
    float velocity = 0f;
    int velocityHash;
    int isMovingHash;
    float turn;
    float camTurnVertical;

    [Header("Prefabs")]
    [SerializeField] Transform obstaclePrefab;
    [SerializeField] Transform rocketPrefab;
    //Transform currRocket;
    //List<Transform> rockets = new(5);

    [Header("Preferences")]
    [SerializeField] float mouseSensitivity = 5f;

    [Header("Constraints")]
    [SerializeField] float minVert = -60.0f;
    [SerializeField] float maxVert = 60.0f;

    [Header("Materials")]
    [SerializeField] Material opaqueBoxMat;
    [SerializeField] Material transparentBoxMat;
    Transform currObstacle;
    MeshRenderer currMeshRenderer;
    
    bool obstacleIsBeingPlaced = false;
    readonly List<Transform> obstacles = new();

    //[SerializeField] float acceleration = 0.1f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        camera = transform.GetChild(0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        velocityHash = Animator.StringToHash("velocity");
        isMovingHash = Animator.StringToHash("isMoving");

    }

    // Update is called once per frame
    void Update()
    {
        
        //transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        turn += Input.GetAxis("Mouse X") * mouseSensitivity;
        camTurnVertical -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        camTurnVertical = Mathf.Clamp(camTurnVertical, minVert, maxVert);
        transform.localRotation = Quaternion.Euler(0, turn, 0);
        camera.localRotation = Quaternion.Euler(camTurnVertical, 0, 0);

        

        bool jump = Input.GetKey(KeyCode.Space);

        if (Input.GetKey(KeyCode.W))
        {
            bool run = Input.GetKey(KeyCode.LeftShift);
            if (run)
            {
                velocity = 1f;
            }
            else
            {
                velocity = 0.5f;
            }
            animator.SetBool(isMovingHash, true);
        }
        else
        {
            animator.SetBool(isMovingHash, false);
        }
        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            obstacleIsBeingPlaced = true;
        }
        if (obstacleIsBeingPlaced)
        {
            if (!currObstacle)
            {
                currObstacle = Instantiate(obstaclePrefab);
                currMeshRenderer = currObstacle.GetChild(0).GetComponent<MeshRenderer>();
                if (currMeshRenderer)
                {
                    currMeshRenderer.material = transparentBoxMat;
                }
                else
                {
                    Debug.Log("No MeshRenderer found");
                }
                currObstacle.position = transform.position + transform.forward * 100f;
            }
            currObstacle.position += transform.right * Input.GetAxis("Horizontal");
            currObstacle.position += transform.forward * Input.GetAxis("Vertical");
            if (Input.GetKeyUp(KeyCode.RightShift))
            {
                obstacleIsBeingPlaced = false;
                if (currObstacle)
                {
                    if (currMeshRenderer)
                    {
                        currMeshRenderer.material = opaqueBoxMat;
                    }
                    obstacles.Add(currObstacle);
                    currObstacle = null;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Transform currRocket = Instantiate(rocketPrefab);
            currRocket.position = transform.position + transform.up * 50f + transform.forward * 20f;
            currRocket.eulerAngles = transform.eulerAngles + new Vector3(0f, -90f, -90f);
            currRocket.GetComponent<ConstantForce>().force = transform.forward * 100f;
            //currRocket.GetComponent<ConstantForce>().force = Vector3.zero;
            //rockets.Add(currRocket);
            //newRocket.eulerAngles = transform.eulerAngles;


            //bool frontIsHit = Physics.Raycast(transform.position, transform.forward, out RaycastHit frontHit, 200f);
            //if (frontIsHit)
            //{
            //    if (frontHit.transform.CompareTag("Obstacle"))
            //    {

            //    }
            //}
        }



        animator.SetFloat(velocityHash, velocity);

    }

    void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        controller.Move(velocity);
        //controller.SimpleMove(velocity);
        if (controller.isGrounded)
        {
            print("CharacterController is grounded");
        }
    }

    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
    //    Debug.Log(transform.position);
        
    //}




}