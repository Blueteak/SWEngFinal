using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyMatchmaker : MonoBehaviour {

    private List<string> partyList = new List<string>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void AddPlayerToParty(string player)
    {
        partyList.Add(player);
    }
    public void CreateGame()
    {
        PhotonNetwork.CreateRoom(PhotonNetwork.playerName, new RoomOptions() { maxPlayers = 5 }, null);
        //need to ask others to join room
    }
}
