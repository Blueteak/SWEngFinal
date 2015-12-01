using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour {

	public List<Score> GameScores;
	public Text ScoreLabel;

	public void ChangeScore(int id, int change)
	{
		Debug.Log("Changed Score");
		string s = id+":"+change;
		GetComponent<PhotonView>().RPC("Adjust", PhotonTargets.All,s);
	}

	void Update()
	{
		if(GameScores.Count > 0 && PhotonNetwork.inRoom)
		{
			string s = "";
			GameScores.Sort((x, y) => x.currentScore.CompareTo(y.currentScore));
			foreach(var v in GameScores)
			{
				string name = v.Name.Split(':')[0];
				s+= name + ":  " + v.currentScore;
			}
			ScoreLabel.text = s;
		}
		else
			ScoreLabel.text = "";
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

	void OnPlayerConnected()
	{
		Debug.Log("Player Disconnected");
		UpdateList();
	}

	void OnlayerJoined()
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

