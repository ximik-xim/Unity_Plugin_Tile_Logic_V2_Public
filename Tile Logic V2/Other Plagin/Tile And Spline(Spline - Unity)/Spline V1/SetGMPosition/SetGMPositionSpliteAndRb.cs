using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Splines;

/// <summary>
/// Прикрепляет заданный обьект к ломанной линии(Spline) получаемой из контейнера с ломанными линиями
/// Так же сразу поварачивает обьект в напровлении движеня
/// И имеет логику для взаимодействия с RB
/// (если отдельно её выносить, то нужно синхронизироть установку напр. движ. обьекта, а только потом в напр. движ. обьекта применять физику)(гемор)
/// </summary>
public class SetGMPositionSpliteAndRb : MonoBehaviour
{
   public bool IsInit => _isInit;
   private bool _isInit = false;
   public event Action OnInit;

   /// <summary>
   /// Устанавливать ли направление обьекту вдоль линии движения
   /// </summary>
   [SerializeField]
   private bool _setForwardSpline = false;
   
   /// <summary>
   /// Устанавливать ли вектор силы вдоль линии движения
   /// </summary>
   [SerializeField]
   private bool _setForceForwardSpline = false;
   
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
   /// ссылка на RB прикрепленого  обьекта
   /// </summary>
   [SerializeField]
   private Rigidbody _rigidbody;

   [SerializeField] 
   private bool _startAwake = true;

   [SerializeField] 
   private Vector3 _startOffset;

   /// <summary>
   /// Как именно буду уст поз игрока, через Rb или просто у трансформа укажу позицию
   /// </summary>
   [SerializeField] 
   private TypeSetPos _typeSetPos;
   
   /// <summary>
   /// Как именно буду уст скорость
   /// - Резко
   /// - Через плавное добавление AddForce(в этом случае есть некий зазор который надо учесть)
   /// </summary>
   [SerializeField] 
   private TypeSetForceTargetForward _forceTargetForward;

   /// <summary>
   /// Текущий Spline
   /// </summary>
   public Spline CurrentSpline => _currentSpline;
   private Spline _currentSpline;
   public event Action OnUpdateCurrentSpline;

   public Vector3 TargetPositon => _targetPositon;
   private Vector3 _targetPositon;
   public event Action OnUpdateTargetPositon;
   
   /// <summary>
   /// Значение T на текущем Spline
   /// </summary>
   public float CurrentT => _currentT;
   private float _currentT;
   public event Action OnUpdateCurrentSplineT;
   
   /// <summary>
   /// Направление Spline в текущей точке
   /// </summary>
   public Vector3 ForwardSpline => _forwardSpline;
   private Vector3 _forwardSpline;
   public event Action OnUpdateCurrentSplineForward;
   
   private bool _isStart = false;
   
   private void Awake()
   {
      Init();
   }
   
   private void Init()
   {
      if (_startAwake == true)
      {
         StartLogic();
         _isStart = true;
      }
      
      _isInit = true;
      OnInit?.Invoke();
   }

   public void StartLogic()
   {
      //Устанавливаем позицию у игрока
      _targetGM.transform.position = _splineContainer.Splines[0].EvaluatePosition(0);
      _targetGM.transform.position += _splineContainer.transform.position + _startOffset;
      
      _isStart = true;
   }

   public void StartSetPosition()
   {
      _isStart = true;
   }
   
   public void StopSetPosition()
   {
      _isStart = false;
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

        //получаем направление Spline
        Vector3 splineForward = Vector3.Normalize(targetSpline.EvaluateTangent(targetT));
        Vector3 worldForward = _splineContainer.transform.TransformDirection(splineForward);
        
        //Устанавливаем направление взгляда игрока вдоль линии
        if (_setForwardSpline == true)
        {
// 1. Получаем направление кривой в конкретной точке t (нормализованное)
           // ВАЖНО: EvaluateTangent возвращает вектор в локальном пространстве сплайна
           splineForward = Vector3.Normalize(targetSpline.EvaluateTangent(targetT));
           Vector3 splineUp = Vector3.Normalize(targetSpline.EvaluateUpVector(targetT));

           // 2. Переводим локальный вектор сплайна в мировой, чтобы вращение было корректным 
           // относительно мировой сцены (если контейнер сплайна повернут)
           worldForward = _splineContainer.transform.TransformDirection(splineForward);
           Vector3 worldUp = _splineContainer.transform.TransformDirection(splineUp);

           // 3. Устанавливаем вращение. 
           // LookRotation делает ось Z объекта направленной вдоль worldForward.
           if (worldForward.sqrMagnitude > 0.001f)
           {
              _targetGM.transform.rotation = Quaternion.LookRotation(worldForward, worldUp);
           }
        }

        // Указываем вектор направления вдоль линии
        if (_setForceForwardSpline == true)
        {
           Vector3 engineForward = _targetGM.transform.forward;
         
           //Проверяем направление скорости
           if (Vector3.Dot(_rigidbody.linearVelocity, _targetGM.transform.forward) < 0)
           {
              engineForward *= -1;
           }
        
           SetForceTargetForward(engineForward);   
        }

        if (_targetPositon != targetPosition) 
        {
           _targetPositon = targetPosition;
           OnUpdateTargetPositon?.Invoke();
        }

        if (_currentSpline != targetSpline) 
        {
           _currentSpline = targetSpline;   
           OnUpdateCurrentSpline?.Invoke();
        }

        _currentT = targetT;
        OnUpdateCurrentSplineT?.Invoke();

        if (_forwardSpline != worldForward) 
        {
           _forwardSpline = worldForward;
           OnUpdateCurrentSplineForward?.Invoke();
        }
      }
      
      
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.green;
      Gizmos.DrawLine(transform.position, transform.position + transform.forward * 5f);

      // if (_currentSpline!=null)
      // {
      //    Vector3 splineForward = Vector3.Normalize(_currentSpline.EvaluateTangent(_currentT));
      //
      //    Gizmos.color = Color.red;
      //    Gizmos.DrawLine(transform.position, transform.position + splineForward * 5f);
      //    
      //    Vector3 splineUp = Vector3.Normalize(_currentSpline.EvaluateUpVector(_currentT));
      //    
      //    Gizmos.color = Color.yellow;
      //    Gizmos.DrawLine(transform.position, transform.position + splineUp * 5f);
      //    
      //    
      //    //
      //    Vector3 splineForwardWorld = Vector3.Normalize(_currentSpline.EvaluateTangent(_currentT));
      //    
      //    Gizmos.color = Color.blue;
      //    Gizmos.DrawLine(transform.position, transform.position + _splineContainer.transform.TransformDirection(splineForward) * 5f);
      //    
      //    Vector3 splineUpWorld = Vector3.Normalize(_currentSpline.EvaluateUpVector(_currentT));
      //    
      //    Gizmos.color = Color.gray;
      //    Gizmos.DrawLine(transform.position, transform.position + _splineContainer.transform.TransformDirection(splineUp) * 5f);
      // }
      
   }

   //Переносит игрока на линию движения
   private void MoveTargetPos(Vector3 targetPosition)
   {
      switch (_typeSetPos)
      {
         case TypeSetPos.SetPosTransform:
         {
            _targetGM.transform.position = targetPosition;
         }
            break;
         
         case TypeSetPos.RB_MovePos:
         {
            _rigidbody.MovePosition(targetPosition);
         }
            break;
      }
   }
   
   private void SetForceTargetForward(Vector3 engineForward)
   {
      switch (_forceTargetForward)
      {
         case TypeSetForceTargetForward.SetVelocity:
         {
            //Сохраняет скорость по модулю и резко перенапровляет её вдоль движения
            _rigidbody.linearVelocity = _rigidbody.linearVelocity.magnitude * engineForward;
         }
            break;
         
         case TypeSetForceTargetForward.AddForce:
         {
            //Вычисляет желаемую скорость и через AddForce меняет плавно напровление
            Vector3 desiredVelocity = engineForward * _rigidbody.linearVelocity.magnitude;
      
            Vector3 steering = desiredVelocity - _rigidbody.linearVelocity;
      
            _rigidbody.AddForce(steering, ForceMode.Acceleration);
         }
            break;
      }
      

   }
   
   
   public enum TypeSetPos
   {
      None,
      SetPosTransform,
      RB_MovePos
   }
   
   public enum TypeSetForceTargetForward
   {
      None,
      SetVelocity,
      AddForce
   }

}




