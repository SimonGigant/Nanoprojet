using UnityEngine;
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


	//serialized field
	[SerializeField]private float duration;
	[SerializeField]private float distance;
	[SerializeField]private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));


	private void Awake()
	{
		fighter = GetComponent<Fighter>();
		//rb = GetComponent<Rigidbody>();
		controller = GetComponent<CharacterController>();

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
		}
		else
		{
			doDash = false;
		}
	}

	private void Update()
	{
		if (doDash)
		{
			Vector3 prevPos = startPosition + direction * distance * curve.Evaluate(counter);
			counter += Time.deltaTime / duration;		
			Vector3 nextPos = startPosition + direction  * distance * curve.Evaluate(counter);
			//rb.MovePosition();
			controller.Move(nextPos - prevPos);
			//Debug.Log(counter);
			if (counter >= 1)
			{
				fighter.DashEnd();
			}
		}
	}
}
