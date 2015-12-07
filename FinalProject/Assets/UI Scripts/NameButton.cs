using UnityEngine;
using System.Collections;

public class NameButton : MonoBehaviour {

	public RectTransform panelPosition;

	// Use this for initialization
	public void click() 
	{
		GetComponentInParent<ChatMessageObj>().OpenContext(panelPosition);
	}
}
