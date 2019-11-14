using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FighterState {Idle, SetUpAttack, Block, Parry, Attack, AttackLag, Dash, Hit, Throw, Death};

public class Fighter : MonoBehaviour
{
    //Stats
    private float speed = 0.2f;
    private int maxHP = 3;
    
    private float dashSpeed = 0.8f;
    private float dashDuration = 0.2f;
    private float setUpAttackDuration;
    private float blockDuration;
    private float attackDuration;
    private float attackLagDuration;

    //Values
    private int hp;
    private FighterState state;
    private Vector2 currentDirection;

    private GameObject currentHitbox;

    //Counters
    private float counterInState;

    [SerializeField] private Fighter opponent;
    [SerializeField] private GameObject hitboxPrefab;

    private void Initialize()
    {
        hp = maxHP;
        state = FighterState.Idle;
    }

    private void FaceOpponent()
    {
        transform.LookAt(opponent.transform, transform.forward);
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
        }
    }

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
                    currentHitbox = GameObject.Instantiate(hitboxPrefab, transform.position, transform.rotation, transform);
                    currentHitbox.GetComponent<Hitbox>().opponent = opponent;
                    break;
                }
        }
        state = nextState;
    }

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

    private void Movement(Vector2 movement)
    {
        transform.position += new Vector3(movement.x, 0f, movement.y);
    }

    //************************************************************************
    //Public methods to control the character in Control class and derivatives
    //************************************************************************

    public bool Move(Vector2 dir)
    {
        if(dir.magnitude.Equals(0f))
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

    public void Damage(int amount)
    {
        hp -= amount;
        if (hp <= 0)
        {
            ChangeState(FighterState.Death);
        }
    }
}
