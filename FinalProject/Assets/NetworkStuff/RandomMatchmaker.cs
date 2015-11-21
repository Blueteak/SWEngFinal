using UnityEngine;

public class RandomMatchmaker : Photon.PunBehaviour
{
    public GameObject playerPrefab;
	public GameObject[] Arenas;
	public EMOpenCloseMotion MainMenu;
	public EMOpenCloseMotion GameUI;

	public bool testArena;

	GameObject CurrentArena;

    // Use this for initialization
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

	float deltaTime = 0.0f;
	
	void Update()
	{
		deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
	}

    void OnGUI()
    {
		int w = Screen.width, h = Screen.height;
		
		GUIStyle style = new GUIStyle();
		
		Rect rect = new Rect(0, 0, w, h * 2 / 100);
		style.alignment = TextAnchor.UpperLeft;
		style.fontSize = h * 2 / 100;
		style.normal.textColor = new Color (0.0f, 0.0f, 0.5f, 1.0f);
		float msec = deltaTime * 1000.0f;
		float fps = 1.0f / deltaTime;
		string text = string.Format("{0:0.0} ms ({1:0.} fps)", msec, fps);
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString()+"\n"+text);
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
		if(testArena)
        	PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
		FindObjectOfType<Note>().Notify("No open games","Creating new room.", 1.5f);
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }
    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
		if(MainMenu != null)
			MainMenu.Close();
		GameUI.Open();
		DestroyCurrentArena();
		SpawnArena(FindObjectOfType<GameSetup>().ArenaIndex);
        Debug.Log("Joined Room");
        GameObject player = PhotonNetwork.Instantiate("Ship", Vector3.zero, Quaternion.identity, 0);
        player.GetComponent<RocketControl>().canMove = true;
        player.GetComponent<RocketShoot>().canShoot = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ObjFollow>().target = player.transform;
    }

	void DestroyCurrentArena()
	{
		if(CurrentArena != null)
			Destroy(CurrentArena);
	}

	void SpawnArena(int index)
	{
		if(index < Arenas.Length)
		{
			CurrentArena = PhotonNetwork.Instantiate(Arenas[index].name, Vector3.zero, Quaternion.identity, 0);
		}
		else
			CurrentArena = PhotonNetwork.Instantiate(Arenas[0].name, Vector3.zero, Quaternion.identity, 0);
	}
}