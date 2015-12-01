using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.Chat;
public class LobbyChat : MonoBehaviour {

	public static string appID = "8b07b1f6-0303-490c-a8e2-b6157b7bfa81";
	public ChatClient chat;
	// Use this for initialization
	void LoginChat (string username) 
	{
		chat = new ChatClient(this, ExitGames.Client.Photon.ConnectionProtocol.Udp);
		chat.Connect( appID, "1.0", username, null);
		chat.Subscribe(new string[] {"lobby"});
	}
	
	// Update is called once per frame
	void Update () 
	{
		chat.Service();
	}

	public void OnGetMessages( string channelName, string[] senders, object[] messages )
	{
		string msgs = "";
		for ( int i = 0; i < senders.Length; i++ )
		{
			msgs += senders[i] + "=" + messages[i] + ", ";
		}
		Debug.Log( "OnGetMessages: " + channelName + "(" + senders.Length + ") > " + msgs );
	}

	public void OnPrivateMessage( string sender, object message, string channelName )
	{
		ChatChannel ch = chat.PrivateChannels[ channelName ];
		foreach ( object msg in ch.Messages )
		{
			Debug.Log( msg );
		}
	}
}
