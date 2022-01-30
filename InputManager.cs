using UnityEngine;

public class	InputManager : Monobehavior
{
	// Singleton
	public static InputManager	instance;
//	==========
	[HideInInspector] public float	moveX;
	[HideInInspector] public bool	jump;
//	==========
	private bool	lockControl = false;

//	####################################################################################
//	RUNTIME ====

	// Runs once when the GameObject is initialized	(Unity built-in)
	private void	Awake()
	{
		if (instance == null)
			instance = this;
		else Destroy(this.gameObject);
	}

	// Runs once per frame before rendering phase	(Unity built-in)
	private void	Update()
	{
		if (lockControl)
			return ;
		moveX = Input.GetAxisRaw("Horizontal");
		jump = Input.GetButtonDown("Jump");
	}

//	####################################################################################
//	FUNCTIONS ====

	public void	SetControlLock(bool _lock)
	{
		lockControl = _lock;
	}

//	####################################################################################
//	DEBUG ====

	// In editor, draws debug GUI elements when GameObject is selected	(Unity built-in)
	private void	OnDrawGizmosSelected()
	{
	}
}
