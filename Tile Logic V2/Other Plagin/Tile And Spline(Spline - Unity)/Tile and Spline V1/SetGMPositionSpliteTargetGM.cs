using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SetGMPositionSpliteTargetGM : MonoBehaviour
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

   /// <summary>
   /// Нужен, что бы зафиксировать игрока по одной из позиции
   /// </summary>
   [SerializeField] 
   private AbsGetPositionBoolXYZ _positionGM;
   
   private Spline currentSpline;

   private bool _isStart = false;
    
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
      _isInit = true;
      OnInit?.Invoke();
      
   }

   public void StartLogic()
   {
      GetNextSplineLogic();
      _isStart = true;
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
      if (_isStart == true)
      {
         Vector3 vector3 = _targetGM.transform.position;
         
         if (_positionGM.ReturnX() == true)
         {
            vector3.x = _positionGM.GetPosition().x;
         }
         
         if (_positionGM.ReturnY() == true)
         {
            vector3.y = _positionGM.GetPosition().y;
         }
         
         if (_positionGM.ReturnZ() == true)
         {
            vector3.z = _positionGM.GetPosition().z;
         }

         _targetGM.transform.position = vector3; 
      
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
         
         
         // if (ComparePrecisionFractionalValue(1 % t, 1f, 3) == true)
         // {
         //    Debug.Log("Get Next ");
         //    GetNextSplineLogic();
         // }


        
         var targetPosF3= currentSpline.EvaluatePosition(1f);
         Vector3 targetPos = new Vector3(targetPosF3.x, targetPosF3.y, targetPosF3.z);
         targetPos += _splineContainer.transform.position;

         if (CheckPosition(_targetGM.transform.position, targetPos, 3) == true) 
         {
            GetNextSplineLogic();
         }
      }
   }


   private bool CheckPosition(Vector3 currentPos,Vector3 targetPos,int precision)
   {
      if (ComparePrecisionFractionalValue(currentPos.x, targetPos.x, precision) == true) 
      {
         if (ComparePrecisionFractionalValue(currentPos.y, targetPos.y, precision) == true) 
         {
            if (ComparePrecisionFractionalValue(currentPos.z, targetPos.z, precision) == true)
            {
               return true;
            }
         }
      }

      return false;
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
