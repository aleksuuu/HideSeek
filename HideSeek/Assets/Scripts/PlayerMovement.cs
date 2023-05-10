using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CharacterController), typeof(Animator), typeof(AudioSource))]
public class PlayerMovement : MonoBehaviour
{
    public static PlayerMovement Instance;
    private void Awake()
    {
        // Singleton pattern
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        controller = GetComponent<CharacterController>();
    }
    Animator animator;
    CharacterController controller;
    AudioSource source;
    Transform camera;
    float velocity = 0f;
    int velocityHash;
    int isMovingHash;
    float turn;
    float camTurnVertical;

    [Header("Prefabs")]
    [SerializeField] Transform obstaclePrefab;
    [SerializeField] Transform rocketPrefab;

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

        if (GameBehavior.Instance.CurrentState != State.Play)
        {
            return;
        }
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
        if (Input.GetKeyDown(KeyCode.RightShift) && PlayerStats.Instance.BoxIsAvailable)
        {
            obstacleIsBeingPlaced = true;
            PlayerStats.Instance.BoxProgress = 0f;
            PlayerStats.Instance.DoStartBoxProgress = false;
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
                PlayerStats.Instance.DoStartBoxProgress = true;
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
        }



        animator.SetFloat(velocityHash, velocity);

    }

    void OnAnimatorMove()
    {
        Vector3 velocity = animator.deltaPosition;
        controller.Move(velocity);
        if (transform.position.y != 0f) // definitely not a very elegant solution but this prevents the player from floating or sinking...
        {
            controller.enabled = false;
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
            controller.enabled = true;
        }
    }

    public void Reset()
    {
        controller.enabled = false;
        transform.position = PlayerStats.Instance.initPosition;
        controller.enabled = true;
    }



    //void OnControllerColliderHit(ControllerColliderHit hit)
    //{


    //}

}