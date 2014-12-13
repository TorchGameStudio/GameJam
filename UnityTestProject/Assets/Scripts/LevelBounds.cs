using UnityEngine;
using System.Collections;
/// <summary>
/// Either Respawns or destroys things that leave its bounds
/// </summary>
public class LevelBounds : MonoBehaviour {
	
	void OnTriggerExit2D(Collider2D collider)
	{
		ISpawnable spawnalbe = (ISpawnable)collider.gameObject.GetComponent(typeof(ISpawnable));
		if(spawnalbe != null)
		{
			spawnalbe.ReSpawn();
			Debug.Log("Respawned");
		}
		else
		{
			Debug.Log("BOOM");
			Destroy(collider.gameObject);
		}

	}
}
