using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace ScriptablesOBJ.Stages
{
    [CreateAssetMenu(menuName = "ScriptableObjects/Player", order = 1)]
    public class PlayerSo : ScriptableObject
    {
        public string Pname { get; private set; }
        public Numbers pNum;
        public CastleSO castleSo;
        public UnitSo unitT1;

        public enum Numbers
        {
            None = -1,
            P1,
            P2,
            P3,
            P4,
            P5,
            P6,
            P7,
            P8
        }
       
        public static PlayerSo CheckCurrentPByStage(Stage.Type currentStage, PlayersDB playersDB)
        {
            PlayerSo currentPdb = null;
            foreach (var i in playersDB.playerDbs)
            {
                if (i.castleSo.startStage == currentStage)
                    currentPdb = i;
            }

            return currentPdb;

        }
    }
}
