using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Fighter opponent;

    private void OnTriggerStay(Collider collider)
    {
        if (collider.gameObject != null && collider.gameObject.GetComponent<Fighter>() != null && collider.gameObject.GetComponent<Fighter>().Equals(opponent))
        {
            opponent.Damage(1);
            GetComponentInParent<Fighter>().SucceedAttack();
        }
    }
}
