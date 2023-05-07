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
        turn += Input.GetAxis("Mouse X") * mouseSensitivity;
        camTurnVertical -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        camTurnVertical = Mathf.Clamp(camTurnVertical, minVert, maxVert);
        transform.localRotation = Quaternion.Euler(0, turn, 0);
        camera.localRotation = Quaternion.Euler(camTurnVertical, 0, 0);
        bool forward = Input.GetKey(KeyCode.W);

        

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
                    if (currMeshRenderer)
                    {
                        currMeshRenderer.material = opaqueBoxMat;
                    }
                    obstacles.Add(currObstacle);
                    currObstacle = null;
                }
            }
            if (!currObstacle)
            {
                currObstacle = Instantiate(obstaclePrefab);
                //currMeshRenderer = currObstacle.GetComponent<MeshRenderer>();
                currMeshRenderer = currObstacle.GetChild(0).GetComponent<MeshRenderer>();
                if (currMeshRenderer)
                {
                    currMeshRenderer.material = transparentBoxMat;
                }
                else
                {
                    Debug.Log("No MeshRenderer found");
                }
                currObstacle.position = transform.position + transform.forward * 60f;
            }
            currObstacle.position += transform.right * Input.GetAxis("Horizontal");
            currObstacle.position += transform.forward * Input.GetAxis("Vertical");
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {

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


    // if setToTransparent is true, then set to transparent; else set to opaque
    //void SwitchToTransparent(Material material, bool setToTransparent)
    //{
    //    // code from StandardShaderGUI.cs (see https://docs.unity3d.com/Manual/StandardShaderMaterialParameterRenderingMode.html)
    //    //int minRenderQueue = -1;
    //    //int maxRenderQueue = 5000;
    //    //int defaultRenderQueue = -1;
    //    if (setToTransparent)
    //    {
    //        material.SetOverrideTag("RenderType", "Transparent");
    //        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
    //        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //        material.SetFloat("_ZWrite", 0.0f);
    //        material.DisableKeyword("_ALPHATEST_ON");
    //        material.DisableKeyword("_ALPHABLEND_ON");
    //        material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
    //        //minRenderQueue = (int)UnityEngine.Rendering.RenderQueue.GeometryLast + 1;
    //        //maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Overlay - 1;
    //        //defaultRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    //    }
    //    else
    //    {
    //        material.SetOverrideTag("RenderType", "");
    //        material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
    //        material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
    //        material.SetFloat("_ZWrite", 1.0f);
    //        material.DisableKeyword("_ALPHATEST_ON");
    //        material.DisableKeyword("_ALPHABLEND_ON");
    //        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //        //minRenderQueue = -1;
    //        //maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest - 1;
    //        //defaultRenderQueue = -1;
    //    }
    //    //if (overrideRenderQueue || material.renderQueue < minRenderQueue || material.renderQueue > maxRenderQueue)
    //    //{
    //    //    if (!overrideRenderQueue)
    //    //        Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "Render queue value outside of the allowed range ({0} - {1}) for selected Blend mode, resetting render queue to default", minRenderQueue, maxRenderQueue);
    //    //    material.renderQueue = defaultRenderQueue;
    //    //}
    //}


    //public static void SetupMaterialWithBlendMode(Material material, BlendMode blendMode, bool overrideRenderQueue)
    //{
    //    int minRenderQueue = -1;
    //    int maxRenderQueue = 5000;
    //    int defaultRenderQueue = -1;
    //    switch (blendMode)
    //    {
    //        case BlendMode.Opaque:
    //            material.SetOverrideTag("RenderType", "");
    //            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
    //            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
    //            material.SetFloat("_ZWrite", 1.0f);
    //            material.DisableKeyword("_ALPHATEST_ON");
    //            material.DisableKeyword("_ALPHABLEND_ON");
    //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //            minRenderQueue = -1;
    //            maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest - 1;
    //            defaultRenderQueue = -1;
    //            break;
    //        case BlendMode.Cutout:
    //            material.SetOverrideTag("RenderType", "TransparentCutout");
    //            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
    //            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.Zero);
    //            material.SetFloat("_ZWrite", 1.0f);
    //            material.EnableKeyword("_ALPHATEST_ON");
    //            material.DisableKeyword("_ALPHABLEND_ON");
    //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //            minRenderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
    //            maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.GeometryLast;
    //            defaultRenderQueue = (int)UnityEngine.Rendering.RenderQueue.AlphaTest;
    //            break;
    //        case BlendMode.Fade:
    //            material.SetOverrideTag("RenderType", "Transparent");
    //            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.SrcAlpha);
    //            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //            material.SetFloat("_ZWrite", 0.0f);
    //            material.DisableKeyword("_ALPHATEST_ON");
    //            material.EnableKeyword("_ALPHABLEND_ON");
    //            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
    //            minRenderQueue = (int)UnityEngine.Rendering.RenderQueue.GeometryLast + 1;
    //            maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Overlay - 1;
    //            defaultRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    //            break;
    //        case BlendMode.Transparent:
    //            material.SetOverrideTag("RenderType", "Transparent");
    //            material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
    //            material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
    //            material.SetFloat("_ZWrite", 0.0f);
    //            material.DisableKeyword("_ALPHATEST_ON");
    //            material.DisableKeyword("_ALPHABLEND_ON");
    //            material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
    //            minRenderQueue = (int)UnityEngine.Rendering.RenderQueue.GeometryLast + 1;
    //            maxRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Overlay - 1;
    //            defaultRenderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    //            break;
    //    }

    //    if (overrideRenderQueue || material.renderQueue < minRenderQueue || material.renderQueue > maxRenderQueue)
    //    {
    //        if (!overrideRenderQueue)
    //            Debug.LogFormat(LogType.Log, LogOption.NoStacktrace, null, "Render queue value outside of the allowed range ({0} - {1}) for selected Blend mode, resetting render queue to default", minRenderQueue, maxRenderQueue);
    //        material.renderQueue = defaultRenderQueue;
    //    }
    //}

}