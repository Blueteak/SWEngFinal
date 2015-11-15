using UnityEngine;
using System.Collections;

/**
 * Objects with this class will follow a desired target
 **/

public class ObjFollow : MonoBehaviour {

	public Transform target;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = new Vector3(target.position.x, target.position.y, transform.position.z);
	}
}
