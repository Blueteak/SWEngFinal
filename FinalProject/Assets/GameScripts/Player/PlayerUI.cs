using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {

	public Text nameText;
	string name;

	IEnumerator Start()
	{
		yield return new WaitForEndOfFrame();
		if(transform.parent.tag != "myShip")
		{
			name = transform.parent.GetComponent<PhotonView>().owner.name;
			nameText.text = name;
		}
	}
	
	void Update () 
	{
		transform.eulerAngles = Vector3.zero;
	}


}
