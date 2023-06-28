using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class FlashLight : MonoBehaviour
{
    public KeyCode recharge;
    public bool lightOn = false;
    public float Battery = 100f;
    static float rayLength;
    public float amount = 1;

    public int displayBattery;

    public LayerMask cubeCreatorLayer;

    [SerializeField] private int batteryDrain = 3;
    [SerializeField] private int batteryRecharge = 10;

    public GameObject Light;
    public GameObject soundVolume;
    public GameObject flashlight;
    public GameObject visionCube;
    public GameObject Monster;
    RaycastHit hit;

    public Text batteryText;

    private Vector3 hitDirection;
    private Vector3 Start, end;

    // Update is called once per frame
    void Update()
    {
        //creates and draws ray
        Ray ray = new Ray(flashlight.transform.position, flashlight.transform.forward);
        Debug.DrawRay(flashlight.transform.position, flashlight.transform.forward * rayLength, Color.red);
        Physics.Raycast(ray, out hit, cubeCreatorLayer, LayerMask.GetMask("Walls", "ground"));
        //detects if the ray has hit somethign on the correct layer
        
       
        //detects if battery is zero and turns the torch off. stops the battery from going under 0%
        if (Battery <= 0)
        {
            lightOn = false;
            Light.SetActive(false);
            Battery = 0;
        }
        //toggles the state of the flashlight
        if (Input.GetMouseButtonDown(0))
        {
            if (lightOn == false && Battery > 0)
            {
                lightOn = true;
                Light.SetActive(true);
               
            }
            else if (lightOn == true && Battery > 0)
            {
                lightOn = false;
                Light.SetActive(false);
               
            }
        }
        //drains battery
        if (lightOn == true)
        {
            Battery -= batteryDrain * Time.deltaTime;
        }
        //recharges battery and de/activates the sound detection volume
        if (Input.GetKey(recharge))
        {
            Battery += batteryRecharge * Time.deltaTime;
            soundVolume.SetActive(true);
        }
        else
        {
            soundVolume.SetActive(false);
        }
        //keeps battery from going over 100%
        if (Battery > 100)
        {
            Battery = 100;
        }
        
        if (lightOn)
        {
            if (Physics.Raycast(ray, out hit, cubeCreatorLayer, LayerMask.GetMask("Walls", "ground")))
            {
                if (visionCube == null)
                {
                    return;
                }
                //grabs distance, start and end positions as well as length
                //Debug.Log(hit.distance);
                rayLength = hit.distance;
                hitDirection = (hit.point - flashlight.transform.position).normalized;
                Start = flashlight.transform.position;
                end = hit.point;
                visionCube.SetActive(true);

                //finds where to place the objects along the line multiplying direction and distance then adding the origin. creates a cube every 1 metre along the raycast line
                for (float d = 0; d < rayLength; d += 1.0f)
                {
                    Instantiate(visionCube, Start + hitDirection * d, Quaternion.identity);
                }
            }
        }
        else if (lightOn == false)
        {
            visionCube.SetActive(false);
        }
        //displays battery percent on screen

        batteryText.text = "Battery:" + Battery + "%";

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("end"))
        {
            Monster.gameObject.SetActive(false);
        }
    }
}