using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class Pistol : MonoBehaviour
{

    public GunData gun_data;
    public Camera cam;
    public Ray ray;

    //Ammo Variable
    private int ammo_in_clip;


    //Debug
    public TMP_Text debug_text;


    //Shooting
    private bool primary_fire_is_shooting = false;
    private bool primary_fire_hold = false;


    private float shoot_delay_timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        //Set variable
        ammo_in_clip = gun_data.ammo_per_clip;

    }

    // Update is called once per frame
    void Update()
    {
        //Debug.DrawRay(cam.transform.position, cam.transform.forward * 10000, Color.green);

        //Debug text
        debug_text.text = "Ammo in Clip" + ammo_in_clip.ToString();
        PrimaryFire();

        if (shoot_delay_timer > 0) shoot_delay_timer -= Time.deltaTime;
    }

    public void GetPrimaryFireInput(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started)
        {
            primary_fire_is_shooting = true;
        }

        if(gun_data.automatic)
        {
            if( context.interaction is HoldInteraction && context.phase == InputActionPhase.Performed) 
            { 
                primary_fire_hold = true;            
            }

           
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            primary_fire_is_shooting = false;
            primary_fire_hold = false;
        }

    }

    public void GetSecondaryFireInput(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started) SecondaryFire();

    }
    private void PrimaryFire()
    {
        if (shoot_delay_timer <= 0.0f)
        {
            if (primary_fire_is_shooting || primary_fire_hold)
            {
                shoot_delay_timer = gun_data.primary_fire_delay;
                primary_fire_is_shooting = false;

                //Raycast
                Vector3 dir = Quaternion.AngleAxis(Random.Range(-gun_data.spread, gun_data.spread), Vector3.up) * cam.transform.forward;
                dir = Quaternion.AngleAxis(Random.Range(-gun_data.spread, gun_data.spread), Vector3.right) * dir;
                ray = new Ray(cam.transform.position, dir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, gun_data.range)) ;
                {
                    Debug.DrawLine(transform.position, hit.point, Color.green, 0.05f);
                }

                //Ammo
                ammo_in_clip--;
                if (ammo_in_clip <= 0) ammo_in_clip = gun_data.ammo_per_clip;
              

            }
        }
    }

    private void SecondaryFire()
    {


    }



}
