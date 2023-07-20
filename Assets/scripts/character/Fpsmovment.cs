using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

public class Fpsmovment : MonoBehaviour
{
    public KeyCode m_forward;
    public KeyCode m_back;
    public KeyCode m_left;
    public KeyCode m_right;
    public KeyCode m_sprint;
    public KeyCode m_jump;

    public float m_movementSpeed = 12f;
    public float m_runSpeed = 1.5f;

    public float m_gravity = -9.81f;
    private Vector3 m_velocity;
    private float m_finalSpeed = 0;
    public UnityEngine.CharacterController m_charControler;

    public float m_jumpHeight = 3f;
    public Transform m_groundCheckPoint;
    public float m_groundDistance = 0.4f;
    public LayerMask m_groundMask;
    private bool m_isGrounded;

    public GameObject smallMazeZone;
    public GameObject smallMazeZone2;
    public static bool inSmallMaze;

    private bool walkingFootsteps;
    private bool runningFootsteps;

    public float startTime;
    public float runningTime;
    public bool exhausted;
    //public GameObject soundVolume;



    private void Awake()
    {
        m_finalSpeed = m_movementSpeed;
        m_isGrounded = HitGroundCheck();
        walkingFootsteps = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject == smallMazeZone || other.gameObject == smallMazeZone2)
        {
            inSmallMaze = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == smallMazeZone)
        {
            inSmallMaze = false;
        }
    }
    // Update is called once per frame
    void Update()
    {
        m_isGrounded = HitGroundCheck(); // CHecks touching the ground every frame
        MoveInputCheck();
    }
    void MoveInputCheck()
    {
        float x = Input.GetAxis("Horizontal"); // Gets the x input value 
        float z = Input.GetAxis("Vertical"); // Gets the z input value 

        Vector3 move = Vector3.zero;

        if (Input.GetKey(m_forward) || Input.GetKey(m_back) || Input.GetKey(m_left) || Input.GetKey(m_right))
        {
            move = transform.right * x + transform.forward * z; // calculate the move vector (direction)          
        }
        else
        {
            FindObjectOfType<AudioManager>().StopSound("Player Walk");
            FindObjectOfType<AudioManager>().StopSound("Player Run");
            walkingFootsteps = false;
            runningFootsteps = false;
        }

        if (NoiseLevelManager.running == false && walkingFootsteps == false)
        {
            FindObjectOfType<AudioManager>().PlaySound("Player Walk");
            walkingFootsteps=true;
        }
        else if (NoiseLevelManager.running == true && runningFootsteps == false)
        {
            FindObjectOfType<AudioManager>().PlaySound("Player Run");
            runningFootsteps=true;
        }
        MovePlayer(move); // Run the MovePlayer function with the vector3 value move
        RunCheck(); // Checks the input for run
        JumpCheck(); // Checks if we can jump

    }
    void MovePlayer(Vector3 move)
    {
        m_charControler.Move(move * m_finalSpeed * Time.deltaTime); // Moves the Gameobject using the Character Controller
        
        m_velocity.y += m_gravity * Time.deltaTime; // Gravity affects the jump velocity
        m_charControler.Move(m_velocity * Time.deltaTime); //Actually move the player up

    }

    void RunCheck()
    {
        if (Input.GetKeyDown(m_sprint)) // if key is down, sprint
        {
            startTime = Time.time;
            NoiseLevelManager.running = true;
            m_finalSpeed = m_movementSpeed * m_runSpeed;
            //soundVolume.SetActive(true);
        }
        if (Input.GetKey(m_sprint))
        {
            if (Time.time - startTime >= 4)
            {
                exhausted = true;
            }
        }
        if (Input.GetKeyUp(m_sprint)) // if key is up, don't sprint
        {
            if (exhausted)
            {
                exhausted=false;
                FindObjectOfType<AudioManager>().PlaySound("Player Exhausted");
            }
            NoiseLevelManager.running = false;
            FindObjectOfType<AudioManager>().StopSound("Player Run");
            m_finalSpeed = m_movementSpeed;
            //soundVolume.SetActive(false);
        }

    }

    void JumpCheck()
    {
        if (Input.GetKeyDown(m_jump))
        {
            if (m_isGrounded == true)
            {
                m_velocity.y = Mathf.Sqrt(m_jumpHeight * -2f * m_gravity);
            }

        }

    }

    bool HitGroundCheck()
    {
        bool isGrounded = Physics.CheckSphere(m_groundCheckPoint.position, m_groundDistance, m_groundMask);

        //Gravity
        if (isGrounded && m_velocity.y < 0)
        {
            m_velocity.y = -2f;
        }

        return isGrounded;
    }
}
