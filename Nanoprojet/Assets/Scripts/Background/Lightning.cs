using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    private static GameObject lightningPrefab;
    private float counter = 0f;
    private float duration = 0.12f;
    
    void Update()
    {
        counter += Time.deltaTime;
        if (counter > duration)
        {
            Destroy(gameObject);
        }
    }

    public static void SpawnLightning()
    {
        lightningPrefab = (GameObject)Resources.Load("Prefabs/Environment/Lightning", typeof(GameObject));
        Lightning l = Object.Instantiate(lightningPrefab, new Vector3(), new Quaternion()).GetComponent<Lightning>();
        Gamefeel.Instance.InitScreenshake(0.2f,0.5f);
    }
}
