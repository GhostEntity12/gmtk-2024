using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScroller : MonoBehaviour
{
	[SerializeField] float dayLength = 20;
	float daytimeElapsed;

	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		daytimeElapsed += Time.deltaTime / dayLength;
		transform.position = new(0, Mathf.Lerp(-35, 40, daytimeElapsed / dayLength), 8);
	}
}
