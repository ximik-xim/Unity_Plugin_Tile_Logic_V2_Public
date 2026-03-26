using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Набросок упровления скоростью движения таилов
/// !!! ПОЛНОСТЬЮ САМОСТОЯТЕЛЬНЫЙ И НИ КАК НЕ СВЯЗАН С ЛОГИКОЙ УПРАВЛЕНИЯ ТАИЛАМИ !!!
///(просто для примера) 
/// </summary>
public class ExampleControllerSpeedTileRB : MonoBehaviour
{
    [SerializeField] 
    private Vector3 _currentValue = new Vector3(1f, 1f, 1f);

    [SerializeField] 
    private Vector3 _bufferValue = Vector3.zero;

    [SerializeField] 
    private Vector3 _maxSpeed = new Vector3(5f, 5f, 5f);
    
    [SerializeField] 
    private Vector3 _addValue = new Vector3(0.05f, 0.05f, 0.05f);

    private IEnumerator _cor;
    private bool _cont = false;

    [SerializeField]
    private Rigidbody _rbTile;

    private void Awake()
    {
        StartGo();
    }


    public void StopGo()
    {
        StopCoroutine(_cor);
        
        _bufferValue = _currentValue;
        _currentValue = Vector3.zero;
        
        _rbTile.velocity = _currentValue;
    }

    public void ContinueGo()
    {
        _currentValue = _bufferValue;

        AddSpeed();
    }

    public void StartGo()
    {
        AddSpeed();
    }
    
    private void AddSpeed()
    {
        _cont = true;
        _cor = UpSpeedCor();
        StartCoroutine(_cor);
    }

    

    private IEnumerator UpSpeedCor()
    {
        if (_currentValue.x < _maxSpeed.x)
        {
            _currentValue.x += _addValue.x;
        }
        
        if (_currentValue.y < _maxSpeed.y)
        {
            _currentValue.y += _addValue.y;
        }
        
        if (_currentValue.z < _maxSpeed.z)
        {
            _currentValue.z += _addValue.z;
        }
        
        _bufferValue = _currentValue;
        _rbTile.velocity = _currentValue;
        
        if (_currentValue.x > _maxSpeed.x && _currentValue.y > _maxSpeed.y && _currentValue.z > _maxSpeed.z)
        {
            _cont = false;
        }
        
        yield return new WaitForSeconds(1);

        if (_cont == true)
        {
            _cor = UpSpeedCor();
            StartCoroutine(_cor);
        }
    }

    private void OnDisable()
    {
        _cont = false;
        StopCoroutine(_cor);
    }

    private void OnDestroy()
    {
        _cont = false;
        StopCoroutine(_cor);
    }
}
