using UnityEngine;
using System.Collections;

public class RocketShoot : MonoBehaviour {

	public Transform GunPosition;
	BulletManager bmr;
	public float BulletCD = 0.35f;
	float baseCD;
	float cdTime;
	public BulletType BType = BulletType.Default;
	float timer;
    public bool canShoot = false;

	PhotonView pv;

	// Use this for initialization
	void Start () 
	{
		bmr = FindObjectOfType<BulletManager>();
		pv = GetComponent<PhotonView>();
		baseCD = BulletCD;
        //canShoot = GetComponent<RocketControl>().canMove;
	}

	public void CooldownEffect(float newCD, float time)
	{
		BulletCD = newCD;
		cdTime = time;
	}
	
	// Update is called once per frame
	void Update () 
	{
        if(canShoot)
        {

			if(cdTime > 0)
				cdTime -= Time.deltaTime;
			else
				BulletCD = baseCD;
	        if(timer > 0)
	        {
		        timer -= Time.deltaTime;
	        }
	        else
	        {
		        if(Input.GetMouseButton(0))
		        {
			        timer = BulletCD;
			        bmr.Shoot(BType, GunPosition.position, transform.eulerAngles.z, GetComponent<Rigidbody2D>().velocity, pv.ownerId);
		        }
	        }
        }
	}
}
