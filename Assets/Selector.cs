using System;
using UnityEngine;

[System.Serializable]
public class UnitTag
{
    public string Name;  // Specify type for Name
    public Color Color;
}

public class Selector : MonoBehaviour
{
    public float YOffset = 0;
    public float MinScale = 0;
    public float ScaleMultiplier = 0; // Fixed typo here
    public GameObject prefab;
    public LayerMask ground;
    
    protected virtual void SetInstance(GameObject go) { }
    protected virtual void SetColor(Color color) { }
    protected virtual void SetScale(float scale) { }
    protected virtual void SetPosition(Collider collider) { }
    protected virtual void SetEnabled(bool on) { }

    protected float GetScale(float scale)
    {
        return Mathf.Max(scale, MinScale);
    }

    private void Awake()
    {
        GameObject instance = Instantiate(prefab);
        SetInstance(instance);
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ground))
            {
                
                SetPosition(hit.collider);
                SetColor(Color.red); 
                SetScale(GetScale(1.0f)); 
                SetEnabled(true); 
            }
        }
    }
}