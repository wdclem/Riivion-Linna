using UnityEngine;

public class	EntityMotor : MonoBehavior
{
	// Components
	RigidBody2D	rb;
	Collider2D	hitbox;
	Animator	anim;

	// Class references
	private InputManager	input;
	
	[Header("Properties")]
	public float	speed;
	[Space(2)]
	public float	jumpH;
	public int		jumps;	// For mid-air jumping
	public float	coyoteTime;

//	==========
	private float	runF = Vector2.zero;
	private Vector2	_velocity;
	
	private float	jumpF = 2f;
	private float	_coyoteT;
	private int		_jumps = 0;
	private bool	queJump = false;
	private bool	isGrounded = true;

	// [0f - 1f] ratio of constraint to apply on movement, 1 to lock
	private float	moveLock = 0f;
	private float	jumpLock = 0f;

	private const float	groundOffset = 0.05f;
//	####################################################################################
//	RUNTIME ====

	// Runs once when the GameObject is initialized	(Unity built-in)
	private void	Awake()
	{
		rb = GetComponent<RigidBody2D>();
		anim = GetComponent<Animator>();

		input = InputManager.instance;	// Singleton:	make sure to set the script exec order for InputManager early!
	}

	// Runs once on the first Update()	(Unity built-in)
	private void	Start()
	{
		// calculate force needed to jump to jumpH height in units
		jumpF = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpH);
	}



	// Runs once per frame before rendering phase, good for basic logic	(Unity built-in)
	private void	Update()
	{
		queJump = input.jump;

		SetAnimationFlags();
	}

	// Runs once per fixed timestep, used for physics	(Unity built-in)
	private void	FixedUpdate()
	{
		_velocity = rb.velocity;
		isGrounded = Physics2D.Raycast(hitbox.center, Vector2.down, hitbox.extents.y + groundOffset) != null;

		CallAction_Walk();
		CallAction_Jump();
		
		rb.velocity = _velocity;
	}

//	####################################################################################
//	FUNCTIONS ====

	private void	CallAction_Walk()
	{
		if (moveLock == 1f)
			return ;
		// calculate force needed to reach target velocity from current velocity
		runF = -rb.velocity.x + input.moveX * speed;
		_velocity.x += runF * (1f - moveLock);
	}

	private void	CallAction_Jump()
	{
		if (isGrounded)
		{
			_jumps = jumps;
			_coyoteT = coyoteTime;
		}
		else
		{
			if (_coyoteT > 0f)
				_coyoteT -= Time.deltaTime;
			else
			{
				_jumps--;
				_coyoteT = 0f;
			}
		}
		
		if (jumpLock == 1f)
			return ;
		if (queJump && jumps > 0)
		{
			_jumps--;
			_velocity.y = jumpF * (1f - jumpLock);
		}
	}

	private void	SetAnimationFlags()
	{
	}

	public void		SetMotorLock(float moveL, float jumpL)
	{
		moveLock = moveL;
		jumpLock = jumpL;
	}

//	####################################################################################
//	DEBUG ====

	// In editor, draws debug GUI elements when GameObject is selected	(Unity built-in)
	private void	OnDrawGizmosSelected()
	{
		Vector3	feetPos = (Vector3)hitbox.center + Vector3.down * hitbox.extents.y;

		Gizmos.color = Color.yellow;
		Gizmos.DrawLine(feetPos, feetPos + jumpH);
	}
}
