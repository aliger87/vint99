using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyBackground : MonoBehaviour
{
    public float Multiplay = 2;

    private Vector3 StartScale = Vector3.one;

    private void Awake()
    {
        StartScale = transform.localScale;
        UpdateScale();
    }

#if DEBUG
    private void Update()
    {
        UpdateScale();
    }
#endif

    public void UpdateScale()
    {
        float screenAspect = (float)Screen.width / Screen.height;
        float targetAspect = Camera.main.aspect;

        float scaleHeight = screenAspect / targetAspect;

        if (scaleHeight < 1.0f)
        {
            Vector3 scale = new Vector3(StartScale.x, StartScale.y * scaleHeight, StartScale.z);
            transform.localScale = scale * Multiplay;
        }
        else
        {
            Vector3 scale = new Vector3(StartScale.x / scaleHeight, StartScale.y, StartScale.z);
            transform.localScale = scale * Multiplay;
        }
    }
}
