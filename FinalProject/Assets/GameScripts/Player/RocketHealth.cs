using UnityEngine;
using System.Collections;

public class RocketHealth : MonoBehaviour {

	public int MaxHealth;
	public int MaxShield;
	public int curShield;
	public int currentHealth;
	PhotonView pView;

	public GameObject ExplEffect;
	public GameObject SpawnEffect;

	public SpriteRenderer Highlights;

	RocketControl rc;

	
	void Start () {
		pView = GetComponent<PhotonView>();
		rc = GetComponent<RocketControl>();
		if(pView.isMine)
			Respawn();
	}

	public void doDamage(int dmg, int playerID)
	{
		Debug.Log("Dmg: " + dmg);
		curShield -= dmg;
		if(curShield < 0)
		{
			currentHealth += curShield;
			curShield = 0;
		}

		if(currentHealth <= 0)
		{
			rc.disableMovement();
			StartCoroutine("ResAfterTime", 1.5f);
			pView.RPC("Explode", PhotonTargets.All);

		}
	}

	public void AddShield(int amount)
	{
		curShield = Mathf.Min(curShield+amount, MaxShield);
	}

	public void heal(int amount)
	{
		currentHealth = Mathf.Min(currentHealth+amount, MaxHealth);
	}

	public void Respawn()
	{
		//Reset position to a new spawn point

		rc.canMove = true;
		curShield = 0;
		pView.RPC("Reset", PhotonTargets.All);
		currentHealth = MaxHealth;
	}

	IEnumerator ResAfterTime(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Respawn();
	}

	[PunRPC]
	void Reset()
	{
		//Enable Ship Collision
		GetComponent<Rigidbody2D>().isKinematic = false;
		GetComponent<PolygonCollider2D>().enabled = true;

		//Instantiate(SpawnEffect, transform.position - Vector3.forward, Quaternion.identity);
		GetComponent<SpriteRenderer>().enabled = true;
		Highlights.enabled = true;
	}

	[PunRPC]
	void Explode()
	{
		//Disable ship collision
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<PolygonCollider2D>().enabled = false;
		Instantiate(ExplEffect, transform.position- Vector3.forward, Quaternion.identity);
		GetComponent<SpriteRenderer>().enabled = false;
		Highlights.enabled = false;
	}
	
}
