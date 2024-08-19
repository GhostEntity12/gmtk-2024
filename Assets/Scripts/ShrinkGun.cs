using UnityEngine;
using UnityEngine.InputSystem;

public class ShrinkGun : MonoBehaviour
{
	public enum Beam { None, Grow, Shrink }

	[SerializeField] private ParticleSystem beamGrow;
	[SerializeField] private ParticleSystem beamShrink;

	[SerializeField] private Pizza pizza;

	public Beam ActiveBeam { get; private set; } = Beam.None;

	private InputActions inputs;

	private void Start()
	{
		// Caching InputActions
		inputs = GameManager.Instance.Inputs;
		// Add callbacks
		inputs.Ray.Grow.started += GrowRayStart;
		inputs.Ray.Grow.canceled += GrowRayEnd;
		inputs.Ray.Shrink.started += ShrinkRayStart;
		inputs.Ray.Shrink.canceled += ShrinkRayEnd;
	}

	/// <summary>
	/// When Grow Ray button is pressed
	/// </summary>
	/// <param name="ctx"></param>
	private void GrowRayStart(InputAction.CallbackContext ctx)
	{
		// Disable the other beam if it's active
		if (ActiveBeam == Beam.Shrink)
		{
			beamShrink.Stop();
		}
		// Enable the beam
		ActiveBeam = Beam.Grow;
		beamGrow.Play();
	}

	/// <summary>
	/// When Grow Ray button is released
	/// </summary>
	/// <param name="ctx"></param>
	private void GrowRayEnd(InputAction.CallbackContext ctx)
	{
		// Disable the beam if it's active
		if (ActiveBeam == Beam.Grow)
		{
			beamGrow.Stop();
		}
		// If the other beam's button is pressed, activate it
		if (inputs.Ray.Shrink.IsPressed())
		{
			ActiveBeam = Beam.Shrink;
			beamShrink.Play();
			return;
		}
		ActiveBeam = Beam.None;
	}

	/// <summary>
	/// When Shrink Ray button is pressed
	/// </summary>
	/// <param name="ctx"></param>
	private void ShrinkRayStart(InputAction.CallbackContext ctx)
	{
		// Disable the other beam if it's active
		if (ActiveBeam == Beam.Grow)
		{
			beamGrow.Stop();
		}
		// Enable the beam
		ActiveBeam = Beam.Shrink;
		beamShrink.Play();
	}

	/// <summary>
	/// When Grow Ray button is released
	/// </summary>
	/// <param name="ctx"></param>
	private void ShrinkRayEnd(InputAction.CallbackContext ctx)
	{
		// Disable the beam if it's active
		if (ActiveBeam == Beam.Shrink)
		{
			beamShrink.Stop();
		}
		// If the other beam's button is pressed, activate it
		if (inputs.Ray.Grow.IsPressed())
		{
			ActiveBeam = Beam.Grow;
			beamGrow.Play();
			return;
		}
		ActiveBeam = Beam.None;
	}

	private void Update()
	{
		if (pizza)
		{
			pizza.ChangeSize(ActiveBeam);
		}
	}
}
