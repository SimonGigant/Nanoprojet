using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Fighter))]
public class Dash : MonoBehaviour
{
	//private fields
	private Fighter fighter;
	private float counter;
	private Physics physics;
	private Vector3 startPosition;
	private Vector3 direction;

	//serialized field
	[SerializeField]private float duration;
	[SerializeField]private float distance;
	[SerializeField]private AnimationCurve curve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));


	private void Awake()
	{
		fighter = GetComponent<Fighter>();
		physics = GetComponent<Physics>();
	}

	public void InitDash()
	{
		counter = 0;
		startPosition = transform.position;
		direction = new Vector3(fighter.direction.x, 0, fighter.direction.y).normalized;
	}

	private void Update()
	{
		if(fighter.currentState == FighterState.Dash)
		{
			Vector3 prevPos = startPosition + direction * distance * curve.Evaluate(counter / duration);
			counter += Time.deltaTime;
			Vector3 nextPos = startPosition + direction * distance * curve.Evaluate(counter / duration);
			physics.directMove(nextPos - prevPos);
			if (counter >= duration)
			{
				fighter.DashEnd();
			}
		}

	}
}
