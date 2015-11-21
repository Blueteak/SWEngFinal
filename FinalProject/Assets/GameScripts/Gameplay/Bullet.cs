using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public int damage;

	public float Speed;
	public GameObject HitParticle;
	public GameObject FadeParticle;

	private Vector2 addV; //Relative velocity to fireing gun
	
	public int BounceNum;
	int bounces = 0;

	public bool isDeadly; //Deadly in scene (for shots that local player fires)

	public float MaxLifetime = 3;

	public GameObject[] Children;

	private int playerID;

	public bool affectGrid;
	VectorGrid grid;

	public Color forceColor;
	public bool useColor;
	public float Force;
	public float Radius;

	// Use this for initialization
	IEnumerator Start () 
	{
		grid = FindObjectOfType<VectorGrid>();
		yield return new WaitForEndOfFrame();
		InvokeRepeating("LifeTimeFade", MaxLifetime, 1);
		GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.up)*Speed;
		GetComponent<Rigidbody2D>().velocity += addV;
	}

	//Add velocity based on ship that fired
	public void AddVel(Vector2 vel, int id)
	{
		addV = vel;
		playerID = id;
	}

	public void setDeadly(bool b)
	{
		if(!isDeadly)
			isDeadly = b;
	}

	void Update()
	{
		if(affectGrid && grid != null)
		{
			grid.AddGridForce(transform.position, Force, Radius, forceColor, useColor);
		}
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		Debug.Log("Entered Collision: " + col.gameObject.name);
		if(col.gameObject.tag == "myShip" && isDeadly)
		{
			if(HitParticle != null)
			{
				GameObject obj = (GameObject)Instantiate(HitParticle,col.contacts[0].point,Quaternion.identity);
				obj.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity*0.5f;
			}
			col.gameObject.GetComponent<RocketHealth>().doDamage(damage, playerID);
			Destroy(gameObject);
		}
		else if(bounces < BounceNum && col.gameObject.tag != "Ship")
		{
				bounces++;
		}
		else
		{
			if(HitParticle != null)
			{
				GameObject obj = (GameObject)Instantiate(HitParticle,col.contacts[0].point,Quaternion.identity);
				obj.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity*0.5f;
			}
			Destroy(gameObject);
		}

	}

	void LifeTimeFade()
	{
		if(FadeParticle != null)
		{
			GameObject obj = (GameObject)Instantiate(FadeParticle,transform.position,Quaternion.identity);
			obj.GetComponent<Rigidbody2D>().velocity = GetComponent<Rigidbody2D>().velocity*0.5f;
		}
		Destroy(gameObject);
	}

}
