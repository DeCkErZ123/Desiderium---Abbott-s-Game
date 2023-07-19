using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AiLocomotion : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> smallMazeDestinations;
    public List<Transform> largeMazeDestinations;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdletime, idleTime, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime, sightDistance;
    public bool walking, chasing;

    public Animator aiAnim;

    public LayerMask ignoreLayer;

    public Transform playerTransform;
    public Transform lastHeardPosition;
    Transform currentDest;
    Vector3 dest;
    int randomNum, randomNum2;
    public Vector3 rayCastOffset;
    public Vector3 rayCastTargetOffset;
    public string deathScene;
    public Ray ray;
    float aiDistance;
    public float aiDistanceToNoise;

    private bool walkingFootsteps;
    private bool chasingFootsteps;

    public GameObject deathPannel;
    private void Start()
    {
        walking = true;
        randomNum = Random.Range(0, smallMazeDestinations.Count);
        currentDest = smallMazeDestinations[randomNum];
        walkingFootsteps = false;
    }

    public void NoiseHeard()
    {
        Debug.Log("Monster Reacts To Noise");
        
        if (chasing == false)
        {
            lastHeardPosition.position = new Vector3(playerTransform.position.x, lastHeardPosition.position.y, playerTransform.position.z);
            currentDest = lastHeardPosition;
            currentDest.position = lastHeardPosition.position;
            walking = true;
        }
    }

    void Update()
    {
        Vector3 direction = ((playerTransform.position + rayCastTargetOffset) - transform.position).normalized;
       
        RaycastHit hit;

        aiDistance = (Vector3.Distance(playerTransform.position, this.transform.position));
        aiDistanceToNoise = (Vector3.Distance(lastHeardPosition.position, this.transform.position));

        if (Physics.Raycast(transform.position + rayCastOffset , direction * sightDistance, out hit, ignoreLayer ))
        {
            Debug.DrawRay(transform.position + rayCastOffset, direction * sightDistance);
            if (hit.collider.gameObject.tag == "Player" && MonsterSightline.playerInView == true || hit.collider.gameObject.tag == "Player" && MonsterFlashed.monsterFlashed == true)
            {
                walking = false;
                StopCoroutine("stayIdle");
                StopCoroutine("chaseRoutine");
                StartCoroutine("chaseRoutine");
                chasing = true;
            }
        } 
        
           

        if (chasing == true)
        {
            dest = playerTransform.position;
            ai.destination = dest;
            ai.speed = chaseSpeed;
            aiAnim.ResetTrigger("walk");
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("sprint");
            walkingFootsteps = false;
            FindObjectOfType<AudioManager>().StopSound("Monster Walk");
            if (chasingFootsteps == false)
            {
                FindObjectOfType<AudioManager>().PlaySound("Monster Run");
                chasingFootsteps = true;
            }
            if (aiDistance <= catchDistance)
            {
                playerTransform.gameObject.SetActive(false);
                aiAnim.ResetTrigger("walk");
                aiAnim.ResetTrigger("idle");
                aiAnim.ResetTrigger("sprint");
                aiAnim.SetTrigger("jumpscare");
                StartCoroutine(deathRoutine());
                chasing = false;
            }
        }

        if (walking)
        {
            dest = currentDest.position;
            ai.destination = dest;
            ai.speed = walkSpeed;
            aiAnim.ResetTrigger("sprint");
            aiAnim.ResetTrigger("idle");
            aiAnim.SetTrigger("walk");
            chasingFootsteps = false;
            FindObjectOfType<AudioManager>().StopSound("Monster Run");
            if (walkingFootsteps == false)
            {
                FindObjectOfType<AudioManager>().PlaySound("Monster Walk");
                walkingFootsteps=true;
            }

            if (ai.remainingDistance <= ai.stoppingDistance || currentDest == lastHeardPosition && aiDistanceToNoise <= 2)
            {
                randomNum2 = Random.Range (0,2);
                if (randomNum2 == 0)
                {
                    if (Fpsmovment.inSmallMaze == true)
                    {
                        randomNum = Random.Range(0, smallMazeDestinations.Count);
                        currentDest = smallMazeDestinations[randomNum];
                    }
                    else
                    {
                        randomNum = Random.Range(0, largeMazeDestinations.Count);
                        currentDest = largeMazeDestinations[randomNum];
                    }
                }
                if(randomNum2 == 1)
                {
                    aiAnim.ResetTrigger("sprint");
                    aiAnim.ResetTrigger("walk");
                    aiAnim.SetTrigger("idle");
                    ai.speed = 0;
                    StopCoroutine("stayIdle");
                    StartCoroutine("stayIdle");
                    walking = false;
                    FindObjectOfType<AudioManager>().StopSound("Monster Walk");
                    walkingFootsteps = false;
                }
            }
        }
    }
    IEnumerator stayIdle()
    {
        idleTime = Random.Range (minIdleTime, maxIdletime);
        yield return new WaitForSeconds (idleTime);
        walking = true;
        if (Fpsmovment.inSmallMaze == true)
        {
            randomNum = Random.Range(0, smallMazeDestinations.Count);
            currentDest = smallMazeDestinations[randomNum];
        }
        else
        {
            randomNum = Random.Range(0, largeMazeDestinations.Count);
            currentDest = largeMazeDestinations[randomNum];
        }
    }
    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range (minChaseTime, maxChaseTime);
        yield return new WaitForSeconds (chaseTime);
        walking = true;
        chasing = false;
        if (Fpsmovment.inSmallMaze == true)
        {
            randomNum = Random.Range(0, smallMazeDestinations.Count);
            currentDest = smallMazeDestinations[randomNum];
        }
        else
        {
            randomNum = Random.Range(0, largeMazeDestinations.Count);
            currentDest = largeMazeDestinations[randomNum];
        }
        aiAnim.ResetTrigger("sprint");
        aiAnim.SetTrigger("walk");
    }
    IEnumerator deathRoutine()
    {
        FindObjectOfType<AudioManager>().StopSound("Monster Walk");
        FindObjectOfType<AudioManager>().StopSound("Monster Run");
        FindObjectOfType<AudioManager>().PlaySound("Death Noise");
        yield return new WaitForSeconds(jumpscareTime);
        for (float i = 0; i <= 1; i += Time.deltaTime)
        {
            // set color with i as alpha
            deathPannel.GetComponent<Image>().color = new Color(0, 0, 0, i);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(deathScene);
    }
}
