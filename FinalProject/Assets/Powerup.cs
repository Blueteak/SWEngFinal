using UnityEngine;
using System.Collections;

public class Powerup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Ontriggerenter");
        collision.gameObject.GetComponent<RocketControl>().MainThrust += 20;
        Destroy(this);
    }
}
