using UnityEngine;
using System.Collections;

public class HealPowerup : Powerup 
{
	public int HealAmount;

	public override void DoEffect (GameObject target) 
	{
		Debug.Log("Player Healed");
		target.GetComponent<RocketHealth>().heal(50);
	}
}
