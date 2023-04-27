//using UnityEngine;

//// Require a CharacterController component on the object.
//// If no CharacterController component has been attached, it will
//// be automatically added.
//[RequireComponent(typeof(CharacterController))]

//// Place object in a given component bin.
//[AddComponentMenu("Control Script/FPS Input")]

//public class FPSInput : MonoBehaviour
//{
//    [SerializeField] float mouseSensitivity = 6.0f;
//    [SerializeField] float gravity = -9.8f;

//    CharacterController controller;

//    void Start()
//    {
//        controller = GetComponent<CharacterController>();
//    }

//    void Update()
//    {
//        // Moving the transform via the Translate method won't engage
//        // collision detection.

//        //transform.Translate(
//        //    Input.GetAxis("Horizontal") * _speed * Time.deltaTime,
//        //    0,
//        //    Input.GetAxis("Vertical") * _speed * Time.deltaTime);

//        float deltaX = Input.GetAxis("Mouse X") * mouseSensitivity;
//        float deltaZ = Input.GetAxis("Vertical") * _speed;

//        Vector3 movement = new(deltaX, 0, deltaZ);

//        // Clamp diagonal movement
//        movement = Vector3.ClampMagnitude(movement, _speed);

//        // Apply gravity after X and Z have been clamped
//        movement.y = _gravity;

//        movement *= Time.deltaTime;

//        // Transform movement from local to global coordinates
//        movement = transform.TransformDirection(movement);

//        controller.Move(movement);
//    }
//}
