using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensionManager : MonoBehaviour
{
    public static TensionManager _instance;
    public static TensionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<TensionManager>();
                if (_instance == null)
                {
                    GameObject container = new GameObject("TensionManager");
                    _instance = container.AddComponent<TensionManager>();
                }
            }
            return _instance;
        }
    }
    
    private float tension = 0f;

    private float tensionLoss = 15f;

    public float GetTension()
    {
        return tension;
    }

    public void AddTension(float value)
    {
        tension = Mathf.Min(100f,tension+value);
    }

    private void Update()
    {
        tension = Mathf.Max(0f, tension - tensionLoss*Time.deltaTime);
        Debug.Log(tension);
    }
}
