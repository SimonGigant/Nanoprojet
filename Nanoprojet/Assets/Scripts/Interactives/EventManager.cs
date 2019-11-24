using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager _instance;
    public static EventManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<EventManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("EventManager");
                    _instance = container.AddComponent<EventManager>();
                }
            }
            return _instance;
        }
    }

    private float counter = 0f;
    private float carSpawnCooldown = 5f;

    private void Update()
    {
        counter += Time.deltaTime;
        if (counter > carSpawnCooldown)
        {
            counter = 0f;
            if (Random.value > 0.5f)
            {
                Car.SpawnCar();
                if(Random.value > 0.5f)
                {
                    StartCoroutine("SpawnCarAfterABit");
                }
            }
            else
            {
                Lightning.SpawnLightning();
                StartCoroutine("SpawnLightningAfterABit");
            }
        }
    }

    private IEnumerator SpawnCarAfterABit()
    {
        float rand = Random.Range(0.4f, 1.5f);
        yield return new WaitForSeconds(rand);
        Car.SpawnCar();
    }

    private IEnumerator SpawnLightningAfterABit()
    {
        float rand = Random.Range(0.15f, 1.5f);
        yield return new WaitForSeconds(rand);
        Lightning.SpawnLightning();
        if(Random.value > 0.8f)
        {
            rand = Random.Range(0.15f, 0.6f);
            yield return new WaitForSeconds(rand);
            Lightning.SpawnLightning();
        }
    }
}
