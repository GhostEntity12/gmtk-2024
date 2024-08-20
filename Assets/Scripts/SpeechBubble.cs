using UnityEngine;

public class SpeechBubble : MonoBehaviour
{
	[Header("Renderers")]
	[SerializeField] private SpriteRenderer speechBubbleSprite;
	[SerializeField] private SpriteRenderer emoteSprite;
	[SerializeField] private SpriteRenderer pizzaSprite;
	[SerializeField] private SpriteRenderer arrowSprite;

	[Header("Sprites")]
	[SerializeField] private Sprite[] emotes;
	[SerializeField] private Sprite largerArrow;
	[SerializeField] private Sprite smallerArrow;
    
	public void SetOffset(Vector2 offset) => transform.localPosition = new(offset.x, offset.y, -0.5f);

	public void Show(int ranking, bool sizeIsLarger = true)
	{
		// Setting up sprites
		emoteSprite.sprite = emotes[ranking];
		if (ranking < 3)
		{
			// Offset the emote
			emoteSprite.transform.localPosition = new(0.35f, 0.55f, -0.1f);
			// Show the indicator for if the pizza should have been bigger or smaller
			pizzaSprite.enabled = true;
			arrowSprite.enabled = true;
			arrowSprite.sprite = sizeIsLarger ? largerArrow : smallerArrow;
		}
		else
		{
			// Center emote sprite, hide pizza and arrow sprites
			emoteSprite.transform.localPosition = new(0.64f, 0.55f, -0.1f);
			pizzaSprite.enabled = false;
			arrowSprite.enabled = false;
		}

		// Reveal the speech bubble
		LeanTween.scale(gameObject, Vector3.one, 0.5f).setEaseOutBack();
	}

	public void Hide()
	{
		LeanTween.scale(gameObject, Vector3.zero, 0.5f).setEaseInBack();
	}
}
