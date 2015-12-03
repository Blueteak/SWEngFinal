using UnityEngine;
using System.Collections;
using ExitGames.Client.Photon.Chat;
using UnityEditor;
using System.Diagnostics;
using ExitGames.Client.Photon;
using UnityEngine.UI;
public class ChatSystem : MonoBehaviour, IChatClientListener
{

	public static string appID = "8b07b1f6-0303-490c-a8e2-b6157b7bfa81";
	public ChatDisplay LobbyChatDisplay;
	public ChatClient chatClient;
	// Use this for initialization

	void Start()
	{
		Application.runInBackground = true;
	}

	public void LoginChat (string username) 
	{
		UnityEngine.Debug.Log("Log In");
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

	public void OnGetMessages( string channelName, string[] senders, object[] messages )
	{
		string msgs = "";
		for ( int i = 0; i < senders.Length; i++ )
		{
			msgs += senders[i] + "=" + messages[i] + ", ";
			if(channelName.Equals("lobby"))
			{
				LobbyChatDisplay.AddMessage(senders[i], (string)messages[i]);
			}
		}
		UnityEngine.Debug.Log( "OnGetMessages: " + channelName + "(" + senders.Length + ") > " + msgs );

	}

	public void OnPrivateMessage( string sender, object message, string channelName )
	{
		ChatChannel ch = chatClient.PrivateChannels[ channelName ];
		foreach ( object msg in ch.Messages )
		{
			UnityEngine.Debug.Log( msg );
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
