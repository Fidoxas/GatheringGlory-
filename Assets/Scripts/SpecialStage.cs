using System.Collections.Generic;
using ScriptablesOBJ.Stages;
using UnityEngine;

public class SpecialStage : Stage
{
    // Dodaj dodatkowe właściwości i metody specyficzne dla specjalnego etapu
    public void SpecialMechanic()
    {
        Debug.Log("This is a special mechanic for the special stage.");
    }

    public SpecialStage(Material material, GameObject gameObject,List<GameObject> tilles) 
        : base("", material, PlayerSo.Numbers.None, Type.Special, gameObject ,null,null)
    {
        
    }
}