using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    public Fighter opponent;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Fighter>().Equals(opponent))
        {
            opponent.Damage(1);
            Destroy(this);
        }
    }
}
