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
    [SerializeField] Transform obstaclePrefab;
    [SerializeField] float mouseSensitivity = 5f;

    [Header("Constraints")]
    [SerializeField] float minVert = -60.0f;
    [SerializeField] float maxVert = 60.0f;


    Transform currObstacle;
    bool obstacleIsBeingPlaced = false;
    List<Transform> obstacles = new();

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
        turn += Input.GetAxis("Mouse X") * mouseSensitivity;
        camTurnVertical -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        camTurnVertical = Mathf.Clamp(camTurnVertical, minVert, maxVert);
        transform.localRotation = Quaternion.Euler(0, turn, 0);
        camera.localRotation = Quaternion.Euler(camTurnVertical, 0, 0);
        bool forward = Input.GetKey(KeyCode.W);

        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            obstacleIsBeingPlaced = true;
        }
        if (obstacleIsBeingPlaced)
        {
            if (Input.GetKeyUp(KeyCode.RightShift))
            {
                obstacleIsBeingPlaced = false;
                if (currObstacle)
                {
                    obstacles.Add(currObstacle);
                    currObstacle = null;
                }
            }
        }

        bool jump = Input.GetKey(KeyCode.Space);

        if (forward)
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
        if (obstacleIsBeingPlaced)
        {
            if (!currObstacle)
            {
                currObstacle = Instantiate(obstaclePrefab);
                currObstacle.position = transform.position + transform.forward * 60f + new Vector3(0f, 25f, 0f);
            }
            currObstacle.position += transform.right * Input.GetAxis("Horizontal");
            currObstacle.position += transform.forward * Input.GetAxis("Vertical");
        }


        animator.SetFloat(velocityHash, velocity);

    }

    void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        //velocity.y = ySpeed * Time.deltaTime;
        controller.Move(velocity);
    }

    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{
        
    //}
}