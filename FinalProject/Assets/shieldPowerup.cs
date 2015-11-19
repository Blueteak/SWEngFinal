using UnityEngine;
using System.Collections;

public class shieldPowerup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with Powerup");
        collision.gameObject.GetComponent<RocketHealth>().curShield = 60;
        Destroy(this.gameObject);
    }
}
