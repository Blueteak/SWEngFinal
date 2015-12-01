using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.Chat;
public class LobbyChat : MonoBehaviour {

	public static string appID = "8b07b1f6-0303-490c-a8e2-b6157b7bfa81";
	public IChatClientListener lsr;
	public ChatClient chatClient;
	// Use this for initialization
	void LoginChat (string username) 
	{
		chatClient = new ChatClient( lsr , ExitGames.Client.Photon.ConnectionProtocol.Udp);
		chatClient.Connect( appID, "1.0", null );
		chatClient.Subscribe(new string[] {"lobby"});
	}
	
	// Update is called once per frame
	void Update () 
	{
		chatClient.Service();
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
		ChatChannel ch = chatClient.PrivateChannels[ channelName ];
		foreach ( object msg in ch.Messages )
		{
			Debug.Log( msg );
		}
	}
}
