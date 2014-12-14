﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerNetwork : Photon.MonoBehaviour {
	
	PlayerControl controllerScript;
	private Vector3 correctPlayerPos = Vector3.zero; //We lerp towards this
	private Quaternion correctPlayerRot = Quaternion.identity; //We lerp towards this
	
	void Awake()
	{
		controllerScript = GetComponent<PlayerControl>();
		
		if (photonView.isMine)
		{
			//MINE: local player, simply enable the local scripts
			controllerScript.enabled = true;
		}
		else
		{           
			controllerScript.enabled = false;
			rigidbody2D.isKinematic = true;
		}
		
		gameObject.name = gameObject.name + photonView.viewID;
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			//We own this player: send the others our data
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation); 
		}
		else
		{
			//Network player, receive data
			correctPlayerPos = (Vector3)stream.ReceiveNext();
			correctPlayerRot = (Quaternion)stream.ReceiveNext();
		}
	}
	
	void Update()
	{
		if (!photonView.isMine)
		{
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			transform.position = Vector3.Lerp(transform.position, correctPlayerPos, Time.deltaTime * 5);
			transform.rotation = Quaternion.Lerp(transform.rotation, correctPlayerRot, Time.deltaTime * 5);
		}
	}
}
