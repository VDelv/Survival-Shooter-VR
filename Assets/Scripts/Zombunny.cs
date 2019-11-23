using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Zombunny : MonoBehaviour
{
    public GameController GameControl; //gamecontroller for point inc

    public NavMeshAgent m_navmeshAgent; //navmeshagent from the zombie to control its movements
    public float health = 50f; //here we put the health in public to remotely control it 

    public Animator m_Animator; //Animator taken from the palyer

    bool isDead = false; //is the player dead or not ? 
                         //float timer = 0;

    public AudioSource m_zomb; //sound of a shooting

    public Transform ground;

    private bool score = false;

    void Start()
    {
        m_zomb.Play();
        m_navmeshAgent.SetDestination(Vector3.zero); //go to the castle in position (0,0,0)
        m_Animator.GetComponent<Animator>(); //take the value of the animator from the game object
    }

    void FixedUpdate()
    {
        if (isDead)
        {
            m_navmeshAgent.isStopped = true; //if dead stop to moove the zombunny
        }        
    }

    public void TakeDamage(float m_damage) //function called from other class -> public
    {
        //m_Getshoot.Play(); //audio
        health -= m_damage;
        if (health <= 0f) //if health <= 0 
        {
            isDead = true; //isDead
            Die();
        }
    }
    void Die()
    {
        isDead = false;
        m_zomb.Stop();
        m_Animator.SetBool("isDead", true); //animation
        Destroy(gameObject, 1.458f); //remove object after 1.5f 
        if (!score)
        {
            GameControl = GameObject.Find("GameController").GetComponent<GameController>();
            GameControl.Scored();
            score = true;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.transform.name == "bullet")
        {
            Die();
        }
    }
}
