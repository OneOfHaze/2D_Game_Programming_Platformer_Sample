using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
	private bool jump = false;        // Condition for whether the player should jump.

	[SerializeField]
	private float moveForce = 365f;     // Amount of force added to move the player left and right.
	[SerializeField]
	private float maxSpeed = 5f;       // The fastest the player can travel in the x axis.
	[SerializeField]
	private AudioClip[] jumpClips;     // Array of clips for when the player jumps.
	[SerializeField]
	private float jumpForce = 1000f;     // Amount of force added when the player jumps.
	[SerializeField]
	private AudioClip[] taunts;        // Array of clips for when the player taunts.
	[SerializeField]
	private float tauntProbability = 50f;  // Chance of a taunt happening.
	[SerializeField]
	private float tauntDelay = 1f;      // Delay for when the taunt should happen.


	private int tauntIndex;         // The index of the taunts array indicating the most recent taunt.
	private Transform groundCheck;      // A position marking where to check if the player is grounded.
	private bool grounded = false;      // Whether or not the player is grounded.
	private Animator anim;          // Reference to the player's animator component.
	private Rigidbody2D rigidbody2d;
	private float _horizontal;
	private Gun _gun;
	private LayBombs _layBombs;

	public PlayerHealth Health
	{
		get; private set;
	}

	public Rigidbody2D Rigidbody
	{
		get { return rigidbody2d; }
	}

	public bool Jump
	{
		get { return jump; }
		set
		{
			if ( grounded )
			{
				jump = value;
			}
		}
	}

	public bool FacingRight
	{
		get;
		set;
	}

	public Gun Gun
	{
		get { return _gun; }
	}

	void Awake ()
	{
		// Setting up references.
		FacingRight = true;
		groundCheck = transform.Find ( "groundCheck" );
		anim = GetComponent<Animator> ();
		rigidbody2d = GetComponent<Rigidbody2D> ();
		_gun = GetComponentInChildren<Gun> ( true );
		_layBombs = GetComponent<LayBombs> ();
		Health = GetComponent<PlayerHealth> ();
	}


	void Update ()
	{
		// The player is grounded if a linecast to the groundcheck position hits anything on the ground layer.
		grounded = Physics2D.Linecast ( transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ( "Ground" ) );
	}

	public void LayBomb ()
	{
		_layBombs.LayBomb ();
	}

	public void SetHorizontal ( float input )
	{
		_horizontal = input;
	}

	void FixedUpdate ()
	{
		// The Speed animator parameter is set to the absolute value of the horizontal input.
		anim.SetFloat ( "Speed", Mathf.Abs ( _horizontal ) );

		// If the player is changing direction (h has a different sign to velocity.x) or hasn't reached maxSpeed yet...
		if ( _horizontal * rigidbody2d.velocity.x < maxSpeed )
		{
			// ... add a force to the player.
			rigidbody2d.AddForce ( Vector2.right * _horizontal * moveForce );
		}

		// If the player's horizontal velocity is greater than the maxSpeed...
		if ( Mathf.Abs ( rigidbody2d.velocity.x ) > maxSpeed )
			// ... set the player's velocity to the maxSpeed in the x axis.
			rigidbody2d.velocity = new Vector2 ( Mathf.Sign ( rigidbody2d.velocity.x ) * maxSpeed, rigidbody2d.velocity.y );

		// If the input is moving the player right and the player is facing left...
		if ( _horizontal > 0 && !FacingRight )
			// ... flip the player.
			Flip ();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if ( _horizontal < 0 && FacingRight )
			// ... flip the player.
			Flip ();

		// If the player should jump...
		if ( jump )
		{
			// Set the Jump animator trigger parameter.
			anim.SetTrigger ( "Jump" );

			// Play a random jump audio clip.
			int i = Random.Range ( 0, jumpClips.Length );
			AudioSource.PlayClipAtPoint ( jumpClips[i], transform.position );

			// Add a vertical force to the player.
			rigidbody2d.AddForce ( new Vector2 ( 0f, jumpForce ) );

			// Make sure the player can't jump again until the jump conditions from Update are satisfied.
			jump = false;
		}
	}


	void Flip ()
	{
		// Switch the way the player is labelled as facing.
		FacingRight = !FacingRight;

		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}


	public IEnumerator Taunt ()
	{
		// Check the random chance of taunting.
		float tauntChance = Random.Range ( 0f, 100f );
		if ( tauntChance > tauntProbability )
		{
			// Wait for tauntDelay number of seconds.
			yield return new WaitForSeconds ( tauntDelay );

			// If there is no clip currently playing.
			if ( !GetComponent<AudioSource> ().isPlaying )
			{
				// Choose a random, but different taunt.
				tauntIndex = TauntRandom ();

				// Play the new taunt.
				GetComponent<AudioSource> ().clip = taunts[tauntIndex];
				GetComponent<AudioSource> ().Play ();
			}
		}
	}


	int TauntRandom ()
	{
		// Choose a random index of the taunts array.
		int i = Random.Range ( 0, taunts.Length );

		// If it's the same as the previous taunt...
		if ( i == tauntIndex )
			// ... try another random taunt.
			return TauntRandom ();
		else
			// Otherwise return this index.
			return i;
	}
}
