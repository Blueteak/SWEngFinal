using UnityEngine;
using System.Collections;

public class CamMiniMap : MonoBehaviour {

	MiniMap map;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		while(map == null)
		{
			map = FindObjectOfType<MiniMap>();
		}
	}
}
