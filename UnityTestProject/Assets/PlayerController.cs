using UnityEngine;
using System.Collections;
/// <summary>
/// Basic Player controller
/// -redo later (rushed)
/// </summary>
public class PlayerController : MonoBehaviour {
	#region fields
	Rigidbody2D rigidBody;
	readonly Vector2 PUSHFORCE = new Vector2(200.0f,0.0f);
	readonly Vector2 JUMPFORCE = new Vector2(0.0f, 500.0f);
	const float MAXSPEED = 60.0f;
	#endregion
	#region Monobehaviour Functions
	// Use this for initialization
	void Awake () {

		if(rigidbody2D == null)
		{
			gameObject.AddComponent<Rigidbody2D>();
		}
		rigidBody = this.GetComponent<Rigidbody2D>() as Rigidbody2D;
	}
		

	
	// Update is called once per frame
	void Update () {
		if(isGrounded())
		{
			if(Mathf.Abs(rigidBody.velocity.x) < MAXSPEED)//Can't turn when going to fast or speed up
			{
				if(Input.GetKeyDown(KeyCode.RightArrow))
				{
					rigidBody.AddForce(PUSHFORCE);
				}
				else if(Input.GetKeyDown(KeyCode.LeftArrow))
				{
					rigidBody.AddForce(-PUSHFORCE);
				}
			}
			if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
			{
				rigidBody.AddForce(JUMPFORCE);
			}
			if( Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)  || Input.GetKeyDown(KeyCode.DownArrow))
			{
				rigidBody.AddForce(-JUMPFORCE/4.0f);
			}
		}
		else
		{
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				rigidBody.AddForce(PUSHFORCE/4.0f);
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				rigidBody.AddForce(-PUSHFORCE/4.0f);
			}
		}


	
	}

	#endregion
	#region Private Functions
	bool isGrounded()
	{
		//use renderer to find the base of the gameobject 
		Vector2 origin = new Vector3(transform.position.x,transform.position.y - (renderer.bounds.max.y/1.5f));
		//cast ray down for a short distance
		RaycastHit2D hit = Physics2D.Raycast(origin,-Vector2.up,0.1f);
		//Debug.DrawRay(origin,-Vector2.up,Color.red,1);
		if(hit.collider != null)
		{
			//Debug.Log("GROUNDED " + hit.collider.gameObject.name);
			return true;

		}
		else
		{
			//Debug.Log("AIR TIME");
			return false;
		}
	}
	#endregion
}
