using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using System;
using UniRx;
public class Container3DFacade : MonoBehaviour
{
    private Container3DCoordinator _coordinator;

    [Inject]
    private void Inject(Container3DCoordinator coordinator){
        _coordinator = coordinator;
    }
}
