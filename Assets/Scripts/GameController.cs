using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Valve.VR;


public class GameController : MonoBehaviour
{
    private int score = 0; //score screen on the window
    private float timer = 0; //timer
    private float wait_time = 2f; //time before two apparition

    public float difficulty = 10;//difficulty of the game (by the wait time variation)

    public static GameController m_instance; //reference to our game control to access it 

    public GameObject m_Zombunny; //gameobject corresponding to the zombie
    private int n_zombie = 0;

    public Text m_ScoreText; //reference to the score text representing the score
    public GameObject m_gameOverText; //defining the text appearing when the game is finished
    public bool m_GameOver = false; //is the game over ? or not ? 

    public SteamVR_Action_Boolean grabPinch; //action related to the trigger on the controller
    public SteamVR_Input_Sources inputSource = SteamVR_Input_Sources.Any; //controller


    // Start is called before the first frame update
    void Awake()
    {
        if (m_instance == null)
            m_instance = this;
        else if (m_instance != this)
            Destroy(gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        timer = timer + Time.deltaTime;

        if(m_GameOver && grabPinch.lastStateDown) //if gameover and click 
        {
            print("Reload the game");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); //reload the scence then
        }

        if(timer > wait_time && n_zombie < 25) // TO MODIFY THE apparition time
        {
            wait_time = (10f/difficulty) * 0.9f * wait_time; //decrease of the wait time 
            //initiate a random position
            timer = 0;
            float z = Random.Range(-120f, 110f); 
            float x = Random.Range(-120f, 110f);
            float distance = Mathf.Sqrt(z * z + x * x);

            if(distance > 50f) //if distance from the center is bigger enough
            {
                GameObject new_Zombie = (GameObject)Instantiate(m_Zombunny, new Vector3(x,2,z), Quaternion.identity);//instantiate the new zombie there
                new_Zombie.name = "Zombunny";
                //Debug.Log(m_Zombunny.transform.name);
                n_zombie++;
            }
        }
    }

    public void Scored()
    {
        if (m_GameOver) // if game over 
        {
            return;
        }

        n_zombie--;
        score = score +1; //the score is incrementing automaticly
        m_ScoreText.text = "Score : " + score.ToString();
        
    }

    void OnTriggerEnter(Collider other)
    {
        m_gameOverText.SetActive(true);
        m_GameOver = true;
        print(score);
    }
}
