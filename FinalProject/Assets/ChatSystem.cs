using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.Chat;
using System.Diagnostics;
using ExitGames.Client.Photon;
using UnityEngine.UI;
using System.Collections.Generic;

public class ChatSystem : MonoBehaviour, IChatClientListener
{
	public List<string> BlockedPlayers;

	public static string appID = "8b07b1f6-0303-490c-a8e2-b6157b7bfa81";
	public ChatDisplay LobbyChatDisplay;
	public ChatClient chatClient;
	// Use this for initialization

	void Start()
	{
		Application.runInBackground = true;
	}

	public void ToggleBlock(string user)
	{
		if(!BlockedPlayers.Contains(user))
		{
			BlockedPlayers.Add(user);
			SendSystemMessage(playerToText(user) + " has been blocked.");
		}
		else
		{
			BlockedPlayers.Remove(user);
			SendSystemMessage(playerToText(user) + " has been un-blocked.");
		}
	}

	public bool checkBlocked(string user)
	{
		return BlockedPlayers.Contains(user);
	}
	

	public void LoginChat (string username) 
	{
		FindObjectOfType<PartyMatchmaker>().ClearParty();
		chatClient = new ChatClient( this , ExitGames.Client.Photon.ConnectionProtocol.Udp);
		ExitGames.Client.Photon.Chat.AuthenticationValues auth = new ExitGames.Client.Photon.Chat.AuthenticationValues();
		auth.UserId = username;
		chatClient.Connect( appID, "1.0", auth);
	}

	public void SendMessage(string channel, string msg)
	{
		chatClient.PublishMessage(channel, msg);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(chatClient != null)
			chatClient.Service();
	}

	public void SendSystemMessage(string msg)
	{
		LobbyChatDisplay.AddMessage("System", msg, PlrMessageType.System);
	}

	public static string playerToText(string user)
	{
		return user.Split(':')[0];
	}

	public void Whisper(string user, string message)
	{
		UnityEngine.Debug.Log("Sending Whisper to " + user + ": " + message);
		chatClient.SendPrivateMessage(user, message);
	}

	public void OnGetMessages( string channelName, string[] senders, object[] messages )
	{
		string msgs = "";
		for ( int i = 0; i < senders.Length; i++ )
		{
			msgs += senders[i] + "=" + messages[i] + ", ";
			if(channelName.Equals("lobby") && !checkBlocked(senders[i]))
			{
				LobbyChatDisplay.AddMessage(senders[i], (string)messages[i], PlrMessageType.Default);
			}
		}
		UnityEngine.Debug.Log( "OnGetMessages: " + channelName + "(" + senders.Length + ") > " + msgs );

	}

	public void OnPrivateMessage( string sender, object message, string channelName )
	{
		if(sender != PhotonNetwork.player.name)
		{
			string msg = ((string)message);
			if(checkBlocked(sender))
			{
				Whisper(sender, "PLAYER_BLOCKED");
			}
			else if(msg.Equals("/invite"))
			{
				FindObjectOfType<PartyInvitation>().Open(sender);
			}
			else if(msg.Equals("GAME_STARTED"))
			{
				PhotonNetwork.JoinRoom(sender);
			}
			else if(msg.Equals("ACCEPT_INVITE"))
			{
				FindObjectOfType<PartyMatchmaker>().SetLeader(PhotonNetwork.playerName);
				FindObjectOfType<PartyMatchmaker>().AddPlayerToParty(sender);
				SendSystemMessage(playerToText(sender) + " joined your party.");
			}
			else if(msg.Equals("PLAYER_BLOCKED"))
			{
				SendSystemMessage(playerToText(sender) + " has blocked communication with you.");
			}
			else if(msg.Equals("REMOVED_FROM_PARTY"))
			{
				FindObjectOfType<PartyMatchmaker>().ClearParty();
				SendSystemMessage("You have been removed from the party.");
			}
			else if(msg.Equals("DECLINE_INVITE"))
			{
				SendSystemMessage(sender + " declined your invitation.");
			}
			else
			{
				UnityEngine.Debug.Log("Got Whisper from " + playerToText(sender) + ": " + (string)message);
				LobbyChatDisplay.AddMessage(sender, (string)message, PlrMessageType.Whisper);
			}

		}
	}

	public void OnDisconnected()
	{
		UnityEngine.Debug.Log("Disconnected from Chat");
	}

	public void OnConnected()
	{
		UnityEngine.Debug.Log("Chat Connected");
		chatClient.Subscribe(new string[] {"lobby"});
	}

	public void DebugReturn(DebugLevel level, string message)
	{
		UnityEngine.Debug.Log(message);
	}

	public void OnChatStateChange(ChatState state)
	{
		UnityEngine.Debug.Log(state.ToString());
	}
	
	public void OnSubscribed(string[] channels, bool[] results)
	{}

	public void OnUnsubscribed(string[] channels)
	{}

	public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
	{}

	public void OnApplicationQuit()
	{
		if (chatClient != null)
		{
			chatClient.Disconnect();
		}
	}
}
