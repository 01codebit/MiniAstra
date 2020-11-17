using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
public class Container3DView : MonoBehaviour
{
    private Transform _transform;
    public Transform GetTransform() => _transform;

    [Inject]
    public void Inject(Transform transform3D)
    {
        _transform = transform3D;
    }
}
