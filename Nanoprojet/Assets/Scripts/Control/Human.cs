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
        playerInput = GetComponent<PlayerInput>();
        playerInput.currentActionMap["Dash"].performed += ctx => fighter.DashButton();
        playerInput.currentActionMap["Attack"].performed += ctx => fighter.AttackButton();
        playerInput.currentActionMap["Move"].performed += ctx => lstickBuffer = ctx.ReadValue<Vector2>();
        playerInput.currentActionMap["Move"].canceled += ctx => lstickBuffer = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput != null) {

            if (!lstickBuffer.Equals(Vector2.zero))
            {
                fighter.Move(lstickBuffer);
            }
        }
    }
}
