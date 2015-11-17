﻿using UnityEngine;
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

	public bool invincible;

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
		invincible = true;
		rc.canMove = true;
		curShield = 0;
		pView.RPC("Reset", PhotonTargets.All);
		currentHealth = MaxHealth;
	}

	IEnumerator DisableInvince(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		invincible = false;
	}

	IEnumerator ResAfterTime(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Respawn();
	}

	[PunRPC]
	void Reset()
	{
		transform.position = Vector3.zero;
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
		FindObjectOfType<VectorGrid>().AddGridForce(transform.position,2,3,Color.red,true);
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<PolygonCollider2D>().enabled = false;
		Instantiate(ExplEffect, transform.position- Vector3.forward, Quaternion.identity);
		GetComponent<SpriteRenderer>().enabled = false;
		Highlights.enabled = false;
	}
	
}
