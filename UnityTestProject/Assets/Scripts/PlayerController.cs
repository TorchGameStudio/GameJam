using UnityEngine;
using System.Collections;
/// <summary>
/// Basic Player controller
/// -redo later (rushed)
/// </summary>
public class PlayerController : MonoBehaviour,ISpawnable {
	#region fields
	Rigidbody2D rigidBody;
	readonly Vector2 PUSHFORCE = new Vector2(200.0f,0.0f);
	readonly Vector2 JUMPFORCE = new Vector2(0.0f, 500.0f);
	const float MAXSPEED = 60.0f;
	public Vector3 SpawnPoint = new Vector3 (0.0f,6.0f,0.0f);
	bool isStunned;
	int layerMask = 1 << 0; //gamplay layer
	//public Collider2D attackArea;
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
	void Update () 
	{

		if(isGrounded())
		{
			if(!isStunned)
			{
				//Attack movement
				if(Input.GetKeyDown(KeyCode.RightAlt) || Input.GetKeyDown(KeyCode.LeftAlt))
				{
					Attack();
				}
				if(Mathf.Abs(rigidBody.velocity.x) < MAXSPEED)//Can't turn when going to fast or speed up
				{

					if(Input.GetKeyDown(KeyCode.RightArrow)) //Move right
					{
						if(transform.localScale.x < 0 )
						{
							flipScale();
						}
						rigidBody.AddForce(PUSHFORCE);
					}
					else if(Input.GetKeyDown(KeyCode.LeftArrow)) //move left
					{
						if(transform.localScale.x > 0 )
						{
							flipScale();
						}
						rigidBody.AddForce(-PUSHFORCE);
					}
				}
				//Jumping
				if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
				{
					rigidBody.AddForce(JUMPFORCE);
				}//going down
				else if( Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)  || Input.GetKeyDown(KeyCode.DownArrow))
				{
					rigidBody.AddForce(-JUMPFORCE/4.0f);
				}

			}
		}
		else //if falling
		{
			if(Input.GetKeyDown(KeyCode.RightArrow))
			{
				rigidBody.AddForce(PUSHFORCE/4.0f);
			}
			else if(Input.GetKeyDown(KeyCode.LeftArrow))
			{
				rigidBody.AddForce(-PUSHFORCE/4.0f);
			}
			//Down Force
			if( Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)  || Input.GetKeyDown(KeyCode.DownArrow))
			{
				rigidBody.AddForce(-JUMPFORCE/4.0f);
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
		RaycastHit2D hit = Physics2D.Raycast(origin,-Vector2.up,0.1f,layerMask);
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
	/// <summary>
	/// Spawns a box in front of the player and activates the stun function of any players in the area.
	/// </summary>
	void Attack()
	{
		int direction;
		if(transform.localScale.x > 0)
		{
			direction = 1;
		}
		else
		{
			direction = -1;
		}

		Vector3 pos = new Vector3( transform.position.x+(direction *0.5f),transform.position.y-0.5f,transform.position.z);
		RaycastHit2D[] hit;
		hit = Physics2D.BoxCastAll(new Vector2(pos.x,pos.y),new Vector2(2.0f,4.0f),0.0f,direction * Vector2.right);
		for(int i =0; i < hit.Length;i++)
		{
			PlayerController other = hit[i].collider.gameObject.GetComponent<PlayerController>() as PlayerController;
			if(other != null)
			{
				other.Stun();
			}
			if(hit[i].rigidbody != null)
			{
			    hit[i].rigidbody.AddForce(new Vector2(direction * 200.0f, 100.0f));
			}
		}
		//send objects/players upwards

		//Debug.DrawLine(pos, new Vector2( pos.x + (direction*2.0f),pos.y + 4.0f),Color.red,1.0f);

	}
	void flipScale()
	{
		transform.localScale *= -1;
	}
	/// <summary>
	/// Stunned for the specified stunTime.
	/// </summary>
	/// <param name="stunTime">Stun time.</param>
	IEnumerator Stunned(float stunTime)
	{
		isStunned = true;
		while(isStunned)//loop incase we want to do stuff later
		{
			yield return new WaitForSeconds(stunTime);
			isStunned = false;
		}

	}
	#endregion
	#region Public Functions
	public void Stun()
	{
		StartCoroutine(Stunned(2.0f));
	}
	public void ReSpawn()
	{
		rigidbody2D.velocity = new Vector2(0.0f,0.0f);
		transform.position = SpawnPoint;
	}
	#endregion
}
