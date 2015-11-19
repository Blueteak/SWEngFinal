using UnityEngine;
using System.Collections;

public class GridMouse : MonoBehaviour {

	public bool inMenu = true;
	RocketControl rocket;
	VectorGrid grid;

	void Start()
	{
		grid = FindObjectOfType<VectorGrid>();
	}

	// Update is called once per frame
	void Update () 
	{
		CheckInMenu();
		//if(Input.GetMouseButton(0) && inMenu)
		if(inMenu)
		{
			Vector3 mPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			mPos.z = 0.25f;
			grid.AddGridForce(mPos, .5f, .5f, Color.white, false);
		}
	}

	void CheckInMenu()
	{
		if(rocket == null)
			rocket = FindObjectOfType<RocketControl>();

		if(rocket == null)
			inMenu = true;
		else
			inMenu = false;
	}
}
