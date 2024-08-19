using UnityEngine;

[CreateAssetMenu(fileName = "AnimalData")]
public class AnimalData : ScriptableObject
{
	public Sprite sprite;
	public Vector2 speechBubbleOffset;
	public float yOffset;
	
	[Header("Rating")]
	public Vector2 idealSizeRange;
	public float acceptableOffset = 0.25f;
	public float okayOffset = 0.5f;
	public float upsetOffset = 0.75f;
}
