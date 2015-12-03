using UnityEngine;
using System.Collections;

public class RocketHealth : MonoBehaviour {

	public int MaxHealth;
	public int MaxShield;
	public int curShield;
	public int currentHealth;
	PhotonView pView;

	public GameObject ExplEffect;
	public GameObject SpawnEffect;

	public SpriteRenderer Highlights;

	public bool invincible;

	public Transform[] SpawnPoints;

	RocketControl rc;
	bool hasPoints;

	public GameObject ShieldView;

	
	void Start () {
		pView = GetComponent<PhotonView>();
		rc = GetComponent<RocketControl>();
		if(pView.isMine)
			Respawn();

	}

	void Update()
	{
		if(!hasPoints)
		{
			GameObject[] Spoints = GameObject.FindGameObjectsWithTag("SpawnPoint");
			if(Spoints.Length > 0)
			{
				SpawnPoints = new Transform[Spoints.Length];
				for(int i=0; i<Spoints.Length; i++)
				{
					SpawnPoints[i] = Spoints[i].transform;
				}
				hasPoints = true;
			}
		}
	}

	public void doDamage(int dmg, int playerID)
	{
		Debug.Log("Dmg: " + dmg);
		curShield -= dmg;
		if(curShield <= 0)
		{
			if(playerID != PhotonNetwork.player.ID)
				FindObjectOfType<ScoreKeeper>().ChangeScore(playerID, 20);
			currentHealth += curShield;
			curShield = 0;
			ShieldView.SetActive(false);
		}
		else
		{
			if(playerID != PhotonNetwork.player.ID)
				FindObjectOfType<ScoreKeeper>().ChangeScore(playerID, 15);
		}

		if(currentHealth <= 0)
		{
			if(playerID != PhotonNetwork.player.ID)
				FindObjectOfType<ScoreKeeper>().ChangeScore(playerID, 80);
			else
				FindObjectOfType<ScoreKeeper>().ChangeScore(playerID, -50);
			rc.disableMovement();
			GetComponent<RocketShoot>().canShoot = false;
			StartCoroutine("ResAfterTime", 1.5f);
			pView.RPC("Explode", PhotonTargets.All);

		}
	}

	public void AddShield(int amount)
	{
		ShieldView.SetActive(true);
		curShield = Mathf.Min(curShield+amount, MaxShield);

	}

	public void heal(int amount)
	{
		currentHealth = Mathf.Min(currentHealth+amount, MaxHealth);
	}

	public void Respawn()
	{
		//Reset position to a new spawn point
		invincible = true;
		rc.canMove = true;
		curShield = 0;
		pView.RPC("Reset", PhotonTargets.All);
		GetComponent<RocketShoot>().BType = BulletType.Default;
		GetComponent<RocketShoot>().canShoot = true;
		currentHealth = MaxHealth;
	}

	IEnumerator DisableInvince(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		invincible = false;
	}

	IEnumerator ResAfterTime(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		Respawn();
	}

	[PunRPC]
	void Reset()
	{
		FarthestSpawnPoint();
		//Enable Ship Collision
		GetComponent<Rigidbody2D>().isKinematic = false;
		GetComponent<PolygonCollider2D>().enabled = true;

		//Instantiate(SpawnEffect, transform.position - Vector3.forward, Quaternion.identity);
		GetComponent<SpriteRenderer>().enabled = true;
		Highlights.enabled = true;
	}

	void FarthestSpawnPoint()
	{
		if(SpawnPoints.Length > 0)
		{
			float maxDist = 0;
			int curIndex = 0;
			for(int i=0; i<SpawnPoints.Length; i++)
			{
				float d = Vector2.Distance(SpawnPoints[i].position, transform.position);
				if(d > maxDist)
				{
					maxDist = d;
					curIndex = i;
				}
			}
			transform.position = SpawnPoints[curIndex].position;
		}
		else
		{
			transform.position = Vector3.zero;
		}
	}

	[PunRPC]
	void Explode()
	{
		//Disable ship collision
		FindObjectOfType<VectorGrid>().AddGridForce(transform.position,2,3,Color.red,true);
		GetComponent<Rigidbody2D>().isKinematic = true;
		GetComponent<PolygonCollider2D>().enabled = false;
		Instantiate(ExplEffect, transform.position- Vector3.forward, Quaternion.identity);
		GetComponent<SpriteRenderer>().enabled = false;
		Highlights.enabled = false;
	}
	
}
