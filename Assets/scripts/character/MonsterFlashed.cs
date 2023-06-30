using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFlashed : MonoBehaviour
{
    public static bool monsterFlashed = false;
    public bool monsterInView;
    public void Update()
    {
        monsterInView = monsterFlashed;
    }

    public void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Monster") && monsterFlashed == false)
        {
            monsterFlashed = true;
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            monsterFlashed = false;
        }
    }
}
