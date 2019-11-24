using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private static GameObject carPrefab;
    private static float distance = 10f;


    private float speed = 20f;
    private float counter = 0f;
    public float direction = 0f;

    void Update()
    {
        counter += Time.deltaTime;
        transform.position += new Vector3(Time.deltaTime * speed * -direction,0f,0f);
        if(counter > 1f)
        {
            Destroy(gameObject);
        }
    }

    public static void SpawnCar()
    {
        float direction = 1f;
        if (Random.value >= 0.5f)
        {
            direction = -1f;
        }
        carPrefab = (GameObject)Resources.Load("Prefabs/Environment/Car", typeof(GameObject));
        GameObject o = Object.Instantiate(carPrefab,new Vector3(distance * direction,0f,0f), new Quaternion());
        o.GetComponent<Car>().direction = direction;
    }
}
