using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class goalPost : MonoBehaviour {
	#region fields
	public Text scoreSpot; //The Text GameObject
	PolygonCollider2D groundCollider;
	uint score = 0;
	const uint goalPoints = 1;
	BoxCollider2D myCollider;
	Vector3 groundCenter;
	#endregion
	#region Monobehaviours
	// Use this for initialization
	void Start () 
	{


	   gameObject.AddComponent<BoxCollider2D>();
		myCollider = gameObject.GetComponent<BoxCollider2D>() as BoxCollider2D;
		//set up the collider so that it is in the back of the net, and a third of the nets width
		//myCollider = gameObject.GetComponent<BoxCollider2D>() as BoxCollider2D;
//		myCollider.center.Set(gameObject.transform.position.x/3.0f,gameObject.transform.position.y);
//		myCollider.size.Set(0.6f,renderer.bounds.size.y); //bounds.SetMinMax(new Vector3(renderer.bounds.min.x,renderer.bounds.min.y,renderer.bounds.min.z),new Vector3(renderer.bounds.max.x/3,renderer.bounds.max.y,renderer.bounds.max.z));
//
//
//		collider2D.bounds.center.Set(renderer.bounds.center.x,renderer.bounds.center.y,0);
//		collider2D.bounds.size.Set(renderer.bounds.size.x/3,renderer.bounds.size.y,0);
//		collider2D.bounds.SetMinMax(renderer.bounds.min,renderer.bounds.max);

		groundCollider = GameObject.FindGameObjectWithTag("Ground").GetComponent<PolygonCollider2D>() as PolygonCollider2D; 

		groundCenter = groundCollider.transform.position;

	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if(col.gameObject.tag.Equals("Ball"))
		{
			score += goalPoints;
			scoreSpot.text = score.ToString();
			col.transform.position = new Vector3(groundCenter.x,groundCenter.y + 5.0f, groundCenter.z);



		}

	}
	#endregion
}
