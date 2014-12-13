using UnityEngine;
using System.Collections;
/// <summary>
/// Basic Player controller
/// -redo later (rushed)
/// </summary>
public class PlayerController : MonoBehaviour {
	#region fields
	Rigidbody2D rigidBody;
	readonly Vector2 PUSHFORCE = new Vector2(25.0f,0.0f);
	readonly Vector2 JUMPFORCE = new Vector2(0.0f, 50.0f);
	readonly float MAXSPEED = 20.0f;
	bool _isGrounded;
	#endregion
	#region Monobehaviour Functions
	// Use this for initialization
	void Start () {

		if(rigidbody2D == null)
		{
			gameObject.AddComponent<Rigidbody2D>();
		}
		rigidBody = this.GetComponent<Rigidbody2D>() as Rigidbody2D;


	}
		void OnCollisionEnter2D(Collision2D collider)
	{
		if(collider.gameObject.tag.Equals("Ground"))
		{
			_isGrounded = true;
		}  

	}
	void OnCollisionExit2D(Collision2D collider)
	{
		if(collider.gameObject.tag.Equals("Ground"))
		{
			_isGrounded = false;
		}
	}

	
	// Update is called once per frame
	void Update () {
		if(_isGrounded)
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


	
	}

	#endregion
}
