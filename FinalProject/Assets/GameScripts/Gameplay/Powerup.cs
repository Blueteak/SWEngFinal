﻿using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PhotonView))] // Synced in network
public class Powerup : MonoBehaviour {

	public float RespawnTime;
	private float StartScale;
    public bool pickedUp = false;

	void Start()
	{
		StartScale = transform.localScale.x;
		transform.localScale = Vector3.zero;
		StartCoroutine("Spawn");
	}

    public virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUp == false && collision.tag == "myShip")
        {
			DoEffect(collision.gameObject);
			GetComponent<PhotonView>().RPC("PickupObj", PhotonTargets.All);
        }
		Invoke("RespawnPowerup", RespawnTime);
    }

	[PunRPC]
	public virtual void PickupObj()
	{
		pickedUp = true;
		StopCoroutine("Spawn");
		StartCoroutine("Despawn");
	}

	IEnumerator Despawn()
	{
		while(transform.localScale.x > 0.01f)
		{
			transform.localScale -= Time.deltaTime*Vector3.one*4*StartScale;
			yield return true;
		}
		transform.localScale = Vector3.zero;	
	}

	IEnumerator Spawn()
	{
		while(transform.localScale.x < StartScale)
		{
			transform.localScale += Time.deltaTime*Vector3.one*2*StartScale;
			yield return true;
		}
		transform.localScale = Vector3.one*StartScale;
	}

    void RespawnPowerup()
    {
		StopCoroutine("Despawn");
		StartCoroutine("Spawn");
        pickedUp = false;
    }

	public virtual void DoEffect(GameObject target)
	{
		Debug.Log("Powerup Taken!");
	}
}
