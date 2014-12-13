using UnityEngine;
using System.Collections;

public class GamJamCamera : MonoBehaviour {
	#region Fields
	float _targetx;
	public GameObject playerGO;
	const float TARGETY = 12.0f;
	const float TARGETZ = -10.0f;
	#endregion
	#region monobehaviours
	// Use this for initialization
	void Start () 
	{
	 playerGO = GameObject.FindGameObjectWithTag("Player");
	_targetx = playerGO.transform.position.x;
	
	
	}
	void Update()
	{
		_targetx = playerGO.transform.position.x;
		this.transform.position = new Vector3(_targetx,TARGETY,TARGETZ);
	}
	#endregion
}
