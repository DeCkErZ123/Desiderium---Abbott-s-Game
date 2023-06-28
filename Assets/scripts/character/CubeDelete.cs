using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class CubeDelete : MonoBehaviour
{
    public GameObject cube;
   
    void Update()
    {
        Task.Delay(2000);
        Destroy(cube);
        if (cube == null)
        {
            return;
        }
    } 
}
