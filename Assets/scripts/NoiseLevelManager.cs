using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseLevelManager : MonoBehaviour
{
    public GameObject noiseBubble;
    public Collider noiseCollider;
    public AiLocomotion monsterManager;
    public static bool rechargingLight;
    public static bool running;
    public int noiseLevel1Radius = 50;
    public int noiseLevel2Radius = 80;
    public GameObject noiseLevelLow;
    public GameObject noiseLevelMid;
    public GameObject noiseLevelHigh;
    // Start is called before the first frame update
    void Start()
    {
        noiseBubble = this.gameObject;
        noiseCollider = this.gameObject.GetComponent<Collider>();
        noiseCollider.enabled = false;
        rechargingLight = false;
        running = false;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Monster"))
        {
            Debug.Log("Noise Heard");
            monsterManager.NoiseHeard();
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (rechargingLight && running)
        {
            noiseBubble.transform.localScale = new Vector3(noiseLevel2Radius, noiseLevel2Radius, noiseLevel2Radius);
            noiseCollider.enabled = true;
            noiseLevelHigh.SetActive(true);
            noiseLevelMid.SetActive(false);
            noiseLevelLow.SetActive(false);
        }
        else if (running || rechargingLight)
        {
            noiseBubble.transform.localScale = new Vector3(noiseLevel1Radius, noiseLevel1Radius, noiseLevel1Radius);
            noiseCollider.enabled = true;
            noiseLevelHigh.SetActive(false);
            noiseLevelMid.SetActive(true);
            noiseLevelLow.SetActive(false);
        }
        else
        {
            noiseCollider.enabled = false;
            noiseLevelHigh.SetActive(false);
            noiseLevelMid.SetActive(false);
            noiseLevelLow.SetActive(true);
        }
    }
}
