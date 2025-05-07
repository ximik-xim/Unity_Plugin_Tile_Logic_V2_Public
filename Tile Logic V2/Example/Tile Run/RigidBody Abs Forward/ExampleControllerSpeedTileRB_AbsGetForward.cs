using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Набросок упровления скоростью движения таилов
/// !!! ПОЛНОСТЬЮ САМОСТОЯТЕЛЬНЫЙ И НИ КАК НЕ СВЯЗАН С ЛОГИКОЙ УПРАВЛЕНИЯ ТАИЛАМИ !!!
///(просто для примера) 
/// </summary>
public class ExampleControllerSpeedTileRB_AbsGetForward : MonoBehaviour
{
    [SerializeField] 
    private AbsGetForwardRunTile _forwardTileRun;
    
    [SerializeField]
       private float _currentValue = 1f;

       [SerializeField] 
       private float _bufferValue = 0f;

       [SerializeField] 
       private float _maxSpeed = 5f;

       [SerializeField]
       private float _addValue = 0.05f;
   
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
           _currentValue = 0;

           _rbTile.velocity = _currentValue * _forwardTileRun.GetForward();
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
       
           
           _bufferValue = _currentValue;
           _rbTile.velocity = _currentValue * _forwardTileRun.GetForward();

           if (_currentValue < _maxSpeed)
           {
               _currentValue+= _addValue;
           }
           else
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
