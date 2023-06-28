using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class PuzzleBehaviour : MonoBehaviour
{

    [SerializeField] private bool PuzzleSolved;

    public List<Light> puzzleLights;
    public GameObject[] buttons;

    [SerializeField] private int NoOfLights;
   
    [SerializeField] private int lightsOn;

    public UnityEvent puzzleComplete;

    // Start is called before the first frame update
    
    // Counts the lights when the game starts
    void Start()
    {
        NoOfLights = puzzleLights.Count;
        CountLights();
    }
   // everytime a button is pressed it makes sure all the lights are on
    public void CountLights()
    {
        //Sets lights to zero to make sure it only counting th the current lights on and not the lights that used to be on
        lightsOn = 0;

        //Counts each light to make sure it's on
        foreach (Light light in puzzleLights)
        {
           if (light.gameObject.activeSelf)
            {
                lightsOn++;
                Debug.Log(lightsOn);
                Debug.Log("on");
           }
           else
           {
              if (lightsOn > 0)
              {
                   lightsOn--;
                   Debug.Log(lightsOn);
                   Debug.Log("off");
              }
           }


        }
    }

    void Update()
    {
        // Sets the puzzle solved to false if the number of lights and the amount of lights on are not equal
        if (lightsOn != NoOfLights)
        {
            PuzzleSolved = false;
        }
        // makes sure the amount of lights on never goes below zero
        if (lightsOn <= 0)
        {
            lightsOn = 0;
        }
        // It's redundant now but this makes sure the amount of lights on, never goes over the current amount of lights
        if (lightsOn > NoOfLights)
        {
            lightsOn = NoOfLights;
        }
        //Checks if the puzzle is solved and fires a game event if it's true
        if (lightsOn == NoOfLights && PuzzleSolved == false)
        {
            Debug.Log("puzzle complete");
            PuzzleSolved = true;
            puzzleComplete.Invoke();
        }
        //Turns off the buttons if the puzzle is solved so the player knows the puzzle is solved
        if (PuzzleSolved == true)
        {
            foreach (GameObject gameObject in buttons)
            {
                gameObject.SetActive(false);
            }
        }
    }
}
