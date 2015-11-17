using UnityEngine;
using System.Collections;

public class Setup : MonoBehaviour {
    public Texture2D cursor;
	// Use this for initialization
	void Start () {
        Cursor.SetCursor(cursor, new Vector2(cursor.width/2, cursor.height/2) , CursorMode.ForceSoftware);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
