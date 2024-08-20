using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
	private AnimalData currentAnimal;
	
	[SerializeField] private SpriteRenderer animalSprite;
	[SerializeField] private SpeechBubble speechBubble;

	public AnimalData CurrentAnimal
	{
		get => currentAnimal;
		set
		{
			currentAnimal = value;
			// Set up the sprites
			animalSprite.transform.localPosition = new(0, currentAnimal.yOffset, 0);
			animalSprite.sprite = currentAnimal.sprite;
			speechBubble.SetOffset(currentAnimal.speechBubbleOffset);
		}
	}
}
