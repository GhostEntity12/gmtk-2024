using UnityEngine;

public class Animal : MonoBehaviour
{
	private AnimalData currentAnimal;
	private Animator anim;
	private bool onscreen;

	[SerializeField] private SpriteRenderer animalSprite;

	[field: SerializeField] public SpeechBubble SpeechBubble { get; private set; }

	public AnimalData CurrentAnimal
	{
		get => currentAnimal;
		set
		{
			currentAnimal = value;
			// Set up the sprites
			animalSprite.transform.localPosition = new(0, currentAnimal.yOffset, 0);
			animalSprite.sprite = currentAnimal.sprite;
			SpeechBubble.SetOffset(currentAnimal.speechBubbleOffset);
		}
	}

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	/// <summary>
	/// Attempts to move the animal.
	/// </summary>
	/// <param name="moveOnscreen">Use true if trying to move the animal onscreen, or false if trying to move the animal offscreen</param>
	/// <returns>Returns if the movement started correctly</returns>
	public bool Move(bool moveOnscreen)
	{
		if ((moveOnscreen && onscreen) || (!moveOnscreen && !onscreen))
		{
			string status = onscreen ? "onscreen" : "offscreen";
			Debug.LogError($"Attempting to make the animal walk {status} when the animal is already {status}");
			return false;
		}
		anim.SetTrigger(moveOnscreen ? "WalkOn" : "WalkOff");
		onscreen = moveOnscreen;
		return true;
	}
}
