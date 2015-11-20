using UnityEngine;
using System.Collections;

public class BulletCDPowerup : Powerup
{
    public int buffTimer = 15;

	public override void DoEffect(GameObject target)
	{
		target.GetComponent<RocketShoot>().CooldownEffect(0.1f, 5f);
	}

}
