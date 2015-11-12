using UnityEngine;
using System.Collections;


/**
 * This class defines how the rocket moves through space using User Input
 */
public class RocketControl : MonoBehaviour {

	//Global Variables
	public bool canMove = false;
	public float MainThrust;
	public float SideThrust;

	public ParticleSystem MainRocket;
	public ParticleSystem LeftRocket;
	public ParticleSystem RightRocket;

	NetworkCharacter nchar;

	//Local Variables
	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		nchar = GetComponent<NetworkCharacter>();
	
	}

	void Update () 
	{
		if(canMove)
		{
			FaceMouse();
			DoMovement();
			DoRockets();
		}
	}

	void FaceMouse()
	{
        if (canMove)
        {
            Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
            diff.Normalize();

            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
	}

	void DoMovement()
	{
        if (canMove)
        {
            if (Input.GetAxis("Vertical") > 0)
			{
				rb.AddForce(transform.TransformDirection(Vector2.up) * Input.GetAxis("Vertical") * MainThrust);
				nchar.givenInput = true;
			}
           
			nchar.LR = Input.GetAxis("Horizontal");
            rb.AddForce(transform.TransformDirection(Vector2.right) * Input.GetAxis("Horizontal") * SideThrust);

			if(Input.GetAxis("Horizontal") == 0 &&  Input.GetAxis("Vertical") == 0)
				nchar.givenInput = false;
        }
	}

	void DoRockets()
	{
        if (canMove)
        {
            if (Input.GetAxis("Vertical") > 0)
                MainRocket.enableEmission = true;
            else
                MainRocket.enableEmission = false;

            if (Input.GetAxis("Horizontal") > 0)
                LeftRocket.enableEmission = true;
            else
                LeftRocket.enableEmission = false;

            if (Input.GetAxis("Horizontal") < 0)
                RightRocket.enableEmission = true;
            else
                RightRocket.enableEmission = false;
        }
	}
    

	public void disableMovement()
	{
		canMove = false;
	}
}
