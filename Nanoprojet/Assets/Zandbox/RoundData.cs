using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "Round", menuName = "NanoProject/RoundData", order = 0)]
public class RoundData : ScriptableObject
{
	[SerializeField]
	private float roundDuration = 60;
	
	[SerializeField]
	private AnimationCurve gameFeelIntensity = new AnimationCurve(new Keyframe(0, 1), new Keyframe(1, 1));
	


	private float GetNormalizeRoundTime(float currentRoundTime)
	{
		return currentRoundTime / roundDuration;
	}

	public float GetGameFeelIntensity(float currentRoundTime)
	{
		return gameFeelIntensity.Evaluate(GetNormalizeRoundTime(currentRoundTime));
	}
}
