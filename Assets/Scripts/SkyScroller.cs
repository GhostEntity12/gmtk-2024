using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyScroller : MonoBehaviour
{
	[SerializeField] float dayLength = 20;
	float daytimeElapsed;
	bool progressTime = false;

	// Update is called once per frame
	void Update()
	{
		// Time progression is disabled
		if (!progressTime) return;

		daytimeElapsed += Time.deltaTime;
		if (daytimeElapsed > dayLength)
		{
			// Day is over
			progressTime = false;
			GameManager.Instance.OnDayFinished();
		}
		transform.position = new(0, Mathf.Lerp(-35, 40, daytimeElapsed / dayLength), 8);
	}

	public void Enable() => progressTime = false;

	public void ResetSky()
	{
		daytimeElapsed = 0;
		transform.position = new(0, -35, 8);
	}
}
