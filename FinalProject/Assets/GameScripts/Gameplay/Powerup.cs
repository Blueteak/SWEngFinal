using UnityEngine;
using System.Collections;

[RequireComponent (typeof(PhotonView))] // Synced in network
public class Powerup : MonoBehaviour {

	public float RespawnTime;
    bool pickedUp = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (pickedUp == false && collision.tag == "myShip")
        {
			DoEffect(collision.gameObject);
			GetComponent<PhotonView>().RPC("PickupObj", PhotonTargets.All);
        }
        Invoke("RespawnPowerup", 5.0f);
    }

	[PunRPC]
	public virtual void PickupObj()
	{
		GetComponent<Renderer>().enabled = false;
		pickedUp = true;
	}

    void RespawnPowerup()
    {
        this.GetComponent<Renderer>().enabled = true;
        pickedUp = false;
    }

	public virtual void DoEffect(GameObject target)
	{
		Debug.Log("Powerup Taken!");
	}
}
