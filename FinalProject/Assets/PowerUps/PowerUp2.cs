using UnityEngine;
using System.Collections;

public class PowerUp2 : MonoBehaviour {
    
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Collision with PowerUp 2");
        collision.gameObject.GetComponent<RocketShoot>().BulletCD = 0.05f;
        Destroy(this.gameObject);
    }
}
