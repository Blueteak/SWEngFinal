using UnityEngine;
using System.Collections;


/**
 * This class defines how the rocket moves through space using User Input
 */
public class RocketControl : MonoBehaviour {

	//Global Variables
	public bool canMove;
	public float MainThrust;
	public float SideThrust;

	public ParticleSystem MainRocket;

	//Local Variables
	Rigidbody2D rb;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
	
	}

	void Update () 
	{
		if(canMove)
		{
			FaceMouse();
			DoMovement();
		}
	}

	void FaceMouse()
	{
		Vector3 diff = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		diff.Normalize();
		
		float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
	}

	void DoMovement()
	{
		if(Input.GetAxis("Vertical") > 0)
		{
			rb.AddForce(transform.TransformDirection(Vector2.up)*Input.GetAxis("Vertical")*MainThrust);
			MainRocket.enableEmission = true;
		}
		else
			MainRocket.enableEmission = false;
			
		rb.AddForce(transform.TransformDirection(Vector2.right)*Input.GetAxis("Horizontal")*SideThrust);
	}

	public void disableMovement()
	{
		canMove = false;
	}
}
