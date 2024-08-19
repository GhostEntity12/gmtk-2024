using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    [SerializeField] SpriteRenderer animalSprite;
    [SerializeField] SpriteRenderer speechBubbleSprite;
    [SerializeField] SpriteRenderer emoteSprite;
    [SerializeField] SpriteRenderer pizzaSprite;
    [SerializeField] SpriteRenderer arrowSprite;

    [SerializeField] private Sprite[] emotes;
    [SerializeField] private Sprite[] arrows;
    
    AnimalData currentAnimal;

    public void SetAnimal(AnimalData animal)
    {
        // Cache the animal being set
        currentAnimal = animal;
        // Set up the sprites
        animalSprite.transform.localPosition = new(0, currentAnimal.yOffset, 0);
        animalSprite.sprite = currentAnimal.sprite;
        speechBubbleSprite.transform.localPosition = new(currentAnimal.speechBubbleOffset.x, currentAnimal.speechBubbleOffset.y, -0.5f);
    }

    public void SetBubble(int ranking, bool sizeIsLarger = true)
    {
        emoteSprite.sprite = emotes[ranking];
        if (ranking < 3)
        {
			// Show the indicator for if the pizza should have been bigger or smaller
            emoteSprite.transform.localPosition = new(0.35f, 0.55f, -0.1f);
			pizzaSprite.enabled = true;
			arrowSprite.enabled = true;
            arrowSprite.sprite = arrows[sizeIsLarger ? 0 : 1];
		}
        else
        {
            // Center emote sprite, hide pizza and arrow sprites
            emoteSprite.transform.localPosition = new(0.64f, 0.55f, -0.1f);
            pizzaSprite.enabled = false;
            arrowSprite.enabled = false;
        }

    }
}
