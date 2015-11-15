using UnityEngine;
using System.Collections;

public class Setup : MonoBehaviour {
    public Texture2D cursor;
	// Use this for initialization
	void Start () {
        Cursor.SetCursor(cursor, Vector2.one/2, CursorMode.Auto);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
