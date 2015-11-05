using UnityEngine;
using System.Collections;

public class RocketShoot : MonoBehaviour {

	public Transform GunPosition;
	BulletManager bmr;
	public float BulletCD = 0.35f;
	public BulletType BType = BulletType.Default;
	float timer;

	// Use this for initialization
	void Start () 
	{
		bmr = FindObjectOfType<BulletManager>();
	}
	
	// Update is called once per frame
	void Update () 
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
