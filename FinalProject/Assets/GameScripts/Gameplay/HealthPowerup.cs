using UnityEngine;
using System.Collections;

public class HealthPowerup : MonoBehaviour {
    bool pickedUp = false;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUp == false)
        {
            Debug.Log("Shield Picked Up");
            collision.gameObject.GetComponent<RocketHealth>().currentHealth += 50;
            this.GetComponent<Renderer>().enabled = false;
            pickedUp = true;
        }
        Invoke("RespawnPowerup", 5.0f);

    }

    void RespawnPowerup()
    {
        this.GetComponent<Renderer>().enabled = true;
        pickedUp = false;
    }
}
