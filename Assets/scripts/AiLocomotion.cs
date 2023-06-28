using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AiLocomotion : MonoBehaviour
{
    public NavMeshAgent ai;
    public List<Transform> destinations;
    public float walkSpeed, chaseSpeed, minIdleTime, maxIdletime, idleTime, catchDistance, chaseTime, minChaseTime, maxChaseTime, jumpscareTime, sightDistance;
    public bool walking, chasing;

    public Animator aiAnim;

    public Transform playerTransform;
    Transform currentDest;
    Vector3 dest;
    int randomNum, randomNum2;
    public Vector3 rayCastOffset;
    public Vector3 rayCastTargetOffset;
    public string deathScene;
    public Ray ray;
    float aiDistance;

    private void Start()
    {
        walking = true;
        randomNum = Random.Range (0,destinations.Count);
        currentDest = destinations[randomNum];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("soundVolume")) 
        {
            chasing = true;
        }
    }

    void Update()
    {
        Vector3 direction = ((playerTransform.position + rayCastTargetOffset) - transform.position).normalized;
       
        RaycastHit hit;

        aiDistance = (Vector3.Distance(playerTransform.position, this.transform.position));

        if (Physics.Raycast(transform.position + rayCastOffset , direction * sightDistance, out hit))
        {
            Debug.DrawRay(transform.position + rayCastOffset, direction * sightDistance);
            if (hit.collider.gameObject.tag == "Player")
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

            if(ai.remainingDistance <= ai.stoppingDistance)
            {
                randomNum2 = Random.Range (0,2);
                if (randomNum2 == 0)
                {
                    randomNum = Random.Range(0, destinations.Count);
                    currentDest = destinations[randomNum];
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
                }
            }
        }
    }
    IEnumerator stayIdle()
    {
        idleTime = Random.Range (minIdleTime, maxIdletime);
        yield return new WaitForSeconds (idleTime);
        walking = true;
        randomNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randomNum];
    }
    IEnumerator chaseRoutine()
    {
        chaseTime = Random.Range (minChaseTime, maxChaseTime);
        yield return new WaitForSeconds (chaseTime);
        walking = true;
        chasing = false;
        randomNum = Random.Range(0, destinations.Count);
        currentDest = destinations[randomNum];
        aiAnim.ResetTrigger("sprint");
        aiAnim.SetTrigger("walk");
    }
    IEnumerator deathRoutine()
    {
        yield return new WaitForSeconds(jumpscareTime);
        SceneManager.LoadScene(deathScene);
    }
}
