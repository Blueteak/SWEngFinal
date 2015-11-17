using UnityEngine;
using System.Collections;

public class GameSetup : MonoBehaviour {

	public int ArenaIndex;

	public void StartGame(int index) 
	{
		ArenaIndex = index;
		FindObjectOfType<Note>().Notify("Connected","Finding open room", 1.5f);
		PhotonNetwork.JoinRandomRoom();
	}
}
