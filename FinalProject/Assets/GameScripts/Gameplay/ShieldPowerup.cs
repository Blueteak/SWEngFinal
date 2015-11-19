using UnityEngine;
using System.Collections;

public class ShieldPowerup : Powerup {
   
	public int ShieldAmount;

	public override void OnTriggerEnter2D(Collider2D collision)
	{
		if (pickedUp == false && collision.tag == "myShip" && collision.GetComponent<RocketHealth>().curShield < collision.GetComponent<RocketHealth>().MaxShield)
		{
			DoEffect(collision.gameObject);
			GetComponent<PhotonView>().RPC("PickupObj", PhotonTargets.All);
		}
		Invoke("RespawnPowerup", RespawnTime);
	}

	public override void DoEffect(GameObject target)
	{
		target.GetComponent<RocketHealth>().AddShield(ShieldAmount);
	}

}
