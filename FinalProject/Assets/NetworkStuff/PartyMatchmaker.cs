using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PartyMatchmaker : MonoBehaviour {

    private List<string> partyList = new List<string>();
	private string PartyLeader;

	public bool inParty()
	{
		return !PartyLeader.Equals("");
	}

	public void SetLeader(string user)
	{
		PartyLeader = user;
	}

    public void AddPlayerToParty(string player)
    {
        partyList.Add(player);
    }

	public void ClearParty()
	{
		PartyLeader = "";
		partyList = new List<string>();
		partyList.Add(PhotonNetwork.player.name);
	}

    public void CreateGame()
    {
		if(PhotonNetwork.player.name == PartyLeader)
		{

			PhotonNetwork.CreateRoom(PhotonNetwork.playerName, new RoomOptions() { maxPlayers = 5 }, null);
			foreach(string s in partyList)
			{
				if(s != PhotonNetwork.player.name)
					FindObjectOfType<ChatSystem>().Whisper(s, "GAME_STARTED");
			}
		}
		else
		{
			FindObjectOfType<Note>().Notify("Sorry", "Only the party leader can start the game.", 2.5f);
		}
       
    }
}
