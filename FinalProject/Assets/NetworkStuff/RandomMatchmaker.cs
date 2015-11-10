using UnityEngine;

public class RandomMatchmaker : Photon.PunBehaviour
{
    public GameObject playerPrefab;
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
        PhotonNetwork.JoinRandomRoom();
    }
    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }
    public override void OnJoinedRoom()
    {
        //base.OnJoinedRoom();
        Debug.Log("Joined Room");
        GameObject player = PhotonNetwork.Instantiate("Ship", Vector3.zero, Quaternion.identity, 0);
        player.GetComponent<RocketControl>().canMove = true;
        player.GetComponent<RocketShoot>().canShoot = true;
        GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ObjFollow>().target = player.transform;
    }
}