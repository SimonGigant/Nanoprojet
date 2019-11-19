using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Control
{
    [SerializeField] private Fighter fighter;
    [SerializeField] private int controller;
    // Update is called once per frame
    void Update()
    {
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
        }
    }
}
