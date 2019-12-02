using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    public Controls controls;
    private PlayerInput playerInput;
    private Vector2 inputSave;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();

        playerInput.currentActionMap["Move"].performed += ctx => inputSave = ctx.ReadValue<Vector2>();
        playerInput.currentActionMap["Move"].canceled += ctx => inputSave = Vector2.zero;


        /*controls.Player.Move.performed += ctx => inputSave = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => inputSave = Vector2.zero;*/
    }


    public void Update()
    {
        Vector3 m = new Vector3(inputSave.x, 0, inputSave.y) * Time.deltaTime;

        transform.Translate(m, Space.World);
    }
}
