using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pizza : MonoBehaviour
{
	public enum Status { AtRay, AtCounter, Serving }

	[SerializeField] private float scaleSpeed = 1;
	[SerializeField] private Vector2 sizeRange = new(0.1f, 5);

	private float scaleFactor = 1;
	private bool isMoving = false;
	public Status currentStatus = Status.AtRay;
	private Animator anim;


	private InputActions inputs;

	private void Awake()
	{
		anim = GetComponent<Animator>();
	}

	private void Start()
	{
		inputs = GameManager.Instance.Inputs;

		inputs.Counter.Serve.performed += ServePizza;
	}

	private void ServePizza(InputAction.CallbackContext ctx)
	{
		if (currentStatus != Status.AtCounter || GameManager.Instance.ActiveAnimal.IsMoving) return;
		currentStatus = Status.Serving;
		anim.SetTrigger("Serve");
		GameManager.Instance.EvaluatePizza(scaleFactor);
	}

	public void ChangeSize(ShrinkGun.Beam effect)
	{
		if (isMoving) return;

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

	public void MarkNotMoving()
	{
		isMoving = false;

		if (currentStatus == Status.Serving)
		{
			transform.localScale = Vector3.one;
			scaleFactor = 1;
		}
	}

	public void MovePizza(bool toFrontCounter)
	{
		anim.SetBool("AtFrontCounter", toFrontCounter);
		currentStatus = toFrontCounter ? Status.AtCounter : Status.AtRay;
	}

	public void SpawnPizza()
	{
		anim.SetTrigger("SpawnPizza");
		anim.SetBool("AtFrontCounter", false);
	}
}
