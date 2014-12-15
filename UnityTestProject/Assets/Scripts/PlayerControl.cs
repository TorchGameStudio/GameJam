using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour,ISpawnable
{
	[HideInInspector]
	public bool facingRight = true;			// For determining which way the player is currently facing.
	[HideInInspector]
	public bool jump = false;				// Condition for whether the player should jump.


	public float moveForce = 1200f;			// Amount of force added to move the player left and right.
	public float maxSpeed = 30f;				// The fastest the player can travel in the x axis.
	public AudioClip[] jumpClips;			// Array of clips for when the player jumps.
	public AudioClip[] attackClips;			// Array of clips for when the player attacks.
	public float jumpForce = 1200f;			// Amount of force added when the player jumps.
	public AudioClip[] taunts;				// Array of clips for when the player taunts.
	public float tauntProbability = 50f;	// Chance of a taunt happening.
	public float tauntDelay = 1f;			// Delay for when the taunt should happen.


	private int tauntIndex;					// The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;			// A position marking where to check if the player is grounded.
	private bool grounded = false;			// Whether or not the player is grounded.
	//private Animator anim;					// Reference to the player's animator component.

	bool isStunned = false; 				//If the player is currently stunned
	int attackRangeX = 1;					
	int attackRangeY = 2;
	bool attack = false;			//Condition for if the player should attack
	public bool isControllable = true;


	void Awake()
	{
		// Setting up references.
		//anim = GetComponent<Animator>();
	}


	void Update()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		// grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));  
		Vector2 rayOffset = new Vector3(transform.position.x,transform.position.y - 2f,0f);
		Debug.DrawRay(rayOffset,-Vector2.up); //Ray for testing (to pick an offset where the ray doesnt hit the player)
		grounded = Physics2D.Raycast(rayOffset,-Vector2.up,0.5f,1 << 0);
		// If the jump button is pressed and the player is grounded then the player should jump.
		if(Input.GetButtonDown("Jump") && grounded && !isStunned)
			jump = true;
		//If the Fire button is pressed and the player is not stunned the player should attack
		if(Input.GetButtonDown("Fire1") && !isStunned)
		{
			attack = true;
			Debug.Log("Fire");
		}
	}


	void FixedUpdate ()
	{
		if(!isStunned && isControllable)
		{
			// Cache the horizontal input.
			float h = Input.GetAxis("Horizontal");

			// The Speed animator parameter is set to the absolute value of the horizontal input.
			//anim.SetFloat("Speed", Mathf.Abs(h));

			// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
			if(h * rigidbody2D.velocity.x < maxSpeed)
				// ... add a force to the player.
				rigidbody2D.AddForce(Vector2.right * h * moveForce);

			// If the player's horizontal velocity is greater than the maxSpeed...
			if(Mathf.Abs(rigidbody2D.velocity.x) > maxSpeed)
				// ... set the player's velocity to the maxSpeed in the x axis.
				rigidbody2D.velocity = new Vector2(Mathf.Sign(rigidbody2D.velocity.x) * maxSpeed, rigidbody2D.velocity.y);

			// If the input is moving the player right and the player is facing left...
			if(h > 0 && !facingRight)
				// ... flip the player.
				Flip();
			// Otherwise if the input is moving the player left and the player is facing right...
			else if(h < 0 && facingRight)
				// ... flip the player.
				Flip();

			// If the player should jump...
			if(jump)
			{
				// Set the Jump animator trigger parameter.
				//anim.SetTrigger("Jump");

				// Play a random jump audio clip.
				int i = Random.Range(0, jumpClips.Length);
				//AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);

				// Add a vertical force to the player.
				rigidbody2D.AddForce(new Vector2(0f, jumpForce));

				// Make sure the player can't jump again until the jump conditions from Update are satisfied.
				jump = false;
			}

			// If the player should attack...
			if(attack)
			{
				// Set the Jump animator trigger parameter.
				//anim.SetTrigger("Jump");
				
				// Play a random jump audio clip.
				int i = Random.Range(0, attackClips.Length);
				//AudioSource.PlayClipAtPoint(jumpClips[i], transform.position);
				
				Attack();
				
				// Make sure the player can't jump again until the jump conditions from Update are satisfied.
				attack = false;
			}
		}
	}
	
	
	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;

		// Multiply the player's x local scale by -1.

		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;

	}


	public IEnumerator Taunt()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range(0f, 100f);
		if(tauntChance > tauntProbability)
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds(tauntDelay);

			// If there is no clip currently playing.
			if(!audio.isPlaying)
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom();

				// Play the new taunt.
				audio.clip = taunts[tauntIndex];
				audio.Play();
			}
		}
	}



	int TauntRandom()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range(0, taunts.Length);

		// If it's the same as the previous taunt...
		if(i == tauntIndex)
			// ... try another random taunt.
			return TauntRandom();
		else
			// Otherwise return this index.
			return i;
	}
	public void ReSpawn()
	{
		rigidbody2D.velocity = new Vector2(0.0f,0.0f);
		transform.position = Vector3.zero;
	}

	/// <summary>
	/// Spawns a box in front of the player and activates the stun function of any players in the area.
	/// </summary>
	void Attack()
	{

		Vector3 attackOrigin; //where the attack originates from (offset from player)
		RaycastHit2D[] hit; //contains info on all objects that are hit by a raycast
		int dir; //direction facing sign for multiplying
		if(facingRight)
		{
			attackOrigin = new Vector3( transform.position.x+1f,transform.position.y-0.5f,transform.position.z);
             dir = 1;	
		}
		else
		{
			dir = -1;
			attackOrigin = new Vector3( transform.position.x-1f,transform.position.y-0.5f,transform.position.z);

		}

		//cast a box collider in the attack area to see if there are things that will be attacked
		hit = Physics2D.BoxCastAll(attackOrigin,new Vector2(attackRangeX,attackRangeY),0,dir * Vector2.right);
		Debug.DrawLine(attackOrigin,new Vector2(attackOrigin.x + attackRangeX,attackOrigin.y + attackRangeY));

		//check all hit objects to see if they are a player and stun them
		for(int i =0; i < hit.Length;i++)
		{
			Debug.Log(hit[i].collider.gameObject.name);
			PlayerControl other = hit[i].collider.gameObject.GetComponent<PlayerControl>() as PlayerControl;
			//Debug.Log(other + " OTHER");
			if(other != null)
			{
				//other.Stun();
			}
			if(hit[i].rigidbody != null && hit[i].transform.gameObject.GetComponent<BallNetwork>()) //regardless of its a player or not, if it has a rigidbody, push them
			{
				hit[i].rigidbody.AddForce(new Vector2(dir * moveForce * 2, jumpForce * 0.5f));
			}
		}

		

	}

	public void Stun()
	{
		Debug.Log("STUNNED " + transform.gameObject.name);
		int dir;
		if(facingRight)
		{
			dir = 1;	
		}
		else
		{
			dir = -1;
			
		}
		if (rigidbody != null) 
		{
			rigidbody.AddForce (new Vector2 (dir * moveForce * 2, jumpForce * 0.5f));
		}
		StartCoroutine(Stunned(2.0f));
	}
	/// <summary>
	/// Sets stunned variable to true for the specified time
	/// </summary>
	/// <param name="stunTime">Stun time in seconds.</param>
	IEnumerator Stunned(float stunTime)
	{
		int dir;
		if(facingRight)
		{
			dir = 1;	
		}
		else
		{
			dir = -1;
			
		}
		//Tyler Question: Won't this send the stunned player flying in the direction that he is facing and not that the attacking player is facing?
		rigidbody2D.AddForce(new Vector2(dir * moveForce * 2, jumpForce * 0.5f));

		isStunned = true;
		while(isStunned)//loop incase we want to do stuff later
		{
			yield return new WaitForSeconds(stunTime);
			isStunned = false;
		}
		Debug.Log("UNSTUNNED " + transform.gameObject.name);
		
	}

}
