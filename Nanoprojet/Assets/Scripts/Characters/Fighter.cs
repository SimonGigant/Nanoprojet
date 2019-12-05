using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterState {Idle, SetUpAttack, Block, Parry, Attack, AttackLag, Dash, Hit, Throw, Death};

public class Fighter : MonoBehaviour
{
    //Stats
    private float speed = 6f;
    private int maxHP = 3;
	

    //Values
    private int hp;
	[SerializeField][DisplayWithoutEdit]
    private FighterState state;
    private Vector2 currentDirection;
	private Physics physics;
    private bool moved = false;

    private Hitbox currentHitbox;
	private float lastDash;
    private Impact impact;
    private Animator animator;
	private FXManager fxManager;
	private Dash dash;

	//Counters
	private float counterInState;

    //Serialized fields
    [SerializeField]private Fighter opponent;
	[SerializeField]private float dashCooldown = 0.5f;
	[SerializeField]private float setUpAttackDuration = 0.2f;
	[SerializeField]private float blockDuration = 0.5f;
	[SerializeField]private float attackDuration = 0.2f;
	[SerializeField]private float attackLagDuration = 0.05f;
	//accessors
	public FighterState currentState => state;
	public Vector2 direction => currentDirection;
	public float stateCounter => counterInState;

    public void Initialize()
    {
        hp = maxHP;
        state = FighterState.Idle;
        impact.ResetImpact();
    }

    /**
     * Turn the fighter to look at the opponent
     */
    private void FaceOpponent()
    {
        Vector3 dir = opponent.transform.position - transform.position;
		dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        Quaternion rot = Quaternion.LookRotation(dir, Vector3.up);
		transform.rotation = rot;
    }

	private void Awake()
	{
		currentHitbox = GetComponentInChildren<Hitbox>();
		currentHitbox.opponent = opponent;
		physics = GetComponent<Physics>();
        impact = GetComponent<Impact>();
        animator = GetComponentInChildren<Animator>();
		fxManager = GetComponent<FXManager>();
		dash = GetComponent<Dash>();
    }

	void Start()
    {
        Initialize();
    }

    void Update()
    {
        if (!GameManager.Instance.PlayerCanInteract()) {
            return;
        }

        counterInState += Time.deltaTime;
        switch (state)
        {
            case FighterState.Idle:
                {
                    Idle();
                    break;
                }
            case FighterState.Dash:
                {
                    Dash();
                    break;
                }
            case FighterState.SetUpAttack:
                {
                    SetUpAttack();
                    break;
                }
            case FighterState.Block:
                {
                    Block();
                    break;
                }
            case FighterState.Attack:
                {
                    Attack();
                    break;
                }
            case FighterState.AttackLag:
                {
                    AttackLag();
                    break;
                }
        }

        if (moved)
        {
            moved = false;
        }
        else
        {
            animator.SetFloat("Speed", 0f);
        }
    }

    /**
     * Called to change the fighter state.
     * Initializes attributes depending on the state
     * @param nextState the state to which the fighter will go
     */
    private void ChangeState(FighterState nextState)
    {
        //Initialisations
        counterInState = 0f;
        switch (nextState)
        {
            case FighterState.Dash:
                {
					dash.InitDash();
                    TensionManager.Instance.AddTension(10f);
					fxManager.DashFx();
                    break;
                }
            case FighterState.SetUpAttack:
                {
                    animator.SetTrigger("Attack");
                    TensionManager.Instance.AddTension(15f);
                    break;
                }
            case FighterState.Attack:
                {
					currentHitbox.SetAttacking(true);
					break;
                }
            case FighterState.AttackLag:
                {
					currentHitbox.SetAttacking(false);
					break;
                }
            case FighterState.Death:
                {
                   // Destroy(gameObject);
                    break;
                }
        }
		if(nextState != state)
		{
			state = nextState;
            //GetComponent<Dash>().OnStateChange(nextState);
		}
       
    }
    

    //***************************************
    // Specific states methods called in Idle
    //***************************************

    private void Idle()
    {
        FaceOpponent();
    }

    private void Dash()
    {
		
    }

    private void SetUpAttack()
    {
        FaceOpponent();
        if(counterInState > setUpAttackDuration)
        {
            ChangeState(FighterState.Block);
        }
    }

    private void Block()
    {
        if(counterInState > blockDuration)
        {
            ChangeState(FighterState.Attack);
        }
    }

    private void Attack()
    {
        if(counterInState > attackDuration)
        {
            ChangeState(FighterState.AttackLag);
        }
    }
    
    private void AttackLag()
    {
        if(counterInState > attackLagDuration)
        {
            ChangeState(FighterState.Idle);
        }
    }

    //****************
    // Utility methods
    //****************

    private void Movement(Vector2 movement)
    {
        Vector3 dir = new Vector3(movement.x, 0f, movement.y);
		physics.AddForce(dir);
    }

    //************************************************************************
    // Public methods to control the character in Control class and derivatives
    //************************************************************************

    public bool Move(Vector2 dir)
    {
        if (!GameManager.Instance.PlayerCanInteract())
        {
            return false;
        }
        if (dir.magnitude.Equals(0f) || (state != FighterState.Idle && state != FighterState.Dash))
        {
            return false;
        }
        currentDirection = dir.normalized;
        if (state == FighterState.Dash)
        {
            return true;
        }
        //We ensure magnitude is <= 1f not to move faster than the character speed
        if (dir.magnitude > 1f)
        {
            dir.Normalize();
        }
        Vector2 movement = dir * speed;
        Movement(movement);
        animator.SetFloat("Speed",movement.magnitude);
        moved = true;
        return true;
    }

    public bool DashButton()
    {
        if (!GameManager.Instance.PlayerCanInteract())
        {
            return false;
        }
        if (state == FighterState.Idle && Time.time > lastDash + dashCooldown)
        {
            ChangeState(FighterState.Dash);
            return true;
        }
        return false;
    }

    public bool AttackButton()
    {
        if (!GameManager.Instance.PlayerCanInteract())
        {
            return false;
        }
        if (state == FighterState.Idle)
        {
            ChangeState(FighterState.SetUpAttack);
            return true;
        }
        return false;
    }



    //*************************************
    // Public methods for other scripts
    // (Do not call them in the controller)
    //*************************************

    public void Damage(int amount)
    {
        if (!GameManager.Instance.PlayerCanInteract())
        {
            return;
        }
        if (state == FighterState.Block)
        {
            Debug.Log("Parade !");
            TensionManager.Instance.AddTension(35f);
            ChangeState(FighterState.Idle);
            ImpulseOppositToOpponent(7f);
            opponent.ImpulseOppositToOpponent(7f);
			fxManager.ParryFX();
            return;
        }

		fxManager.HitFX();
        hp -= amount;
        if (hp <= 0)
        {
            ChangeState(FighterState.Death);
            animator.SetTrigger("Death");
        }
        else
        {
            animator.SetTrigger("Hit");
        }
        ImpulseOppositToOpponent(15f);
        Gamefeel.Instance.InitScreenshake(0.2f, 0.2f);
    }

    public bool IsDead()
    {
        return state == FighterState.Death;
    }

    public void ImpulseOppositToOpponent(float force)
    {
        Vector3 impulseDir = (transform.position - opponent.transform.position).normalized;
        impulseDir.y = 0f;
        impact.AddImpact(impulseDir, force);
    }

    public void SucceedAttack()
    {
        if (!GameManager.Instance.PlayerCanInteract())
        {
            return;
        }
        if (state == FighterState.Attack)
        {
            GameManager.Instance.TryWin();
			currentHitbox.SetAttacking(false);
            ImpulseOppositToOpponent(4f);
            TensionManager.Instance.AddTension(50f);
        }
    }

    public void SetOpponent(Fighter op)
    {
        opponent = op;
    }

	public void DashEnd()
	{
		if(state == FighterState.Dash)
		{
			lastDash = Time.time;
			ChangeState(FighterState.Idle);
		}
	}
}
