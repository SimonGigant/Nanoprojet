using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class Physics : MonoBehaviour
{
	//inspector values
	[SerializeField]
	private float maxSpeed = 1;
	[SerializeField]
	private float acceleration = 1;
	[SerializeField]
	private float deceleration = 1;
	[SerializeField]
	private float rotationLerp = 10f;

	//private value
	private Vector3 currentDirection;
	[SerializeField, DisplayWithoutEdit]
	private float currentSpeed;
	private Vector3 gravity = new Vector3(0, -10, 0);
	private bool isForce;

	//accessors
	public float speed => currentSpeed;
	public float velocity => controller.velocity.magnitude;
	public float orientationVelocity => Mathf.Sign( Vector3.Dot(controller.velocity.normalized, transform.forward));
	

	//external component
	private CharacterController controller;

	private void Awake()
	{
		controller = GetComponent<CharacterController>();
	}

	public void AddForce(Vector3 dir)
	{
		if (dir.magnitude < 0.01f) return;//to do check deadzone

		currentDirection = Vector3.Lerp(currentDirection, dir, rotationLerp * Time.deltaTime);

		isForce = true;
	}

	public void directMove(Vector3 move)
	{
		controller.Move(move);
	}

	private void Update()
	{
		if (isForce)
		{
			currentSpeed = Mathf.Clamp(currentSpeed + acceleration * Time.deltaTime, 0, maxSpeed);
		}
		else
		{
			currentSpeed = Mathf.Clamp(currentSpeed - deceleration * Time.deltaTime, 0, maxSpeed);
		}
		controller.Move((currentDirection * currentSpeed + gravity) * Time.deltaTime);
		isForce = false;
	}

}