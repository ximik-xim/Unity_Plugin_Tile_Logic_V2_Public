using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

public class SetGMPositionSpliteTargetGM : MonoBehaviour
{
    public bool IsInit => _isInit;
   private bool _isInit = false;
   public event Action OnInit;
   
   /// <summary>
   ///нужен для координат обьекта
   ///и вычесления смещени 
   /// </summary>
   [SerializeField] 
   private SplineContainer _splineContainer;
   
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

   /// <summary>
   /// Устанавливать ли направление обьекту вдоль линии движения
   /// </summary>
   [SerializeField]
   private bool _setForwardSpline = false;
   
   [SerializeField] 
   private Vector3 _startOffset;
   
   private bool _isStart = false;
    
   private void Awake()
   {
      Init();
   }
   
   private void Init()
   {
      _isInit = true;
      OnInit?.Invoke();
   }

   public void StartLogic()
   {
      _targetGM.transform.position = _splineContainer.Splines[0].EvaluatePosition(0);
      _targetGM.transform.position += _splineContainer.transform.position + _startOffset;
      
      _isStart = true;
   }
   
   private void FixedUpdate()
   {
      if (_isStart == true)
      { 
         //Ищем след. точку, к которой будем двигаться(за 1 точку берем тек. линию)
        SplineUtility.GetNearestPoint(_splineContainer.Splines[0], _targetGM.transform.position - _splineContainer.transform.position, out float3 nearest1, out float t1);
        
        float dist =  math.distance(_targetGM.transform.position - _splineContainer.transform.position, nearest1);
        Vector3 targetPosition = nearest1;
        Spline targetSpline = _splineContainer.Splines[0];
        float targetT = t1;
        
         //Ищем, есть ли точка ближе у других линий(нужно, т.к линия разбита на части и при переходе на след. таил, надо найти точку уже на след. линии)
        if (_splineContainer.Splines.Count > 1)
        {
           for (int i = 1; i < _splineContainer.Splines.Count; i++)
           {
              SplineUtility.GetNearestPoint(_splineContainer.Splines[i], _targetGM.transform.position - _splineContainer.transform.position, out float3 nearest, out float t);
              float currentDist = math.distance(_targetGM.transform.position - _splineContainer.transform.position, nearest);

              if (dist > currentDist)
              {
                 dist = currentDist;
                 targetPosition = nearest;
                 targetSpline = _splineContainer.Splines[i];
                 targetT = t;
              }
           }
        }

        //находим нужную точку и движемся к ней
        targetPosition += _splineContainer.transform.position;
        MoveTargetPos(targetPosition);


        //Устанавливаем направление взгляда игрока вдоль линии
        if (_setForwardSpline == true)
        {
           //получаем направление Spline
           Vector3 forward = Vector3.Normalize(targetSpline.EvaluateTangent(targetT));
           //получаем вектор вверх (относительно ориентации самого spline в этой точке)
           Vector3 up = targetSpline.EvaluateUpVector(targetT);
         
           var remappedForward = new Vector3(0, 0, 1);
           var remappedUp = new Vector3(0, 1, 0);
           var axisRemapRotation = Quaternion.Inverse(Quaternion.LookRotation(remappedForward, remappedUp));
         
           //поворот обьекта вдоль spline
           transform.rotation = Quaternion.LookRotation(forward, up) * axisRemapRotation;   
        }
        
        
      }
      
      
   }

   //Переносит игрока на линию движения
   private void MoveTargetPos(Vector3 targetPosition)
   {
      _targetGM.transform.position = targetPosition;
   }
}
