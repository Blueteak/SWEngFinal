using UnityEngine;
using System.Collections;

public class UserLogin : MonoBehaviour 
{
	public ChatSystem cs;
	public EMOpenCloseMotion ThisPanel;
	public EMOpenCloseMotion MenuPanel;

	public void SetUsername(string username)
	{
		if(PhotonNetwork.insideLobby)
		{
			PhotonNetwork.playerName = username+":"+Random.Range(0,100000000);
			Debug.Log("Logged in as: " + PhotonNetwork.playerName);
			ThisPanel.Close();
			MenuPanel.Open();
			cs.LoginChat(PhotonNetwork.playerName);
		}
		else
		{
			FindObjectOfType<Note>().Error("Not yet connected.");
		}
	}
}
