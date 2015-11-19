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

	public float Boost;

	public ParticleSystem MainRocket;
	public ParticleSystem LeftRocket;
	public ParticleSystem RightRocket;

	NetworkCharacter nchar;

	//Local Variables
	Rigidbody2D rb;

	VectorGrid vGrid;

	public Color TrailColor;

	void Start () {
		rb = GetComponent<Rigidbody2D>();
		nchar = GetComponent<NetworkCharacter>();
	}

	void Update () 
	{
		if(canMove)
		{
			FaceMouse();

			DoRockets();
		}
		if(vGrid != null)
			AddForce();
		else
			vGrid = FindObjectOfType<VectorGrid>();

	}


	void FixedUpdate()
	{
        rb.angularVelocity = 0;
		if(Boost > 0)
			Boost -= Time.fixedDeltaTime*Boost*0.25f;
		else
			Boost = 0;

		DoMovement();
	}

	void AddForce()
	{
		if(canMove)
			vGrid.AddGridForce(transform.position, 1, 0.9f, TrailColor, true);
		else
			vGrid.AddGridForce(transform.position, 1, 0.9f, TrailColor, false);

	}

	public void SetBoost(int boostAmount)
	{
		Boost += boostAmount;
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
				rb.AddForce(transform.TransformDirection(Vector2.up) * Input.GetAxis("Vertical") * (MainThrust + Boost) * Time.fixedDeltaTime);
				nchar.givenInput = true;
			}
         
			nchar.LR = Input.GetAxis("Horizontal");
			rb.AddForce(transform.TransformDirection(Vector2.right) * Input.GetAxis("Horizontal") * (SideThrust + Boost) * Time.fixedDeltaTime);

			if(Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
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
