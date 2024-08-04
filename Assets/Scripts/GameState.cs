using System;
using System.Collections.Generic;
using UnityEngine;

public class GameState : MonoBehaviour
{
    [SerializeField] private List<PlayerState> playerStates;

    public GameState Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }
}