﻿using UnityEngine;

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

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
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
			CurrentArena = (GameObject)Instantiate(Arenas[0]);
	}
}