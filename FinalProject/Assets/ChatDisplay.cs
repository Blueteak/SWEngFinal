using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ChatDisplay : MonoBehaviour {

	public Image[] Displays;
	public Color InFocus;
	public Color OutFocus;
	public List<Message> ChatMsgs;
	public GameObject MsgPrefab;
	public List<GameObject> MessageObjects;

	public string Username;

	public Transform MsgHolder;

	public InputField ifield;

	public GameObject Context;

	public PlrMessageType CurrentType;
	public string WhisperTarget;

	public ChatSystem cs;

	public Text ChatInput;

	bool inFocus;

	EventSystem esyst;

	public ScrollRect scroll;



	public void doMessage()
	{
		if(Input.GetButtonDown("Submit") && ifield.text.Length > 0){
			if(CurrentType == PlrMessageType.Default)
				cs.SendMessage("lobby", ifield.text);
			else if(CurrentType == PlrMessageType.Whisper)
			{
				cs.Whisper(WhisperTarget, ifield.text);
				AddMessage(WhisperTarget, ifield.text, PlrMessageType.WhisperConfirm);
			}
			ifield.text = "";
			FindObjectOfType<EventSystem>().SetSelectedGameObject(null);
		}
	}

	void Start()
	{
		esyst = FindObjectOfType<EventSystem>();
		ChatMsgs = new List<Message>();
		cs = FindObjectOfType<ChatSystem>();
		MessageObjects = new List<GameObject>();
		ChangeType(PlrMessageType.Default);
	}

	void Update()
	{
		if(inFocus)
		{
			if(Input.GetKeyDown(KeyCode.Return))
				esyst.SetSelectedGameObject(ifield.gameObject);
			else if(Input.GetKeyDown(KeyCode.Escape) && inFocus)
			{
				Debug.Log("Chat Left Focus");
				ChangeType(PlrMessageType.Default);
				inFocus = false;
				esyst.SetSelectedGameObject(null);
				foreach(var b in Displays)
					b.color = OutFocus;
			}
		}
		else
		{

			if(Input.GetKeyDown(KeyCode.Return) || esyst.currentSelectedGameObject == ifield.gameObject && !inFocus)
			{
				Debug.Log("Chat Entered Focus");
				inFocus = true;
				esyst.SetSelectedGameObject(ifield.gameObject);
				foreach(var b in Displays)
					b.color = InFocus;
			}

		}
	}

	public void ChangeType(PlrMessageType type)
	{
		CurrentType = type;
		if(CurrentType == PlrMessageType.Whisper)
		{
			ChatInput.color = Color.blue;
		}
		else
		{
			ChatInput.color = Color.black;
		}
	}

	public void InvitePlayer(string user)
	{
		WhisperTarget = user;
		cs.Whisper(WhisperTarget, "/invite");
	}

	public void BlockPlayer(string user)
	{
		cs.ToggleBlock(user);
	}

	public void SetWhisperTarget(string user)
	{
		WhisperTarget = user;
		esyst.SetSelectedGameObject(ifield.gameObject);
		ifield.text = "Whisper [" + ChatSystem.playerToText(user) + "]";

	}

	public void AddMessage(string user, string body, PlrMessageType type)
	{
		Message m = new Message();
		m.user = user;
		m.body = body;
		m.mType = type;
		ChatMsgs.Add(m);
		Redraw();
	}

	public void OpenContext(RectTransform t, string user)
	{
		if(user != PhotonNetwork.player.name)
		{
			Context.SetActive(true);
			Context.GetComponent<RectTransform>().position = t.position;
			Context.GetComponent<ChatContext>().Init(user);
		}

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
			newM.GetComponent<ChatMessageObj>().Init(m);
			MessageObjects.Add(newM);
		}
		Invoke("ScrollToBottom", 0.1f);
	}

	void ScrollToBottom()
	{
		scroll.normalizedPosition = new Vector2(0, 0);
	}
}



public class Message
{
	public string user;
	public string body;
	public PlrMessageType mType;
}

public enum PlrMessageType
{
	Default,
	Whisper,
	WhisperConfirm,
	System
}
