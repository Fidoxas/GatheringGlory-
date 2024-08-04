using System;
using System.Collections;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.Rendering.VirtualTexturing;

public class LobbyCounterToStart : MonoBehaviour
{
    private TextMeshProUGUI _tmp;

    private void Awake()
    {
        var lM =FindObjectOfType<LobbyManager>();
        lM.StartGameFor += HandleGameStartFor;
        _tmp = GetComponent<TextMeshProUGUI>();
    }
    
    private void HandleGameStartFor(string number)
    {
        if (!string.IsNullOrEmpty(number))
        {
            ReloadTmp(number);
        }
        else
        {
            _tmp.text = string.Empty;
        }
    }

    private void ReloadTmp(string number)
    {
        _tmp.text = number;
    }
}