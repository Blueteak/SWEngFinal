using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PartyList : MonoBehaviour {

	public Text listText;
	PartyMatchmaker mmr;


	void Start()
	{
		mmr = FindObjectOfType<PartyMatchmaker>();
	}
	// Update is called once per frame
	void Update () 
	{
		if(PhotonNetwork.connected)
			listText.text = mmr.playerList();
	}
}
