using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

	public float damage;

	public float Speed;
	public GameObject HitParticle;
	public GameObject FadeParticle;

	private Vector2 addV; //Relative velocity to fireing gun
	
	public int BounceNum;
	int bounces = 0;

	public bool isDeadly; //Deadly in scene (for shots that local player fires)

	public float MaxLifetime = 3;

	public GameObject[] Children;

	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForEndOfFrame();
		InvokeRepeating("LifeTimeFade", MaxLifetime, 1);
		GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(Vector2.up)*Speed;
		GetComponent<Rigidbody2D>().velocity += addV;
	}

	//Add velocity based on ship that fired
	public void AddVel(Vector2 vel)
	{
		addV = vel;
	}

	public void setDeadly(bool b)
	{
		if(!isDeadly)
			isDeadly = b;
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
			//col.getComponent<Health>().doDamage(damage);
			Destroy(gameObject);
		}
		else if(bounces < BounceNum)
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
