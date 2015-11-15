using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class Note : MonoBehaviour {

	EMOpenCloseMotion motion;
	public Text TitleText;
	public Text MessageText;

	// Use this for initialization
	void Start () 
	{
		motion = GetComponent<EMOpenCloseMotion>();
	}
	
	public void Notify(string title, string message, float displayTime)
	{
		TitleText.text = title;
		MessageText.text = message;
		DoNoteify(displayTime);
	}

	public void Warin(string message)
	{
		Warn(message, 3f);
	}

	public void Warn(string message, float displayTime)
	{
		TitleText.text = "Warning!";
		MessageText.text = message;
		DoNoteify(displayTime);
	}

	public void Error(string message)
	{
		Error(message, 3f);
	}

	public void Error(string message, float displayTime)
	{
		TitleText.text = "Error";
		MessageText.text = message;
		DoNoteify(displayTime);
	}

	void DoNoteify(float time)
	{
		StopAllCoroutines();
		StartCoroutine("ShowAndHide", time);
	}

	IEnumerator ShowAndHide(float seconds)
	{
		motion.SetStateToClose();
		motion.Open();
		yield return new WaitForSeconds(seconds);
		motion.SetStateToClose();
		motion.Close();
	}
}
