using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Прикрепляет заданный обьект к ломанной линии(Spline) получаемой из контейнера с ломанными линиями
/// Так же сразу поварачивает обьект в напровлении движеня
/// </summary>
public class SetGMPositionSplite : MonoBehaviour
{
     public bool IsInit => _isInit;
   private bool _isInit = false;
   public event Action OnInit;
   
   [SerializeField] 
   private AbsGetNextSpline _absGetNextSpline;
   
   /// <summary>
   ///нужен для координат обьекта
   ///и вычесления смещени 
   /// </summary>
   [SerializeField] 
   private GameObject _splineContainer;
   
   /// <summary>
   /// Обьект, который будет прикреплен к ломанной линии(Spline)
   /// </summary>
   [SerializeField] 
   private GameObject _targetGM;

   private Spline currentSpline;
    
   private void Awake()
   {
      if (_absGetNextSpline.IsInit == false)
      {
         _absGetNextSpline.OnInit += OnInitData;
         return;
      }

      Init();
   }

   private void OnInitData()
   {
      _absGetNextSpline.OnInit -= OnInitData;
      Init();
   }

   private void Init()
   {
      GetNextSplineLogic();
      
      _isInit = true;
      OnInit?.Invoke();
   }

   private void GetNextSplineLogic()
   {
      currentSpline = _absGetNextSpline.GetNextSpline();
      //устанавливаю начальную позицию в 0 знач пути у сплита(крч в начало пути ставлю обьект)
      _targetGM.transform.position = currentSpline.EvaluatePosition(0);
      _targetGM.transform.position += _splineContainer.transform.position;  
      
   }
 
   
   private void FixedUpdate()
   {
      var native = new NativeSpline(currentSpline);
      float distance = SplineUtility.GetNearestPoint(native, _targetGM.transform.position - _splineContainer.transform.position, out float3 nearest,out float t);
      
      _targetGM.transform.position = nearest;
      _targetGM.transform.position += _splineContainer.transform.position;
           
      Vector3 forward = Vector3.Normalize(native.EvaluateTangent(t));
      Vector3 up = native.EvaluateUpVector(t);
           
      var remappedForward = new Vector3(0,0,1);
      var remappedUp = new Vector3(0,1,0);
      var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward, remappedUp));
           
      transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;
   
      Vector3 engineForward = transform.forward;

      if (ComparePrecisionFractionalValue(1 % t, 1f, 3) == true)  
      {
         GetNextSplineLogic();
      }
   }
   
   
   public static bool ComparePrecisionFractionalValue(float a,float b, int precision)
   {
      float value = Mathf.Abs(a - b);
      float DecimalPrecision = Mathf.Pow(10, -precision);

      if (value < DecimalPrecision) 
      {
         return true;
      }

      return false;
   }
}
