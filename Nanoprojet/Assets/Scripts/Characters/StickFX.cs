using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

[RequireComponent(typeof(Fighter))]
public class StickFX : MonoBehaviour
{
	[SerializeField]
	private VisualEffect vfx;
	[SerializeField]
	private Light fxLight;

	private Fighter fighter;

	
	public FXState defaultState;
	public FXState setUpAttackState;
	public FXState blockState;
	public FXState attackState;
	public FXState attackLagState;

	private FighterState lastState;

	[System.Serializable]
	public class FXState
	{
		[GradientUsage(true)]
		public Gradient gradient;
		public float intensity;
		public Color lightColor;
		public float lightIntensity;

		public void Apply(VisualEffect fx, Light l)
		{
			fx.SetFloat("Intensity", intensity);
			fx.SetGradient("MainGradient", gradient);
			l.color = lightColor;
			l.intensity = lightIntensity;
		}
	}


	private void Awake()
	{
		fighter = GetComponent<Fighter>();
	}

	private void Update()
	{
		UpdateFX();
	}



	private void UpdateFX()
	{
		if(fighter.currentState == lastState)
		{
			return;
		}
		lastState = fighter.currentState;

		switch (fighter.currentState)
		{
			case FighterState.SetUpAttack:
				vfx.SendEvent("OnPlay");
				fxLight.enabled = true;
				setUpAttackState.Apply(vfx, fxLight);
				break;
			case FighterState.Block:
				blockState.Apply(vfx, fxLight);
				break;
			case FighterState.Attack:
				attackState.Apply(vfx, fxLight);
				break;
			case FighterState.AttackLag:
				attackLagState.Apply(vfx, fxLight);
				break;
			default:
				defaultState.Apply(vfx, fxLight);
				vfx.SendEvent("OnStop");
				fxLight.enabled = false;
				break;
		}
	}
}
