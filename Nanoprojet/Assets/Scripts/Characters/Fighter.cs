using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterState {Idle, SetUpAttack, Block, Parry, Attack, AttackLag, Dash, Hit, Throw, Death};

public class Fighter : MonoBehaviour
{
    //Stats
    private float speed = 0.1f;
    private int maxHP = 3;
    
    private float dashSpeed = 0.2f;
    private float dashDuration = 0.2f;
    private float setUpAttackDuration = 0.2f;
    private float blockDuration = 0.5f;
    private float attackDuration = 0.2f;
    private float attackLagDuration = 0.05f;

    //Values
    private int hp;
    private FighterState state;
    private Vector2 currentDirection;

    private Hitbox currentHitbox;

    //Counters
    private float counterInState;

    //Serialized fields
    [SerializeField] private Fighter opponent;
    //[SerializeField] private GameObject hitboxPrefab;

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
        transform.LookAt(opponent.transform);
    }

	private void Awake()
	{
		currentHitbox = GetComponentInChildren<Hitbox>();
		currentHitbox.opponent = opponent;
	}

	void Start()
    {
        Initialize();
    }
    
    void Update()
    {
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
                    Destroy(gameObject);
                    break;
                }
        }
        state = nextState;
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
        counterInState += Time.deltaTime;
        Movement(currentDirection * dashSpeed);
        if (counterInState > dashDuration)
        {
            ChangeState(FighterState.Idle);
        }
    }

    private void SetUpAttack()
    {
        counterInState += Time.deltaTime;
        FaceOpponent();
        if(counterInState > setUpAttackDuration)
        {
            ChangeState(FighterState.Block);
        }
    }

    private void Block()
    {
        counterInState += Time.deltaTime;
        if(counterInState > blockDuration)
        {
            ChangeState(FighterState.Attack);
        }
    }

    private void Attack()
    {
        counterInState += Time.deltaTime;
        if(counterInState > attackDuration)
        {
            ChangeState(FighterState.AttackLag);
        }
    }
    
    private void AttackLag()
    {
        counterInState += Time.deltaTime;
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
        transform.position += new Vector3(movement.x, 0f, movement.y);
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
        if (state == FighterState.Idle)
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
            return;
        }
        hp -= amount;
        if (hp <= 0)
        {
            ChangeState(FighterState.Death);
        }
    }

    public void SucceedAttack()
    {
        if(state == FighterState.Attack)
        {
			//Destroy(currentHitbox.gameObject);
			//currentHitbox = null;
			currentHitbox.SetAttacking(false);
        }
    }
}
