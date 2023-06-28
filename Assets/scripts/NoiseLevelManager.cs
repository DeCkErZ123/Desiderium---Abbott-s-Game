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
            noiseBubble.transform.localScale = new Vector3(70, 70, 70);
            noiseCollider.enabled = true;
        }
        else if (running || rechargingLight)
        {
            noiseBubble.transform.localScale = new Vector3(50, 50, 50);
            noiseCollider.enabled = true;
        }
        else
        {
            noiseCollider.enabled = false;
        }
    }
}
