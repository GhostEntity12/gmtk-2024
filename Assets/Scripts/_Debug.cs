using UnityEngine;
using UnityEngine.InputSystem;

public class _Debug : MonoBehaviour
{
	InputActions actions;

	[SerializeField] EarningsText a;

	private void Awake()
	{
		actions = new();
		actions.Debug.Enable();
	}

	// Start is called before the first frame update
	void Start()
	{
		actions.Debug.O.performed += DoTextTest;

	}

	private void DoTextTest(InputAction.CallbackContext ctx)
	{
		a.Trigger(5);
	}
}
