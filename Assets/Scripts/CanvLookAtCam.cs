using System.Collections;
using UnityEngine;

public class CanvLookAtCam : MonoBehaviour
{
    private RectTransform _transform;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
        _transform = GetComponent<RectTransform>();
        // StartCoroutine(FollowCam());
    }

    private IEnumerator FollowCam()
    {
        while (gameObject.activeSelf)
        {
            _transform.LookAt(_transform.position + cam.transform.rotation * Vector3.forward, cam.transform.rotation * Vector3.up);
            yield return null;
        }
    }
}