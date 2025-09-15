using UnityEngine;
using UnityEngine.InputSystem;

public class FirstPersonControllerScript : MonoBehaviour
{
    /*
    1.
    Title: FIRST PERSON MOVEMENT in Unity - FPS Controller
    Author: Asbjørn Thirslund / Brackeys
    Date: 28 July 2025
    Code version: 1
    Availability: https://www.youtube.com/watch?v=_QajrabyTJc

    2.
    Title: Easy Camera Zoom in Unity 3D! 2024 Tutorial
    Author: Matt's Computer Lab
    Date: 13 August 2025
    Code version: 1
    Availability: https://www.youtube.com/watch?v=oGVbC7ooUWI
    */

    public static FirstPersonControllerScript controllerScript;

    public bool playerCanMove;

    public CharacterController controller;

    [Header("Base Movement")]
    public float speed = 12f;
    public float gravity = -9.81f;//though -19.81f seems to work better with the jump mechanic

    [Header("Jumping Mechanic")]
    public float jumpHeight = 3f;
    public float groundDistance = .4f;//radius of Sphere to be used to check ground
    public Transform groundCheck;
    public LayerMask groundMask;

    private Vector3 velocity;
    private bool isGrounded;

    [Header("Double Jump")]
    private int minJumpAmount = 1;
    private int maxJumpAmount = 2;
    private bool hasJumped;

    [Header("Mouse Look")]
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    public GameObject firstPersonCam;

    private float xRotation = 0f;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        firstPersonCam = GameObject.FindWithTag("MainCamera");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //minJumpAmount = 1;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleSight();

        if (controller.gameObject.transform.position.y <= -20)
        {
            FindAnyObjectByType<SceneManagerScript>().Restart();
        }
    }

    public void OnMovement(InputAction.CallbackContext context)
    {
        if (playerCanMove)
        {
            moveInput = context.ReadValue<Vector2>();
        }
    }

    public void HandleMovement()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);//sphere is created from groundcheck gameObject, checking that the player is grounded

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;//resets the velocity that would otherwise build continiously
        }

        Vector3 move = transform.right * moveInput.x + transform.forward *
        moveInput.y;

        controller.Move(move * speed * Time.deltaTime);//"Time.deltaTime" means that the movement speed is now framerate independent

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(groundCheck.transform.position, groundDistance);
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }

    public void HandleSight()
    {
        float mouseX = lookInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = lookInput.y * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);//clamps the rotation, meaning, can never rotate till camera view inverts

        firstPersonCam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);//quaternions are responsible for rotations
        playerBody.Rotate(Vector3.up * mouseX);
    }

    public void OnJump(InputAction.CallbackContext context)
    {
        if (context.performed && isGrounded && playerCanMove)
        {
            
            hasJumped = true;
            minJumpAmount++;
            

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);

            FindAnyObjectByType<AudioManagerScript>().Play("Jump");
        }
        else if (context.performed && hasJumped && isGrounded != true && minJumpAmount >= maxJumpAmount)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            minJumpAmount = 1;
            hasJumped = false;

            FindAnyObjectByType<AudioManagerScript>().Play("Jump");
        }
    }
}
