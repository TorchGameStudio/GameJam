using UnityEngine;
using System.Collections;

public class BallNetwork : Photon.MonoBehaviour,ISpawnable {

	private Vector3 correctBallPos = Vector3.zero; //We lerp towards this
	private Vector2 correctBallVel = Vector2.zero;
	
	void Awake()
	{
		if (!photonView.isMine)
		{
			//rigidbody2D.isKinematic = true;
			rigidbody2D.gravityScale = 0;
		}
		
		gameObject.name = gameObject.name + photonView.viewID;
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
		}
		else
		{
			correctBallPos = (Vector3)stream.ReceiveNext();
		}
	}
	
	void Update()
	{
		if (!photonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, correctBallPos, Time.deltaTime * 10);
		}
	}
	//Just quickly adding so ball can respawn when it falls off the edge, move to other script if needed
	public void ReSpawn()
	{
		transform.position = Vector3.zero;
	}
}
