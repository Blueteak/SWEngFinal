﻿using UnityEngine;

public class NetworkCharacter : Photon.MonoBehaviour
{
    private Vector3 correctPlayerPos;
    private Quaternion correctPlayerRot;
	public bool givenInput = false;
	public float LR = 0;
	RocketControl rcontrol;

	void Start()
	{
		rcontrol = GetComponent<RocketControl>();
	}

    // Update is called once per frame
    void Update()
    {
        if (!photonView.isMine)
        {
            transform.position = Vector3.Lerp(transform.position, this.correctPlayerPos, Time.deltaTime * 5);
            transform.rotation = Quaternion.Lerp(transform.rotation, this.correctPlayerRot, Time.deltaTime * 5);

			//Rocket Particle System Sync

			//				Main Rocket
			if(givenInput)
				rcontrol.MainRocket.enableEmission = true;
			else
				rcontrol.MainRocket.enableEmission = false;

			//			Left/Right Thrusters
			if(LR > 0.1f)
				rcontrol.LeftRocket.enableEmission = true;
			else if(LR < -0.1f)
				rcontrol.RightRocket.enableEmission = true;
			else
			{
				rcontrol.LeftRocket.enableEmission = false;
				rcontrol.RightRocket.enableEmission = false;
			}

        }
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        
        if (stream.isWriting)
        {
            // We own this player: send the others our data
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
			stream.SendNext(givenInput);
			stream.SendNext(LR);
        }
        else
        {
            // Network player, receive data
            this.correctPlayerPos = (Vector3)stream.ReceiveNext();
            this.correctPlayerRot = (Quaternion)stream.ReceiveNext();
			this.givenInput = (bool)stream.ReceiveNext();
			this.LR = (float)stream.ReceiveNext();
        }
    }
}