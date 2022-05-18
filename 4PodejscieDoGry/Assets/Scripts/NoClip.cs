using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoClip : MonoBehaviour
{
    [SerializeField] private float distance;
    [SerializeField] private float radius;

    [SerializeField] private LayerMask clippingLayerMask;

    [SerializeField] private AnimationCurve curve;

    private Vector3 originalLocPos;

    private void Start() => originalLocPos = transform.localPosition;

    private void Update()
    {
        if (Physics.SphereCast(Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f)), radius, out var hit, distance, clippingLayerMask))
        {
            transform.localPosition = originalLocPos - new Vector3(0.0f, 0.0f, curve.Evaluate(hit.distance / distance));
        }
        else
        {
            transform.localPosition = originalLocPos;
        }
    }

}
