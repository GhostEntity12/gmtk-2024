using UnityEngine;

public class Animal : MonoBehaviour
{
	private AnimalData currentAnimal;
	private Animator anim;
	private bool onscreen;
	public bool IsMoving { get; private set; }

	[SerializeField] private SpriteRenderer animalSprite;

	[field: SerializeField] public SpeechBubble SpeechBubble { get; private set; }

	public AnimalData Data
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
	public bool Move(bool moveOnscreen, bool forced = false)
	{
		// Fail if already moving
		if (IsMoving && !forced) return false;

		// Animal is alrady in the requested position
		if ((moveOnscreen && onscreen) || (!moveOnscreen && !onscreen))
		{
			string status = onscreen ? "onscreen" : "offscreen";
			Debug.LogError($"Attempting to make the animal walk {status} when the animal is already {status}");
			return false;
		}
		// Move the animal
		anim.SetTrigger(moveOnscreen ? "WalkOn" : "WalkOff");
		onscreen = moveOnscreen;
		IsMoving = true;
		return true;
	}

	// Called by AnimationEvent
	public void HideBubble() => SpeechBubble.SetVisibility(false);

	// Called by AnimationEvent
	public void OnFinishMoving()
	{
		IsMoving = false;
		// If finished moving offscreen, generate a new animal
		if (!onscreen)
		{
			GameManager.Instance.Pizza.SpawnPizza();
			
			if (GameManager.Instance.IsEndOfDay) return;
			
			SpawnNewAnimal();
		}
	}

	public void SpawnNewAnimal()
	{
		// Begin moving the sprite
		if (Move(true))
		{
			// Randomise the animal
			Data = GameManager.Instance.Animals[Random.Range(0, GameManager.Instance.Animals.Length)];
		}
	}
}
