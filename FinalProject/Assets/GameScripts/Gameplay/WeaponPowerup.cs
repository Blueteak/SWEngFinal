using UnityEngine;
using System.Collections;

public class WeaponPowerup : Powerup {

	public BulletType[] PTypes;

	public override void DoEffect (GameObject target) 
	{
		int cur=0;
		RocketShoot rs = target.GetComponent<RocketShoot>();
		for(int i=0; i<PTypes.Length;i++)
		{
			if(PTypes[i] == rs.BType)
				cur = i;
		}
		while(PTypes[cur] == rs.BType)
		{
			cur = Random.Range(0,PTypes.Length);
		}
		rs.BType = PTypes[cur];
	}
}
