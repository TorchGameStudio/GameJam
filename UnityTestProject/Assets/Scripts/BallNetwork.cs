using UnityEngine;
using System.Collections;

public class BallNetwork : Photon.MonoBehaviour {

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
}
