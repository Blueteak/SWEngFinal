using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {

	public RenderTexture rtext;
	public Image img;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnPostRender()
	{
		Debug.Log("PostRender");
		/*
		Texture2D t = new Texture2D(rtext.width, rtext.height);
		t.ReadPixels(new Rect(0, 0, rtext.width, rtext.height), 0, 0);
		t.Apply();
		*/
//		RawImage r = RawImage.;
		//r.texture = rtext;
		//img.sprite = Sprite.Create(t, new Rect(0,0,t.width,t.height), new Vector2(0.5f, 0.5f));
		//img.sprite = r;
	}
}
