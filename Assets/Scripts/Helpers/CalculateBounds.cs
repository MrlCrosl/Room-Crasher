using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsHelper
{
    public static Bounds Calculate(Transform transform, Renderer[] subRenderers)
    {
        Quaternion currentRotation = transform.rotation;
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        Bounds bounds = new Bounds(transform.position, Vector3.zero);

        foreach (Renderer renderer in subRenderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        Vector3 localCenter = bounds.center - transform.position;
        bounds.center = localCenter;

        transform.rotation = currentRotation;

        return bounds;
    }
}