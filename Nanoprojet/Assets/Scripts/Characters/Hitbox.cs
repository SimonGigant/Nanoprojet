using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
	[SerializeField]private Light attackLight;
    [HideInInspector] public Fighter opponent;
	private bool _attacking = false;

	private void Start()
	{
		SetAttacking(false);
	}

	public void SetAttacking(bool newValue)
	{
		_attacking = newValue;
		attackLight.enabled = _attacking;
	}

	private void OnTriggerStay(Collider collider)
    {
		if (!_attacking) return;
        if (collider.gameObject != null && collider.gameObject.GetComponent<Fighter>() != null && collider.gameObject.GetComponent<Fighter>().Equals(opponent))
        {
			opponent.Damage(1);
            GetComponentInParent<Fighter>().SucceedAttack();
        }
    }
}
