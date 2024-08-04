using System;
using System.Collections;
using System.Collections.Generic;
using ScriptablesOBJ.Stages;
using Sirenix.OdinInspector;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    private Player owner;
    [SerializeField] private ResourceSO res;
    [SerializeField] private float workNeeded = 5f;

    // Field to keep track of the running coroutine
    private Coroutine extractionCoroutine;

    private void Start()
    {
        owner = GetComponent<OwnerShip>().player;
        StartExtractor();
    }

    private IEnumerator Extractor()
    {
        while (owner != null)
        {
            Produce();
            yield return new WaitForSeconds(workNeeded);
        }
    }

    [Button("Produce", ButtonSizes.Medium)]
    private void Produce()
    {
        if (owner == null)
        {
            owner = GetComponent<OwnerShip>().player;
        }
        if (owner != null)
        {
            if (owner.resources.ContainsKey(res))
            {
                owner.resources[res]++;
            }
            else
            {
                owner.resources.Add(res, 1);
            }
            // Debug.Log("Added 1 " + res.type.ToString() + ". Actual Value is: " + owner.resources[res]);
            owner.ReloadValue(res);
        }
        else
        {
            Debug.LogWarning("No owner assigned to produce resources.");
        }
    }

    public void Restart()
    {
        if (extractionCoroutine != null)
        {
            StopCoroutine(extractionCoroutine);
        }

        owner = GetComponent<OwnerShip>().player;

        StartExtractor();
    }

    private void StartExtractor()
    {
        extractionCoroutine = StartCoroutine(Extractor());
    }
}