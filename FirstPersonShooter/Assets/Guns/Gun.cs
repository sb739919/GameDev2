using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
public class Gun : MonoBehaviour
{

    public GunData gun_data;
    public Camera cam;
    protected Ray ray;

    //Ammo Variable
    protected int ammo_in_clip;

    //Debug
    public TMP_Text debug_text;

    //Shooting
    protected bool primary_fire_is_shooting = false;
    protected bool primary_fire_hold = false;

    //Delay
    protected float shoot_delay_timer = 0.0f;

    //Trail and particle effects
    [SerializeField]
    protected Transform shoot_point;
    [SerializeField]
    protected TrailRenderer bullet_trail;
    [SerializeField]
    protected ParticleSystem muzzle_flash;
    [SerializeField]
    protected ParticleSystem impart_particles;





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
        
        //Shooting
        PrimaryFire();

        if (shoot_delay_timer > 0) shoot_delay_timer -= Time.deltaTime;
    }

    public void GetPrimaryFireInput(InputAction.CallbackContext context)
    {

        if (context.phase == InputActionPhase.Started)
        {
            primary_fire_is_shooting = true;
        }

        if (gun_data.automatic)
        {
            if (context.interaction is HoldInteraction && context.phase == InputActionPhase.Performed)
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


    protected virtual void PrimaryFire()
    {

    }

    protected virtual void SecondaryFire()
    {

    }

    protected IEnumerator SpawnTrail(TrailRenderer trail, Vector3 direction, RaycastHit hit)
    {
        float time = 0;
        Vector3 start_position = trail.transform.position;
        Vector3 end_position = Vector3.zero;

        if (hit.point == Vector3.zero)
        {

            end_position = start_position + (direction * 100);

        }
        else end_position = hit.point;

        while (time < 1)
        {
            trail.transform.position = Vector3.Lerp(start_position, end_position, time);
            time += Time.deltaTime / trail.time;

            yield return null;
        }
        trail.transform.position = end_position;

        if (hit.point == Vector3.zero) 
        { 
            Instantiate(impart_particles, hit.point, Quaternion.LookRotation(hit.normal));
            Destroy(trail.gameObject, trail.time);
        
        
        }

    }

}
