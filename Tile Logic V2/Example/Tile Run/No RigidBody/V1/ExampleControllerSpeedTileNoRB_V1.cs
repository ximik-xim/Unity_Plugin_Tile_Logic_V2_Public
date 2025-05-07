using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Набросок упровления скоростью движения таилов
/// !!! ПОЛНОСТЬЮ САМОСТОЯТЕЛЬНЫЙ И НИ КАК НЕ СВЯЗАН С ЛОГИКОЙ УПРАВЛЕНИЯ ТАИЛАМИ !!!
///(просто для примера) 
/// </summary>
public class ExampleControllerSpeedTileNoRB_V1 : MonoBehaviour
{
    [SerializeField]
    private float _speed;

    [SerializeField] 
    private AbsGetForwardRunTile _forwardTileRun;

    [SerializeField] 
    private GameObject _target;
    
    private void FixedUpdate()
    {
        _target.transform.position += _speed * Time.fixedDeltaTime * _forwardTileRun.GetForward();
    }
}
