using UnityEngine;
using System.Collections;

public class HealPowerup : Powerup 
{
	public int HealAmount;

	public override void OnTriggerEnter2D(Collider2D collision)
	{
		if (pickedUp == false && collision.tag == "myShip" && collision.GetComponent<RocketHealth>().currentHealth < collision.GetComponent<RocketHealth>().MaxHealth)
		{
			DoEffect(collision.gameObject);
			GetComponent<PhotonView>().RPC("PickupObj", PhotonTargets.All);
		}
		Invoke("RespawnPowerup", RespawnTime);
	}

	public override void DoEffect (GameObject target) 
	{
		Debug.Log("Player Healed");
		target.GetComponent<RocketHealth>().heal(50);
	}
}
