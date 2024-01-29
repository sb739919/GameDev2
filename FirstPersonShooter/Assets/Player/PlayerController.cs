using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;


public class NewBehaviourScript : MonoBehaviour

{
    //Camera Variables
    public Camera cam;
    private Vector2 look_input = Vector3.zero;
    private float look_speed = 60;
    private float horizontal_look_angle = 0f;

    //invert Cam
    public bool invert_x=false;
    public bool invert_y=false;
    private int invert_factor_x = 1;
    private int invert_factor_y = 1;

    [Range(0.01f, 1f)] public float sensitivity;

    //Movement Variables
    public float max_speed = 10f;
    private Vector2 move_input = Vector2.zero;
    private CharacterController character_controller;
    private Vector3 player_velocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
       //Hide mouse
       Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Inverting Camera
        if (invert_x) invert_factor_x = -1;
        if (invert_y) invert_factor_y = -1;


    }

    // Update is called once per frame
    void Update()
    {
        Look();
        Move();

    }

    public void GetLookInput(InputAction.CallbackContext context)
    {

        look_input = context.ReadValue<Vector2>();
        
    }
    public void GetMoveInput(InputAction.CallbackContext context)
    {
        move_input = context.ReadValue<Vector2>();
    }

    private void Look()
    {
        //Left/Right
        transform.Rotate(Vector2.up, look_input.x * look_speed * Time.deltaTime * invert_factor_x * sensitivity);

        //Up/Down
        float angle = look_input.y * look_speed * Time.deltaTime * invert_factor_y * sensitivity;
        horizontal_look_angle -= angle;
        horizontal_look_angle = Mathf.Clamp(horizontal_look_angle, -90, 90);
        cam.transform.localRotation = Quaternion.Euler(horizontal_look_angle, 0, 0);

    }

    private void Move()
    { 
    
        //Set player velocity
        player_velocity = (transform.right * move_input.x + transform.forward * move_input.y) * max_speed;

        //Move Player
        character_controller.Move(player_velocity * Time.deltaTime);
    
    }


    
}


