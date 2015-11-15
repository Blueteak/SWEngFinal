using UnityEngine;
using System.Collections;

public class UserLogin : MonoBehaviour 
{
	
	public EMOpenCloseMotion ThisPanel;
	public EMOpenCloseMotion MenuPanel;

	public void SetUsername(string username)
	{
		if(PhotonNetwork.insideLobby)
		{
			Debug.Log("Logged in as: " + username);
			PhotonNetwork.playerName = username;
			ThisPanel.Close();
			MenuPanel.Open();
		}
		else
		{
			FindObjectOfType<Note>().Error("Not yet connected.");
		}
	}
}
