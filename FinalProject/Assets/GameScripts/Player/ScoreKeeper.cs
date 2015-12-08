using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public class ScoreKeeper : MonoBehaviour {

	public List<Score> GameScores;
	public Text ScoreLabel;

    public float gameTimeInSeconds = 180;
    private float timeRemaining = 0;
    private bool gameStarted = false;

	public Text Timer;

	public EMOpenCloseMotion WinScreen;
	public Text WinText;

	bool ended;

    public void StartGame()
    {
        GetComponent<PhotonView>().RPC("ResetOnAllClients", PhotonTargets.All);
    }

    [PunRPC]
    void ResetOnAllClients()
    {
        timeRemaining = gameTimeInSeconds;
        gameStarted = true;
        Debug.Log("Game Start");
    }
	//Sync scores to new player when they join

	public void ChangeScore(int id, int change)
	{
		Debug.Log("Changed Score");
		string s = id+":"+change;
		GetComponent<PhotonView>().RPC("Adjust", PhotonTargets.All,s);
	}

	void Update()
	{
        if(gameStarted)
        {
            timeRemaining -= Time.deltaTime;
			if(timeRemaining < 0)
				timeRemaining = 0;
			TimeSpan t = TimeSpan.FromSeconds((int)timeRemaining);
			string timeString = string.Format("{0:D1}:{1:D2}", t.Minutes, t.Seconds);
			Timer.text = timeString;
            if(timeRemaining <= 0 && PhotonNetwork.isMasterClient && !ended)
            {
				ended = true;
				string user = GameScores[0].Name;
				int curBest = GameScores[0].currentScore;
				foreach(var v in GameScores)
				{
					if(v.currentScore > curBest)
					{
						user = v.Name;
						curBest = v.currentScore;
					}
				}
				GetComponent<PhotonView>().RPC("EndGame", PhotonTargets.All, user);
				StopCoroutine("NextMatch");
				StartCoroutine("NextMatch");
            }
        }
		else
		{
			Timer.text = "- Waiting for more players -";
		}
		if(GameScores.Count > 0 && PhotonNetwork.inRoom)
		{
			string s = "";
			GameScores.Sort((x,y) => x.Name.CompareTo(y.Name));
			GameScores.Sort((x, y) => y.currentScore.CompareTo(x.currentScore));
			foreach(var v in GameScores)
			{
				string name = v.Name.Split(':')[0];
				s+= name + ":  " + v.currentScore+"\n";
			}
			ScoreLabel.text = s;
		}
		else
			ScoreLabel.text = "";
	}

	IEnumerator NextMatch()
	{
		Debug.Log("Next game in 10 seconds");
		yield return new WaitForSeconds(10);
		GetComponent<PhotonView>().RPC("NewGame", PhotonTargets.All);
	}



	public void ResetScores()
	{
		GetComponent<PhotonView>().RPC("resetAll", PhotonTargets.All);
	}

	[PunRPC]
	void resetAll()
	{
		foreach(var s in GameScores)
		{
			s.currentScore = 0;
		}
	}

	[PunRPC]
	void EndGame(string user)
	{
		WinScreen.Open();
		WinText.text = ChatSystem.playerToText(user) + " wins the match!";
		GameObject ship = GameObject.FindGameObjectWithTag("myShip");
		ship.GetComponent<RocketControl>().canMove = false;
		ship.GetComponent<RocketShoot>().canShoot = false;
	}

	[PunRPC]
	public void NewGame()
	{
		Debug.Log("New Game Starting");
		if(PhotonNetwork.playerList.Length < 4)
		{
			Debug.Log("Canceled New Game: Player Num");
			gameStarted = false;
		}
		else
		{
			FindObjectOfType<Note>().Notify("","New Game Started!", 1f);
			ResetOnAllClients();
		}
		WinScreen.Close();
		resetAll();
		ended = false;
		GameObject ship = GameObject.FindGameObjectWithTag("myShip");
		ship.GetComponent<RocketHealth>().Respawn();
		ship.GetComponent<RocketHealth>().RandomSpawnPoint();
	}

	[PunRPC]
	void Adjust(string s)
	{
		Debug.Log("Adjust: " + s);
		string[] vals = s.Split(':');
		int id,val;
		int.TryParse(vals[0], out id);
		int.TryParse(vals[1], out val);
		for(int i=0; i<GameScores.Count; i++)
		{
			if(GameScores[i].playerID == id)
			{
				GameScores[i].currentScore += val;
			}
		}
	}

	void OnJoinedRoom()
	{
		Debug.Log("Joined Room");
		UpdateList();
	}

	void OnPhotonPlayerDisconnected()
	{
		Debug.Log("Player Connected");
		UpdateList();
	}

	void OnPhotonPlayerConnected()
	{
		Debug.Log("Player Connected");
		UpdateList();
	}

	//Refactor List to stay up to date with player list
	public void UpdateList()
	{
		if(PhotonNetwork.inRoom)
		{
			//Update existing player ids
			foreach(var p in PhotonNetwork.playerList)
			{
				bool found = false;
				foreach(var s in GameScores)
				{
					if(s.Name.Equals(p.name))
					{
						s.playerID = p.ID;
						found = true;
					}
				}
				if(!found)
				{
					Score ns = new Score();
					ns.currentScore = 0;
					ns.Name = p.name;
					ns.playerID = p.ID;
					GameScores.Add(ns);
				}
			}
			for(int i=0; i<GameScores.Count; i++)
			{
				bool found = false;
				foreach(var p in PhotonNetwork.playerList)
				{
					if(p.name.Equals(GameScores[i].Name))
						found = true;
				}
				if(!found)
					GameScores.RemoveAt(i);
			}

		}
		else
			Debug.Log("Player not in room");
	}

}

[System.Serializable]
public class Score
{
	public int currentScore;
	public string Name;
	public int playerID;
}

