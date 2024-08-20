using TMPro;
using UnityEngine;

public class EarningsText : MonoBehaviour
{
	private TextMeshPro text;
	[SerializeField] AudioClip coinAudio;

	private void Awake()
	{
		text = GetComponent<TextMeshPro>();
	}

	public void Trigger(int amount)
	{
		text.text = $"<size=2>+</size>{amount}<sprite=\"Spr_Coin\" index=0>";
		LeanTween.moveLocalY(gameObject, 1.25f, 1.2f).setEaseOutQuad();
		LeanTween.value(gameObject, SetTextAlpha, 1, 0, 1.2f).setEaseInSine();
		Invoke(nameof(ResetPosition), 1.3f);
		GameManager.Instance.PlaySFX(coinAudio);
	}

	void ResetPosition()
	{
		transform.localPosition = new(0, 1, 0);
	}

	void SetTextAlpha(float a)
	{
		text.color = new(text.color.r, text.color.g, text.color.b, a);
	}

}
