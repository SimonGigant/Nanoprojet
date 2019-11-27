using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterState {Idle, SetUpAttack, Block, Parry, Attack, AttackLag, Dash, Hit, Throw, Death};

public class Fighter : MonoBehaviour
{
    //Stats
    private float speed = 6f;
    private int maxHP = 3;
    
    //private float dashSpeed = 12f;
   // private float dashDuration = 0.2f;
    
	

    //Values
    private int hp;
	[SerializeField][DisplayWithoutEdit]
    private FighterState state;
    private Vector2 currentDirection;
	//private Rigidbody rigidbody;
	private CharacterController charController;

    private Hitbox currentHitbox;
	private float lastDash;

	//Counters
	private float counterInState;

    //Serialized fields
    [SerializeField] private Fighter opponent;
	[SerializeField] private float dashCooldown = 2;
	[SerializeField]private float setUpAttackDuration = 0.2f;
	[SerializeField]private float blockDuration = 0.5f;
	[SerializeField]private float attackDuration = 0.2f;
	[SerializeField]private float attackLagDuration = 0.05f;
	//accessors
	public FighterState currentState => state;
	public Vector2 direction => currentDirection;

	//local events
	public delegate void StateChange(FighterState newState);
	public event StateChange OnStateChange;

    private void Initialize()
    {
        hp = maxHP;
        state = FighterState.Idle;
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
        //rigidbody.MoveRotation(rot);
    }

	private void Awake()
	{
		currentHitbox = GetComponentInChildren<Hitbox>();
		currentHitbox.opponent = opponent;
		charController = GetComponent<CharacterController>();
	}

	void Start()
    {
        //rigidbody = GetComponent<Rigidbody>();
        Initialize();
    }
    
    void Update()
    {
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

                    break;
                }
            case FighterState.Attack:
                {
					currentHitbox.SetAttacking(true);
					//currentHitbox = GameObject.Instantiate(hitboxPrefab, transform.position, transform.rotation, transform);
					//currentHitbox.GetComponentInChildren<Hitbox>().opponent = opponent;
					break;
                }
            case FighterState.AttackLag:
                {
					//Destroy(currentHitbox);
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
			OnStateChange?.Invoke(state);
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
<<<<<<< HEAD
        Movement(currentDirection * dashSpeed);
=======
       /*counterInState += Time.deltaTime;
       Movement(currentDirection * dashSpeed);
>>>>>>> master
        if (counterInState > dashDuration)
        {
            ChangeState(FighterState.Idle);
        }*/
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
        Vector3 dir = new Vector3(movement.x, 0f, movement.y) * Time.deltaTime;
		dir -= Vector3.up * 4 * Time.deltaTime; //add gravity to force the player to stay on the ground
		charController.Move(dir );
        //rigidbody.MovePosition(transform.position + dir);
    }

    //************************************************************************
    // Public methods to control the character in Control class and derivatives
    //************************************************************************

    public bool Move(Vector2 dir)
    {
        if(dir.magnitude.Equals(0f) || (state != FighterState.Idle && state != FighterState.Dash))
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
        return true;
    }

    public bool DashButton()
    {
        if (state == FighterState.Idle && Time.time > lastDash + dashCooldown)
        {
            ChangeState(FighterState.Dash);
            return true;
        }
        return false;
    }

    public bool AttackButton()
    {
        if(state == FighterState.Idle)
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
        if(state == FighterState.Block)
        {
            Debug.Log("Parade !");
            ChangeState(FighterState.Idle);
            ImpulseOppositToOpponent(7f);
            opponent.ImpulseOppositToOpponent(7f);
            return;
        }
        hp -= amount;
        if (hp <= 0)
        {
            ChangeState(FighterState.Death);
        }
        ImpulseOppositToOpponent(15f);
        Gamefeel.Instance.InitScreenshake(0.2f, 0.2f);
    }

    public void ImpulseOppositToOpponent(float force)
    {
        Vector3 impulseDir = (transform.position - opponent.transform.position).normalized;
        //rigidbody.AddForce(impulseDir * force, ForceMode.Impulse);
    }

    public void SucceedAttack()
    {
        if(state == FighterState.Attack)
        {
			currentHitbox.SetAttacking(false);
            ImpulseOppositToOpponent(4f);
        }
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
