using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Playables;

public class Pistol : Gun
{

    protected override void PrimaryFire()
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

                //particle
                muzzle_flash.Play();
                TrailRenderer trail = Instantiate(bullet_trail, shoot_point.position, Quaternion.identity);
                StartCoroutine(SpawnTrail(trail, dir, hit));
            }

        }
    }

    private void SecondaryFire()
    {


    }



}
