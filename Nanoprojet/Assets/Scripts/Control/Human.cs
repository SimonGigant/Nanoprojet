using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Human : Control
{
    [SerializeField] private Fighter fighter;
    [SerializeField] private int controller;
    private PlayerInput playerInput;

    //Buffers:
    private Vector2 lstickBuffer;
    private bool dashBuffer;
    private bool attackBuffer;

    private void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        /*
        if (controller == 0)
        {
            if (Input.GetButtonDown("Fire1"))
            {
                fighter.DashButton();
            }
            if (Input.GetButtonDown("Fire3"))
            {
                fighter.AttackButton();
            }
            if (Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f)
            {
                fighter.Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
            }
        }else if(controller == 1){
            if (Input.GetButtonDown("2Fire1"))
            {
                fighter.DashButton();
            }
            if (Input.GetButtonDown("2Fire3"))
            {
                fighter.AttackButton();
            }
            if (Input.GetAxis("2Horizontal") != 0f || Input.GetAxis("2Vertical") != 0f)
            {
                fighter.Move(new Vector2(Input.GetAxis("2Horizontal"), Input.GetAxis("2Vertical")));
            }
        }*/

        playerInput.currentActionMap["Move"].performed += ctx => lstickBuffer = ctx.ReadValue<Vector2>();
        playerInput.currentActionMap["Dash"].performed += ctx => dashBuffer = ctx.ReadValue<bool>();
        playerInput.currentActionMap["Attack"].performed += ctx => attackBuffer = ctx.ReadValue<bool>();
        if (dashBuffer)
        {
            fighter.DashButton();
        }
        if (attackBuffer)
        {
            fighter.AttackButton();
        }
        if (!lstickBuffer.Equals(Vector2.zero))
        {
            fighter.Move(lstickBuffer);
        }
    }
}
