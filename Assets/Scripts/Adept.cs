using System;
using ScriptablesOBJ.Stages;
using UnityEngine;

public class Adept : MonoBehaviour
{
    public event Action<Player, Transform> CameraBond;
    private Player _owner;

    private void Start()
    {
        _owner = GetComponent<OwnerShip>().player;
    }

    public Player GetOwner()
    {
        return _owner;
    }

    public void CameraAssign()
    {
        CameraBond?.Invoke(_owner, this.transform);
    }
}