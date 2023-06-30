using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterSightline : MonoBehaviour
{
    public static bool playerInView = false;

    // Start is called before the first frame update
    
    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerInView == false)
        {
            Debug.Log("PlayerInView");
            playerInView = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("PlayerLeavesView");
            playerInView = false;
        }
    }
}
