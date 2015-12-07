using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PartyInvitation : MonoBehaviour {

	EMOpenCloseMotion panel;

	string AskedUser;

	bool currentAsk;

	public Text DispText;

	string[] potentialMembers;

	void Start()
	{
		panel = GetComponent<EMOpenCloseMotion>();
	}

	public void Open(string user)
	{
		if(!currentAsk && !FindObjectOfType<PartyMatchmaker>().inParty())
		{
			AskedUser = user;
			DispText.text = ChatSystem.playerToText(AskedUser) + " invited you to a party.";
			currentAsk = true;
			panel.SetStateToClose();
			panel.Open();
		}
	}

	public void Accept () 
	{
		FindObjectOfType<ChatSystem>().Whisper(AskedUser, "ACCEPT_INVITE");
		FindObjectOfType<PartyMatchmaker>().SetLeader(AskedUser);
		panel.SetStateToOpen();
		panel.Close();
		currentAsk = false;
	}

	public void Decline()
	{
		FindObjectOfType<ChatSystem>().Whisper(AskedUser, "DECLINE_INVITE");
		panel.SetStateToOpen();
		panel.Close();
		currentAsk = false;
	}
}
