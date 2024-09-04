using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonkeyPoint : MonoBehaviour
{
    public Transform Point;
    public float Max = 3.5f;
    public LayerMask Layers;

    public float LerpSpeed = 0;

    private void Update()
    {
        if (LerpSpeed == 0)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.forward, out hit, Max, Layers))
                Point.localPosition = -Vector3.forward * Vector3.Distance(hit.point, transform.position);
            else
                Point.localPosition = -Vector3.forward * Max;
        }
        else
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, -transform.forward, out hit, Max, Layers))
                Point.localPosition = Vector3.Lerp(Point.localPosition, -Vector3.forward * Vector3.Distance(hit.point, transform.position), LerpSpeed * Time.deltaTime);
            else
                Point.localPosition = Vector3.Lerp(Point.localPosition, -Vector3.forward * Max, LerpSpeed * Time.deltaTime);
        }
    }
}
