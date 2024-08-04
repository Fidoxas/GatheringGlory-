using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitsButtonMaker : MonoBehaviour
{
   public Player _player;
   public ClientManager _clientMan;
   private ResourceSO _resourceSo;


   private void Start()
   {
      if (_clientMan == null)
      {
         _clientMan = FindObjectOfType<ClientManager>();
      }

      _player = _clientMan.MyPlayer;
   }

   public void TryToCreateUnit()
   {
      _player.CreateT1Unit();
   }
}
