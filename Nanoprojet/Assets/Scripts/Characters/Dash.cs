﻿using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Fighter))]
public class Dash : MonoBehaviour
{
	//private fields
	private Fighter fighter;
	private float counter;
	private bool doDash = false;
	private Rigidbody rb;
	private CharacterController controller;
	private Vector3 startPosition;
	private Vector3 direction;

	//Light
	[SerializeField]private Light dashLight;
	[SerializeField]private float lightFlashDuration = 1;




	//serialized field
	[SerializeField]private float duration;
	[SerializeField]private float distance;
	[SerializeField]private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));


	private void Awake()
	{
		fighter = GetComponent<Fighter>();
		//rb = GetComponent<Rigidbody>();
		controller = GetComponent<CharacterController>();
		dashLight.enabled = false;
	}

	private void OnEnable()
	{
		fighter.OnStateChange += OnStateChange;
	}

	private void OnDisable()
	{
		fighter.OnStateChange -= OnStateChange;
	}

	private void OnStateChange(FighterState newState)
	{
		if (newState == FighterState.Dash)
		{
			counter = 0;
			doDash = true;
			startPosition = transform.position;
			direction = new Vector3(fighter.direction.x, 0, fighter.direction.y);
			dashLight.enabled = true;
			dashLight.transform.parent = null;
			dashLight.transform.position = transform.position;
		}
		else
		{
			doDash = false;
			dashLight.enabled = false;
		}
	}

	private void Update()
	{
		if (doDash)
		{
			Vector3 prevPos = startPosition + direction * distance * curve.Evaluate(counter / duration);
			counter += Time.deltaTime;		
			Vector3 nextPos = startPosition + direction  * distance * curve.Evaluate(counter / duration);
			//rb.MovePosition();
			controller.Move(nextPos - prevPos);
			//Debug.Log(counter);
			if(counter > lightFlashDuration)
			{
				dashLight.enabled = false;
			}
			if (counter >= duration)
			{
				fighter.DashEnd();
			}
		}
	}
}