using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class PartyMatchmaker : MonoBehaviour {

    private List<string> partyList = new List<string>();
	private string PartyLeader;

	public GameObject RandomButton;
	public GameObject CreateButton;

	public bool inParty()
	{
		return !PartyLeader.Equals("");
	}

	public void SetLeader(string user)
	{
		PartyLeader = user;
	}

	public string playerList()
	{
		if(partyList == null || partyList.Count == 0 || PartyLeader == "")
		{
			return ChatSystem.playerToText(PhotonNetwork.playerName);
		}
		string s = "";
		s += ChatSystem.playerToText(PartyLeader);
		foreach(var p in partyList)
		{
			if(p != PartyLeader)
				s += "\n" + ChatSystem.playerToText(p);
		}
		return s;
	}

    public void AddPlayerToParty(string player)
    {
		RandomButton.GetComponent<Button>().interactable = false;
		CreateButton.GetComponent<Button>().interactable = true;
        partyList.Add(player);
    }

	public void ClearParty()
	{
		PartyLeader = "";
		RandomButton.GetComponent<Button>().interactable = true;
		CreateButton.GetComponent<Button>().interactable = false;
		partyList = new List<string>();
		partyList.Add(PhotonNetwork.player.name);
	}

    public void CreateGame()
    {
		if(PhotonNetwork.player.name == PartyLeader)
		{
			PhotonNetwork.CreateRoom(PhotonNetwork.player.name, new RoomOptions() { maxPlayers = 5 }, null);
			FindObjectOfType<ChatSystem>().chatClient.Subscribe(new string[] {PhotonNetwork.player.name});
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
