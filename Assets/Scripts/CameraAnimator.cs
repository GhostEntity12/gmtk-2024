using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CameraAnimator : MonoBehaviour
{
	// State tracking
	private bool facingFront = true;
	private bool isAnimating = false;

	private Animator anim;

	private InputActions inputs;

	private void Awake()
	{
		// Get components
		anim = GetComponent<Animator>();
	}

	private void Start()
	{
		// Caching InputActions
		inputs = GameManager.Instance.Inputs;
		// Add callbacks
		inputs.GeneralGameplay.SwingCamera.performed += Swing;
		// Enable Counter by default
		inputs.Counter.Enable();
	}

	/// <summary>
	/// Swings the camera
	/// </summary>
	/// <param name="ctx">Input context</param>
	public void Swing(InputAction.CallbackContext ctx)
	{
		// If currently animating, don't do anything
		if (!isAnimating)
		{
			// Disable active inputs
			if (facingFront) inputs.Counter.Disable();
			else inputs.Ray.Disable();

			// Store whether the camera is facing forward
			facingFront = !facingFront;

			// Trigger the animation
			anim.SetBool("FacingFront", facingFront);
			// Flag as animating
			isAnimating = true;
		}
	}

	/// <summary>
	/// Sets isAnimating to false. Called by AnimationEvents.
	/// </summary>
	public void OnFinishAnimating()
	{
		isAnimating = false;

		// Enable appropriate inputs
		if (facingFront) inputs.Counter.Enable();
		else inputs.Ray.Enable();
	}
}
