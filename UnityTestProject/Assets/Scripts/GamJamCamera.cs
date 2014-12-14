﻿using UnityEngine;
using System.Collections;

public class GamJamCamera : MonoBehaviour {
	#region Fields
	float _targetx;
	public GameObject playerGO;
	 float _targetY = 12.0f;
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
		_targetY = playerGO.transform.position.y;
		this.transform.position = new Vector3(_targetx,_targetY,TARGETZ);
	}
	#endregion
}
