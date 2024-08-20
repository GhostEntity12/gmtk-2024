using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EndOfDayDisplay : MonoBehaviour
{
	RectTransform rect;
	CanvasGroup newRecordCanvas;

	int highScore = 0;
	
	[SerializeField] TextMeshProUGUI todayScoreText;
	[SerializeField] TextMeshProUGUI highScoreText;
	[SerializeField] RectTransform newRecordRect;

	private void Awake()
	{
		rect = GetComponent<RectTransform>();
		newRecordCanvas = newRecordRect.GetComponent<CanvasGroup>();
	}

		
	public void Show(int todayScore)
	{
		if (highScore < todayScore)
		{
			highScore = todayScore;
			highScoreText.text = $"{highScore}<sprite=\"Spr_Coin\" index=0>";
			LeanTween.alphaCanvas(newRecordCanvas, 1, 0.2f).setDelay(2.9f).setEaseOutCubic();
			LeanTween.scale(newRecordRect, Vector3.one, 0.2f).setDelay(2.9f).setEaseOutCubic();
		}
		todayScoreText.text = $"{todayScore}<sprite=\"Spr_Coin\" index=0>";
		LeanTween.move(rect, Vector3.zero, 0.6f).setEaseOutBack().setDelay(2.1f);
	}

	public void ResetPos()
	{
		rect.localPosition = new(0, 1080, 0);
		newRecordRect.localScale = Vector3.one * 1.3f;
		newRecordCanvas.alpha = 0;
	}
}
