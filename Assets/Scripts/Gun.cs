using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Gun : MonoBehaviour
{
    public float m_damage = 10f; //damage made by one bullet
    public float range = 100f; //range of the gun 
    public float m_impactForce = 0f; //to add a force on the target when its hit

    public ParticleSystem m_MuzzleFlash; //muzzleflash of the gun

    public SteamVR_Action_Boolean grabPinch; //action related to the trigger on the controller
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any; //controller

    //raycast parameter
    public Transform m_Muzzle; //muzzle of the gun 
    public Transform m_Sight; //sight of the gun
    public GameObject m_ImpactEffect; //game object related to the impact of the bullet with the target

    private bool shooting;
    private float time_cooler = 0f;

    public AudioSource m_shootSound; //sound of a shooting

    public GameObject bullet; // gameobject related to the bullet going out from the gun
    public float bullet_force = 1000f;//force to the bullet


    void Update()
    {
        time_cooler = time_cooler + Time.deltaTime;
        grabPinch.UpdateValues();
        if (grabPinch.lastStateDown && time_cooler >0.05f)
        {
            time_cooler = 0;
            shooting = true;
        }

        if(grabPinch.lastStateUp)
        {
            time_cooler = 0;
            shooting = false;
        }
        if (shooting)
        {
            Shoot();
            m_shootSound.Play();
            shooting = false;
        }

    }

    void Shoot()
    {
        m_MuzzleFlash.Play();
        Vector3 direction = m_Sight.position - m_Muzzle.position; // direction of where the gun is pointing
        Ray ray = new Ray(m_Muzzle.position, direction);
        RaycastHit m_Hit; //raycast for checking the direction of the rayù

        Vector3 bullet_direction = direction;
        bullet_direction.z = bullet_direction.z + 90;
        GameObject m_bullet  = (GameObject)Instantiate(bullet, m_Muzzle.position, Quaternion.Euler(bullet_direction) );
        m_bullet.name = "bullet";

        Rigidbody rb_bullet = m_bullet.GetComponent<Rigidbody>();
        rb_bullet.AddForce(direction*bullet_force);

        if(Physics.Raycast(ray, out m_Hit)) //collision
        {
            //Debug.Log(m_Hit.collider.transform.name);
            Debug.Log(m_Hit.collider.transform);
            Zombunny m_zombunny = m_Hit.collider.transform.GetComponent<Zombunny>(); // component from the target
            if(m_zombunny != null)
            {
                m_zombunny.TakeDamage(m_damage);
            }

            GameObject m_impact = Instantiate(m_ImpactEffect, m_Hit.point, Quaternion.LookRotation(m_Hit.normal));
            Destroy(m_impact, 2f); //Destroy the object after 2 '

            Destroy(m_bullet, 1f);
        }


    }
}