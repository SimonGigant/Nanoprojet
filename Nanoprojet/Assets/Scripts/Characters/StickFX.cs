using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.VFX;

[RequireComponent(typeof(Fighter))]
public class StickFX : MonoBehaviour
{
	[SerializeField]
	private VisualEffect vfx;
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

		public void Apply(VisualEffect fx)
		{
			fx.SetFloat("Intensity", intensity);
			fx.SetGradient("MainGradient", gradient);
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
				setUpAttackState.Apply(vfx);
				break;
			case FighterState.Block:
				blockState.Apply(vfx);
				break;
			case FighterState.Attack:
				attackState.Apply(vfx);
				break;
			case FighterState.AttackLag:
				attackLagState.Apply(vfx);
				break;
			default:
				defaultState.Apply(vfx);
				vfx.SendEvent("OnStop");
				break;
		}
	}
}
