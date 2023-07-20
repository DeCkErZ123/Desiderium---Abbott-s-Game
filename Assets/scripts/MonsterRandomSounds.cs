using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterRandomSounds : MonoBehaviour
{
    public AiLocomotion AiLocomotion;
    public bool waiting;
    public int lastSound;
    public int randomNum;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (AiLocomotion.chasing == false)
        {
            if (waiting == false)
            {
                waiting = true;
                StartCoroutine(RandomSound());
            }
        }
    }

    IEnumerator RandomSound()
    {
        yield return new WaitForSeconds(Random.Range(7, 13));
        if (AiLocomotion.chasing == false)
        {
            randomNum = Random.Range(1, 3);
            while (randomNum == lastSound)
            {
                randomNum = Random.Range(1, 4);
            }
            lastSound = randomNum;
            switch (randomNum)
            {
                case 1:
                    FindObjectOfType<AudioManager>().PlaySound("Monster Ambient1");
                break;
                case 2:
                    FindObjectOfType<AudioManager>().PlaySound("Monster Ambient2");
                break ;
                case 3:
                    FindObjectOfType<AudioManager>().PlaySound("Monster Ambient3");
                break;
            }
                
        }
        waiting = false;
    }
}
