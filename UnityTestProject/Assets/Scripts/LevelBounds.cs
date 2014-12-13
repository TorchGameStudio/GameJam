using UnityEngine;
using System.Collections;

public class LevelBounds : MonoBehaviour {


	// Use this for initialization
	void Start () {
	
	}
	
	void OnCollisionEnter2D(Collider2D collider)
	{
		ISpawnable spawnalbe = (ISpawnable)collider.gameObject.GetComponent(typeof(ISpawnable));
		if(spawnalbe != null)
		{
			spawnalbe.ReSpawn();
		}

	}
}
