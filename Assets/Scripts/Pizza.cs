using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pizza : MonoBehaviour
{
	[SerializeField] private float scaleSpeed = 1;
	[SerializeField] private Vector2 sizeRange = new(0.1f, 5);
	private float scaleFactor = 1;

	public void ChangeSize(ShrinkGun.Beam effect)
	{
		switch (effect)
		{
			case ShrinkGun.Beam.Grow:
				scaleFactor += Time.deltaTime * scaleSpeed;
				break;
			case ShrinkGun.Beam.Shrink:
				scaleFactor -= Time.deltaTime * scaleSpeed;
				break;
		}
		scaleFactor = Mathf.Clamp(scaleFactor, sizeRange.x, sizeRange.y);
		transform.localScale = Vector3.one * scaleFactor;
	}
}
