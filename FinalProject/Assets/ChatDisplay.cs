using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChatDisplay : MonoBehaviour {

	public List<Message> ChatMsgs;
	public GameObject MsgPrefab;
	public List<GameObject> MessageObjects;

	public string Username;

	public Transform MsgHolder;

	public InputField ifield;

	public void doMessage()
	{
		FindObjectOfType<ChatSystem>().SendMessage("lobby", ifield.text);
	}

	void Start()
	{
		ChatMsgs = new List<Message>();
		MessageObjects = new List<GameObject>();
	}

	public void AddMessage(string user, string body)
	{
		Message m = new Message();
		m.user = user;
		m.body = body;
		ChatMsgs.Add(m);
		Redraw();
	}

	void Redraw()
	{
		foreach(var v in MessageObjects)
		{
			Destroy(v);
		}
		MessageObjects = new List<GameObject>();
		foreach(var m in ChatMsgs)
		{
			GameObject newM = (GameObject)Instantiate(MsgPrefab);
			newM.transform.SetParent(MsgHolder);
			newM.transform.localScale = Vector3.one;
			newM.GetComponent<ChatMessageObj>().Init(m.user, m.body);
			MessageObjects.Add(newM);
		}
	}
}

public class Message
{
	public string user;
	public string body;
}
