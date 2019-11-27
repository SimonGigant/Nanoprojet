using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//require area light in children

public class TVFlicker : MonoBehaviour
{

	[SerializeField]private Material tvOn;
	[SerializeField]private Material tvOff;
	[SerializeField]private float duration = 1;
	[SerializeField]private Vector2	minMaxLightIntensity = new Vector2(9,10);

	private Light areaLight;
	private MeshRenderer rend;
	private bool isRunning = false;

	private void Awake()
	{
		rend = GetComponent<MeshRenderer>();
		areaLight = GetComponentInChildren<Light>();
		Trigger();
	}

    private void Update()
    {
		if(Random.value > 0.99f)
		{
			Trigger();
		}
	}

	public void Trigger()
	{
		if (!isRunning) StartCoroutine(TVRoutine());
	}

	private IEnumerator TVRoutine()
	{
		isRunning = true;
		rend.material = tvOn;
		float t = 0;
		while (t < duration)
		{
			areaLight.intensity = Random.Range(minMaxLightIntensity.x, minMaxLightIntensity.y);
			t += Time.deltaTime;
			yield return null;
		}
		rend.material = tvOff;
		areaLight.intensity = 0;
		isRunning = false;
	}
}
