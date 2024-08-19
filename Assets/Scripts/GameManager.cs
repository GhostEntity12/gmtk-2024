using UnityEngine;

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


	// Start is called before the first frame update
	protected override void Awake()
	{
		base.Awake();
		Application.targetFrameRate = 60;

		// Create new InputActions
		Inputs = new();
	}
	void Start()
	{
		// Enable the Gameplay Inputs
		Inputs.GeneralGameplay.Enable();
	}

	/// <summary>
	/// Returns a rank between 0 and 4 based on the size of the pizza for the given animal
	/// </summary>
	/// <param name="animal">The given animal.</param>
	/// <param name="size">The size of the pizza.</param>
	/// <param name="sizeIsLarger">Whether the size is larger or smaller than the ideal range. Defaults to larger if the value is in the ideal range.</param>
	/// <returns></returns>
	static int GetRanking(AnimalData animal, float size, out bool sizeIsLarger)
	{
		// If falls within the ideal range
		if (size > animal.idealSizeRange.x && size < animal.idealSizeRange.y)
		{
			// Default value to prevent errors
			sizeIsLarger = true;
			return 4;
		}
		// Get the difference from the ideal range
		float delta =
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
}