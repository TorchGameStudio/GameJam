using UnityEngine;
using System.Collections;

public class BallNetwork : Photon.MonoBehaviour,ISpawnable {

	private Vector3 correctBallPos = Vector3.zero; //We lerp towards this
	private float correctBallAngVel = 0.0f;
	private Vector2 correctBallVel = Vector2.zero;
	
	void Awake()
	{
		if (!photonView.isMine)
		{
			//rigidbody2D.isKinematic = true;
			//rigidbody2D.gravityScale = 0;
		}
		
		gameObject.name = gameObject.name + photonView.viewID;
	}
	
	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting)
		{
			stream.SendNext(transform.position);
			stream.SendNext(rigidbody2D.angularVelocity);
			stream.SendNext(rigidbody2D.velocity);
		}
		else
		{
			correctBallPos = (Vector3)stream.ReceiveNext();
			correctBallAngVel = (float)stream.ReceiveNext();
			correctBallVel = (Vector2)stream.ReceiveNext();
		}
	}
	
	void Update()
	{
		if (!photonView.isMine)
		{
			transform.position = Vector3.Lerp(transform.position, correctBallPos, Time.deltaTime * 10);
			//rigidbody2D.angularVelocity = correctBallAngVel;
			//rigidbody2D.velocity = correctBallVel;
		}
	}
	//Just quickly adding so ball can respawn when it falls off the edge, move to other script if needed
	public void ReSpawn()
	{
		transform.position = Vector3.zero;
		rigidbody2D.velocity = Vector2.zero;
	}
}
