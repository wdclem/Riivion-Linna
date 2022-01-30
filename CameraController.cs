using UnityEngine;

public class	CameraController : MonoBehavior
{
	// Components
	private Camera	cam;

	// Class references
	private InputManager	input;

	[Header("Properties")]
	public float	runOffset; //	how much to look ahead in running direction, camera should center when idle
	public float	jumpOffset; //	how much camera should be offset vertically when mid-air, ideally downwards so player can see where they're falling
	public float	smoothTime; //	time it takes to transition from current to target position

//	==========
	private Transform	target;
	private Vector2		_smoothDampV = Vector2.zero;
	private static float	zoffset = 0f;

//	####################################################################################
//	RUNTIME ====

	// Runs once when the GameObject is initialized	(Unity built-in)
	private void	Awake()
	{
		cam = GetComponent<Camera>();
		input = InputManager.instance;
		target = Player.instance.transform;
	}

	// Runs once on the first Update()	(Unity built-in)
	private void	Start()
	{
		zoffset = transform.position.z;
	}



	// Runs once per frame after rendering phase	(Unity built-in)
	private void	LateUpdate()
	{
		Vector2	_pos = (Vector2)transform.position;
		Vector2	_targetpos = (Vector2)target.position;
		
		_targetpos.x += runOffset * input.moveX;
		_targetpos.y += jumpOffset;

		Vector2	positionTarget = Vector2.SmoothDamp(_pos, _targetpos, ref _smoothDampV, smoothTime);
		transform.position = new Vector3(positionTarget.x, positionTarget.y, zoffset);
	}

//	####################################################################################
//	FUNCTIONS ====

//	####################################################################################
//	DEBUG ====

	// In editor, draws debug GUI elements when GameObject is selected	(Unity built-in)
	private void	OnDrawGizmosSelected()
	{
	}
}
