using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : Singleton<GameManager>
{
	/// <summary>
	/// Publicly available InputActions. Should be referenced and cached on each object that uses it.
	/// </summary>
	public InputActions Inputs { get; private set; }
	/// <summary>
	/// Public property to access animal data
	/// </summary>
	public AnimalData[] Animals => animals;
	
	/// <summary>
	/// List of animal data that can be picked from
	/// </summary>
	[SerializeField] private AnimalData[] animals = new AnimalData[0];

	/// <summary>
	/// Holds the "active animal" object. Shouldn't be overwritten at any point.
	/// </summary>
	[field: SerializeField] public Animal ActiveAnimal { get; private set; }

	[field: SerializeField] public Pizza Pizza { get; private set; }
	
	[SerializeField] CameraAnimator camAnim;
	[SerializeField] private EarningsText earnings;
	[SerializeField] private SkyScroller sky;
	[SerializeField] private EndOfDayDisplay endOfDayDisplay;
	[SerializeField] private CanvasGroup blackoutCanvas;
	[SerializeField] AudioClip menuBGM;
	[SerializeField] AudioClip gameplayBGM;
	[SerializeField] float transitionDuration;

	[SerializeField] private int basePrice = 10;
	[SerializeField] private int tipPrice = 5;
	[SerializeField] private float badPizzaModifier = 2.5f;

	private AudioSource audioSource;

	private int score;
	public bool IsEndOfDay { get; private set; } = false;

	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();
		Application.targetFrameRate = 60;

		audioSource = GetComponent<AudioSource>();

		// Create new InputActions
		Inputs = new();
	}

	/// <summary>
	/// Returns a rank between 0 and 4 based on the size of the pizza for the given animal
	/// </summary>
	/// <param name="animal">The given animal.</param>
	/// <param name="size">The size of the pizza.</param>
	/// <param name="sizeIsLarger">Whether the size is larger or smaller than the ideal range. Defaults to larger if the value is in the ideal range.</param>
	/// <returns></returns>
	public static int GetRanking(AnimalData animal, float size, out bool sizeIsLarger, out float delta)
	{
		delta = 0;
		// If falls within the ideal range
		if (size > animal.idealSizeRange.x && size < animal.idealSizeRange.y)
		{
			// Default value to prevent errors
			sizeIsLarger = true;
			return 4;
		}
		// Get the difference from the ideal range
		delta =
			size < animal.idealSizeRange.x ?
			animal.idealSizeRange.x - size :
			animal.idealSizeRange.y - size;
		// If delta > 0 then the size provided was larger
		sizeIsLarger = delta > 0;
		// Get the absolute value so delta is always positive
		delta = Mathf.Abs(delta);

		// Return the appropriate value based on the other other offsets
		return delta switch
		{
			float i when i <= animal.acceptableOffset => 3,
			float i when i <= animal.okayOffset => 2,
			float i when i <= animal.upsetOffset => 1,
			_ => 0,
		};
	}

	public void EvaluatePizza(float size)
	{
		int ranking = GetRanking(ActiveAnimal.Data, size, out bool larger, out float delta);
		ActiveAnimal.SpeechBubble.SetValues(ranking, larger);
		ActiveAnimal.SpeechBubble.SetVisibility(true);
		audioSource.PlayOneShot(ActiveAnimal.Data.sounds[ranking]);
		ActiveAnimal.Move(false);

		int s = GetScore(delta, ranking);
		earnings.Trigger(s);
		score += s;
	}

	public int GetScore(float delta, int ranking) => Mathf.Max(0,
		ranking switch
		{
			0 => 0,
			3 => basePrice,
			4 => basePrice + tipPrice,
			_ => basePrice - (int)(delta * badPizzaModifier)
		}
	);


	public void OnDayFinished()
	{
		IsEndOfDay = true;
		ActiveAnimal.Move(false, true);
		Inputs.GeneralGameplay.Disable();
		Inputs.Counter.Disable();
		Inputs.Ray.Disable();
		camAnim.EndDay();
		endOfDayDisplay.Show(score);
	}

	public void StartDay(bool initialSetup = false)
	{
		if (!initialSetup)
			camAnim.ResetCamera();
		ActiveAnimal.SpawnNewAnimal();
		Inputs.GeneralGameplay.Enable();
		Inputs.Counter.Enable();
		sky.ResetSky();
		endOfDayDisplay.ResetPos();
		score = 0;
		IsEndOfDay = false;
		LeanTween.alphaCanvas(blackoutCanvas, 0, 0.5f).setDelay(0.2f);
		blackoutCanvas.blocksRaycasts = blackoutCanvas.interactable = false;
	}

	public void FadeOut()
	{
		LeanTween.alphaCanvas(blackoutCanvas, 1, 0.5f).setOnComplete(() => StartDay());
		blackoutCanvas.blocksRaycasts = blackoutCanvas.interactable = true;
	}

	public void PlaySFX(AudioClip clip) => audioSource.PlayOneShot(clip);

	public void ToggleAudioGameplay() => ToggleAudio(gameplayBGM);
	public void ToggleAudioMenu() => ToggleAudio(menuBGM);

	private void ToggleAudio(AudioClip clip)
	{
		LeanTween.value(gameObject, SetAudioVolume, 1, 0, transitionDuration / 2).setOnComplete(() =>
		{
			audioSource.Stop();
			audioSource.clip = clip;
			audioSource.Play();
			LeanTween.value(gameObject, SetAudioVolume, 0, 1, transitionDuration / 2);
		});
	}

	private void SetAudioVolume(float v) => audioSource.volume = v;
}