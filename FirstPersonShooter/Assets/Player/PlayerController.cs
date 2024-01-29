using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using TMPro;

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
  private CharacterController character_controller;
    private Vector3 player_velocity;
    private Vector3 wish_dir = Vector3.zero;


    public float max_speed = 6f;
    public float accelaration = 60;
    public float gravity = 15f;
    public float stop_speed = 0.5f;
    public float jump_impulse = 10;
    public float friction = 4;

    //Debug
    public TMP_Text debug_text;

    //Player Input
    private Vector2 move_input;
    private bool grounded;

    // Start is called before the first frame update
    void Start()
    {
       //Hide mouse
       Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Inverting Camera
        if (invert_x) invert_factor_x = -1;
        if (invert_y) invert_factor_y = -1;

        //Refrence to player controller
        character_controller = GetComponent<CharacterController>();


    }

    // Update is called once per frame
    void Update()
    {
        Look();

        //Debug
        debug_text.text = "Wish Dir: " + wish_dir.ToString();
        debug_text.text += "\nPlayer Velocity: " + player_velocity.ToString();
        debug_text.text += "\nPlayer Speed: " + new Vector3(player_velocity.x, 0, player_velocity.z).magnitude.ToString();
        debug_text.text += "\nGrounded: " + grounded.ToString();
        
    }

    private void FixedUpdate()
    {
        //Find wish_dir
        wish_dir = transform.right * move_input.x + transform.forward * move_input.y;
        wish_dir = wish_dir.normalized;

        player_velocity = MoveGround(wish_dir, player_velocity);
        character_controller.Move(player_velocity * Time.deltaTime);
    }

    public void GetLookInput(InputAction.CallbackContext context)
    {

        look_input = context.ReadValue<Vector2>();
        
    }
    public void GetMoveInput(InputAction.CallbackContext context)
    {

      move_input = context.ReadValue<Vector2>();

    }
    public void GetJumpInput(InputAction.CallbackContext context)
    {

        Jump();

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
    public void Jump()
    {
        if (grounded)
        { 
        
        }
    }

    private Vector3 Accelerate(Vector3 wish_dir, Vector3 current_velocity, float accel, float max_speed)
    {

        float proj_speed = Vector3.Dot(current_velocity, wish_dir);
        float accel_speed = accel * Time.deltaTime;

        if (proj_speed + accel_speed > max_speed)
            accel = max_speed - proj_speed;

        return current_velocity + (wish_dir * accel_speed);
    }
    
    private Vector3 MoveGround(Vector3 wish_dir, Vector3 current_velocity)
    {
        Vector3 new_velocity = new Vector3(current_velocity.x,0, current_velocity.z);

        float speed = new_velocity.magnitude;
        if (speed <= stop_speed)
        {
            new_velocity = Vector3.zero;
            speed = 0;
        }

        if (speed != 0)
        {
            float drop = speed * friction * Time.deltaTime;
            new_velocity *= Mathf.Max(speed - drop, 0) / speed;
        }
        new_velocity = new Vector3(new_velocity.x, new_velocity.y, new_velocity.z);

        return Accelerate(wish_dir, new_velocity, accelaration, max_speed);

    }

    private Vector3 MoveAir(Vector3 wish_dir, Vector3 current_velocity)
    {
        return Accelerate(wish_dir, current_velocity, accelaration, max_speed);
    }

}


