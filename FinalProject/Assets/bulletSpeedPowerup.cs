using UnityEngine;
using System.Collections;

public class bulletSpeedPowerup : MonoBehaviour
{

    public int buffTimer = 15;
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
        Debug.Log("Collision with Powerup");
        collision.gameObject.GetComponent<RocketShoot>().BulletCD = 0.1f;
        Destroy(this.gameObject);
    }

    
}
