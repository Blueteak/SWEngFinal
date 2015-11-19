using UnityEngine;
using System.Collections;

public class ShieldPowerup : Powerup {
   
	public int ShieldAmount;

	public override void DoEffect(GameObject target)
	{
		target.GetComponent<RocketHealth>().AddShield(ShieldAmount);
	}

}
