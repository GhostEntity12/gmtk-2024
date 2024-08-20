using UnityEngine;
using UnityEngine.InputSystem;

public class _Debug : MonoBehaviour
{
	InputActions actions;

	[SerializeField] Animal a;

	private void Awake()
	{
		actions = new();
		actions.Debug.Enable();
	}

	// Start is called before the first frame update
	void Start()
	{
		actions.Debug.RandomiseBubble.performed += RandomiseBubble;
		actions.Debug.ShowBubble.performed += ShowBubble;
		actions.Debug.HideBubble.performed += HideBubble;
		actions.Debug.MoveAnimalOn.performed += MoveAnimalOn;
		actions.Debug.MoveAnimalOff.performed += MoveAnimalOff;

	}

	private void RandomiseBubble(InputAction.CallbackContext ctx)
	{
		float randomSize = Random.Range(0.5f, 5f);
		int ranking = GameManager.GetRanking(a.CurrentAnimal, randomSize, out bool larger);
		a.SpeechBubble.SetValues(ranking, larger);
	}

	private void ShowBubble(InputAction.CallbackContext ctx)
	{
		a.SpeechBubble.SetVisibility(true);
	}

	private void HideBubble(InputAction.CallbackContext ctx)
	{
		a.SpeechBubble.SetVisibility(false);
	}

	private void MoveAnimalOn(InputAction.CallbackContext ctx)
	{
		// Attempt to move the animal onscreen
		if (a.Move(true))
		{
			// If the animal is going to be moved onscreen, randomise the animal.
			a.CurrentAnimal = GameManager.Instance.Animals[Random.Range(0, GameManager.Instance.Animals.Length)];
		}

	}

	private void MoveAnimalOff(InputAction.CallbackContext ctx)
	{
		a.Move(false);
	}
}
