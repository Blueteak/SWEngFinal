using UnityEngine;
using System.Collections;

public class SpeedPowerup : Powerup {

	public int BoostAmount;
	
	public override void DoEffect(GameObject target)
	{
		target.GetComponent<RocketControl>().SetBoost(BoostAmount);
	}
}
