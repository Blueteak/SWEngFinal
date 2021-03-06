﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ChatMessageObj : MonoBehaviour {

	public Text NameText;
	public Text BodyText;

	public Message m;

	public Image Holder;

	// Use this for initialization
	public void Init (Message message, Image holder)
	{
		m = message;
		Holder = holder;
		string nt = "["+ChatSystem.playerToText(m.user)+"]: ";
		if(m.mType == PlrMessageType.System)
		{
			NameText.gameObject.SetActive(false);
			BodyText.text = m.body;
			BodyText.color = Color.green;
		}
		else if(m.mType == PlrMessageType.Default || m.mType == PlrMessageType.Whisper)
		{
			NameText.text = nt;
			BodyText.text = nt+m.body;
			if(m.mType == PlrMessageType.Whisper)
				BodyText.color = Color.blue;
		}
		else if(m.mType == PlrMessageType.WhisperConfirm)
		{
			NameText.text = "To " + nt;
			BodyText.text = "To " + nt + m.body;
			BodyText.color = Color.blue;
		}

	}

	void Update()
	{
		Color c = BodyText.color;
		c.a = Holder.color.a*3;
		BodyText.color = c;
	}

	public void OpenContext(RectTransform t)
	{
		gameObject.GetComponentInParent<ChatDisplay>().OpenContext(t, m.user);
	}

}
