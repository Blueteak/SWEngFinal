using UnityEngine;
using System.Collections;

public class RocketShoot : MonoBehaviour {

	public Transform GunPosition;
	BulletManager bmr;
	public float BulletCD = 0.35f;
	public BulletType BType = BulletType.Default;
	float timer;
    public bool canShoot = false;

	// Use this for initialization
	void Start () 
	{
		bmr = FindObjectOfType<BulletManager>();
        //canShoot = GetComponent<RocketControl>().canMove;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(canShoot)
        {
	        if(timer > 0)
	        {
		        timer -= Time.deltaTime;
	        }
	        else
	        {
		        if(Input.GetMouseButton(0))
		        {
			        timer = BulletCD;
			        bmr.Shoot(BType, GunPosition.position, transform.eulerAngles.z, GetComponent<Rigidbody2D>().velocity);
		        }
	        }
        }
	}
}
