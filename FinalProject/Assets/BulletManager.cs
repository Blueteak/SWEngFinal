using UnityEngine;
using System.Collections;


/**
 * This class manages the shooting mechanism of all ships
 * as well as maintains the different types of bullets
 **/
public class BulletManager : MonoBehaviour {

	public BulletIdentity[] BulletOptions;

	public void Shoot(BulletType type, Vector2 pos, float rot, Vector2 vel)
	{
		//Shoot bullet on server
		string s = BulletDir.toString(pos, rot, vel);
		s += ":"+type;
		//Photon pun = GetComponent<Photon>();
		//pun.RPC("ShootServer",PhotonTargets.Others,s);
		ShootClient(type,pos,rot,vel); //Shoot bullet on client

	}

	void ShootClient(BulletType type, Vector2 pos, float rot, Vector2 vel)
	{
		foreach(var v in BulletOptions)
		{
			if(v.type == type)
			{
				GameObject NewBullet = (GameObject)Instantiate(v.Prefab, pos, Quaternion.identity);
				NewBullet.transform.eulerAngles = new Vector3(0,0,rot);
				NewBullet.GetComponent<BulletSpawn>().Init(vel, false);
			}
		}
	}
	
	//[PunRPC]
	void ShootServer(string s)
	{
		BulletDir dir = new BulletDir(s);
		string[] vals = s.Split(':');
		foreach(var v in BulletOptions)
		{
			if(v.type.ToString().Equals(vals[5]))
			{
				GameObject NewBullet = (GameObject)Instantiate(v.Prefab, dir.pos, Quaternion.identity);
				NewBullet.transform.eulerAngles = new Vector3(0,0,dir.rot);
				NewBullet.GetComponent<BulletSpawn>().Init(dir.vel, false);
			}
		}
	}
}


//Bullet direction class that can be sent over network as a string
public class BulletDir
{
	public Vector2 pos;
	public float rot;
	public Vector2 vel;

	public static string toString(Vector2 p, float r, Vector2 v)
	{
		return p.x+":"+p.y+":"+r+":"+v.x+":"+v.y;
	}

	public BulletDir(string s)
	{
		string[] vals = s.Split(':');
		float x, y, vx, vy;

		float.TryParse(vals[0], out x);
		float.TryParse(vals[1], out y);
		float.TryParse(vals[2], out rot);
		float.TryParse(vals[3], out vx);
		float.TryParse(vals[4], out vy);
		pos = new Vector2(x, y);
	}
}


//Bullet Type enum for easy bullet type manipulation
[System.Serializable]
public enum BulletType
{
	Default,
	Spread,
	Rocket
}

//Link bullet type with object for spawn
[System.Serializable]
public class BulletIdentity
{
	public BulletType type;
	public GameObject Prefab;
}
