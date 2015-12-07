using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ChatContext : MonoBehaviour {

	public ChatDisplay disp;
	string target; 
	public Text BlockText;


	// Use this for initialization
	public void Init (string user)
	{
		target = user;
		if(FindObjectOfType<ChatSystem>().checkBlocked(target))
			BlockText.text = "Unblock";
	}

	public void Invite()
	{
		disp.InvitePlayer(target);
	}

	void Update()
	{
		if(Input.GetMouseButtonUp(0) || Input.GetKeyDown(KeyCode.Escape))
			gameObject.SetActive(false);
	}
	

	public void ToggleBlock()
	{
		disp.BlockPlayer(target);

		if(FindObjectOfType<ChatSystem>().checkBlocked(target))
			BlockText.text = "Unblock";
		else
			BlockText.text = "Block";
	}

	public void whisper()
	{
		disp.SetWhisperTarget(target);
		disp.ChangeType(PlrMessageType.Whisper);
	}

}
