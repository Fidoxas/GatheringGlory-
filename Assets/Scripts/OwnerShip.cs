using System;
using System.Collections;
using System.Collections.Generic;
using ScriptablesOBJ.Stages;
using UnityEngine;

public class OwnerShip : MonoBehaviour
{
  public Player player;
  public ObjSelector selector;

  private void Awake()
  {
    PlayerSelection playerSelection = FindObjectOfType<PlayerSelection>();
    if (playerSelection != null)
    {
      playerSelection.OnPlayerAssigned += HandlePlayerAssigned;
    }
  }

  private void HandlePlayerAssigned(int playernumber)
  {
      if (selector == null)
      {
        selector = GetComponent<ObjSelector>();
      }
  }

  

  public void AssignOwnerShip(Player player)
  {
    this.player = player;
    selector.player = this.player;
    selector.UpdateMaterial();
  }
}
