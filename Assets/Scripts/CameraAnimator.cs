using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CameraAnimator : MonoBehaviour
{
	[SerializeField] AudioClip cameraMoveSFX;
	// State tracking
	public bool FacingFront { get; private set; } = true;
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

	public void StartGame()
	{
		anim.SetTrigger("FromMenu");
		GameManager.Instance.ToggleAudioGameplay();
		GameManager.Instance.StartDay(true);
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
			if (FacingFront) inputs.Counter.Disable();
			else inputs.Ray.Disable();

			// Store whether the camera is facing forward
			FacingFront = !FacingFront;

			// Trigger the animation
			anim.SetBool("FacingFront", FacingFront);
			// Flag as animating
			isAnimating = true;
			// Move the pizza
			GameManager.Instance.Pizza.MovePizza(FacingFront);
			// Play audio
			GameManager.Instance.PlaySFX(cameraMoveSFX);
		}
	}

	/// <summary>
	/// Sets isAnimating to false. Called by AnimationEvents.
	/// </summary>
	public void OnFinishAnimating()
	{
		isAnimating = false;

		// Enable appropriate inputs
		if (FacingFront) inputs.Counter.Enable();
		else inputs.Ray.Enable();
	}

	public void EndDay() => anim.SetTrigger("EndDay");

	public void ResetCamera()
	{
		anim.SetTrigger("StartDay");
	}
}
