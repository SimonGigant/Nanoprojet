using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoin : MonoBehaviour
{
    public List<GameObject> prefabs;
    public PlayerInputManager playerInput;
    
    void Start()
    {
        playerInput = GetComponent<PlayerInputManager>();
        playerInput.playerPrefab = prefabs[0];
    }
    
    // Update is called once per frame
    void Update()
    {
        if(playerInput.playerCount == 1)
        {
            playerInput.playerPrefab = prefabs[1];
        }
    }
}
