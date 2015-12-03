using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ChatMessageObj : MonoBehaviour {

	public Text NameText;
	public Text BodyText;
	// Use this for initialization
	public void Init (string username, string body)
	{
		string nt = "["+username.Split(':')[0]+"]: ";
		NameText.text = nt;
		BodyText.text = nt+body;
	}

}
