using System.Collections.Generic;
using ScriptablesOBJ.Stages;
using UnityEngine;

namespace ScriptablesOBJ
{
   [CreateAssetMenu(menuName = "ScriptableObjects/PlayersDB", order = 1)]
   public class PlayersDB : ScriptableObject
   {
      public PlayerDB[] playerDbs;
   }
}
