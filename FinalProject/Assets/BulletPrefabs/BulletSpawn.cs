using UnityEngine;
using System.Collections;

/**
 * Holds bullet objects and gives them an initial velocity and
 * a flag for if they can kill local player
 **/

public class BulletSpawn : MonoBehaviour {

	public void Init(Vector2 v, bool deadly)
	{
		foreach(var o in GetComponentsInChildren<Bullet>())
		{
			o.AddVel(v);
			o.setDeadly(deadly);
			o.transform.SetParent(null);
		}
		Destroy(gameObject, 0.5f);
	}

}
