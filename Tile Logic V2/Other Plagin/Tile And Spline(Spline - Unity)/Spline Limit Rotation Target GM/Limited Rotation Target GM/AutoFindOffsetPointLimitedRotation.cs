using System;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;

/// <summary>
/// Автоматически будет опр. что выходим за разрешенный диапазон(жопу заносит)
/// и будет стараться найти точку куда сместить обьект(жопу), что бы оставаться в разрешенном диапазоне
/// </summary>
public class AutoFindOffsetPointLimitedRotation : MonoBehaviour
{
    [SerializeField]
    private SetGMPositionSpliteAndRb _setGmPositionSpliteAndRb;
    
    /// <summary>
    /// Мин угол в разрешенного диапазона
    /// </summary>
    [SerializeField]
    private float _minAngle = 0;
    /// <summary>
    /// Макс угол в разрешенного диапазона
    /// (в некоторых случаях прийдется инвертировать в -180)
    /// </summary>
    [SerializeField]
    private float _maxAngle = 180f;
    
    /// <summary>
    /// Шаг с которым будем искать место
    /// </summary>
    [SerializeField]
    private float _searchStep = 0.02f;
    /// <summary>
    /// Максимальный диапозон пути на котором буду искать(как впереди, так и сзади)
    /// </summary>
    [SerializeField]
    private float _maxOffsetT = 0.5f;
    
    /// <summary>
    /// Использовать ли сглаживание для движение к точке
    /// если его исп. то за кадр, обьект не сместиться на указанную точку(не будет телепорта),
    /// а сместиться в направлении точки(на маленькое расстояние),
    /// а насколько маленькое или большое это будет расстояние определяет _followSpeed
    /// </summary>
    [SerializeField]
    private bool _SmoothingTargetPos = true;
    
    [SerializeField]
    private float _followSpeed = 5f;

    /// <summary>
    /// Как именно буду уст поз игрока, через Rb или просто у трансформа укажу позицию
    /// </summary>
    [SerializeField] 
    private TypeSetPos _typeSetPos;
    
    /// <summary>
    /// Rb обьекта с которого будем брать данные об тек. поз
    /// (и которому будем уст. новую поз)
    /// </summary>
    [SerializeField]
    public Rigidbody _targetRb;
    
    /// <summary>
    /// Обьект с которого будем брать данные об тек. поз
    /// (и которому будем уст. новую поз)
    /// </summary>
    [SerializeField]
    private GameObject _targetGm;

    private MarkerLimitRotation _lastActiveCollider;
    
    private void OnTriggerStay(Collider other)
    {
       var targetCol = other.GetComponent<MarkerLimitRotation>();
       
       if (targetCol != null)
       {
           _lastActiveCollider = targetCol;
           Move(targetCol);
       }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_lastActiveCollider != null)
        {
            var targetCol = other.GetComponent<MarkerLimitRotation>();
            if (_lastActiveCollider == targetCol)
            {
                _lastActiveCollider = null;
            }
        }
    }

    private void FixedUpdate()
    {
        if (_lastActiveCollider == null) 
        {
            //Vector3 smoothedPosition = Vector3.MoveTowards(_targetRb.position, _setGmPositionSpliteAndRb.TargetPositon, _followSpeed * Time.fixedDeltaTime);
            _targetRb.MovePosition(_setGmPositionSpliteAndRb.TargetPositon);
            
        }
    }

    private void Move(MarkerLimitRotation colliderTransform)
    {
        // 1. Текущая позиция на сплайне
        SplineUtility.GetNearestPoint(_setGmPositionSpliteAndRb.CurrentSpline, _targetGm.transform.position, out _, out float t);

        for (float i = _searchStep; i < _maxOffsetT; i += _searchStep)
        {
            Vector3 nextPos;
            Vector3 moveDir;
            float signedAngle;
            Vector3 forwardCol;
            
            if (t - i > 0)
            {
                //Сначала ищем точку сзади
                nextPos = _setGmPositionSpliteAndRb.CurrentSpline.EvaluatePosition(t - i);
                moveDir = (nextPos - _targetGm.transform.position).normalized;

                //Изменяем вычесляймое напровления коллайдера(нужно на случай если диапозон находиться не спереди, сзади например
                forwardCol = colliderTransform.transform.forward;
                if (colliderTransform.BackForward == true) 
                {
                    // разворачивает его на 180 градусов по всем осям
                    forwardCol = -forwardCol;
                }
                
                // 3. Считаем УГОЛ между форвардом колайдера и направлением движения
                // Используем Vector3.up как ось вращения (Y), чтобы получить -180...180
                signedAngle = Vector3.SignedAngle(forwardCol, moveDir, Vector3.up);
                
                // 4. Проверяем, входит ли угол в наш диапазон
                if (signedAngle >= _minAngle && signedAngle <= _maxAngle)
                {
                    MovePos(nextPos);
                    return;
                }
            }

            //Потом ищем точку спереди
            nextPos = _setGmPositionSpliteAndRb.CurrentSpline.EvaluatePosition(t + i);
            moveDir = (nextPos - _targetGm.transform.position).normalized;

            //Изменяем вычесляймое напровления коллайдера(нужно на случай если диапозон находиться не спереди, сзади например
            forwardCol = colliderTransform.transform.forward;
            if (colliderTransform.BackForward == true) 
            {
                // разворачивает его на 180 градусов по всем осям
                forwardCol = -forwardCol;
            }
            
            // 3. Считаем УГОЛ между форвардом колайдера и направлением движения(куда хотим двинуться)
            // Используем Vector3.up как ось вращения (Y), чтобы получить -180...180
            signedAngle = Vector3.SignedAngle(forwardCol, moveDir, Vector3.up);
            
            // 4. Проверяем, входит ли угол в наш диапазон
            if (signedAngle >= _minAngle && signedAngle <= _maxAngle)
            {
                MovePos(nextPos);
                return;
            }
        }
        
        Debug.Log("Точка не была найдена");
    }

    private void MovePos(Vector3 targetPos)
    {
        if (_SmoothingTargetPos == true) 
        {
            //Нужно для плавного движения в сторону точку(иначе может начать телепортироваться
            targetPos = Vector3.MoveTowards(_targetRb.position, targetPos, _followSpeed * Time.fixedDeltaTime);
        }
        
        switch (_typeSetPos)
        {
            case TypeSetPos.SetPosTransform:
            {
                _targetGm.transform.position = targetPos;
            }
                break;
         
            case TypeSetPos.RB_MovePos:
            {
                _targetRb.MovePosition(targetPos);
            }
                break;
        }
        
    }

    private void OnDrawGizmos()
    {
        // Рисуем только если есть активный колайдер и цель
        if (_lastActiveCollider == null || _targetGm == null || _setGmPositionSpliteAndRb.CurrentSpline == null) return;

        Vector3 origin = _targetGm.transform.position;
        
        //Изменяем вычесляймое напровления коллайдера(нужно на случай если диапозон находиться не спереди, сзади например
        Vector3 forward = _lastActiveCollider.transform.forward;
        if (_lastActiveCollider.BackForward == true) 
        {
            // разворачивает его на 180 градусов по всем осям
            forward = -forward;
        }

        // 1. Отрисовка центральной оси колайдера (Синий)
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(origin, forward * 2f);

        // 2. Отрисовка границ разрешенного сектора (Зеленый)
        Gizmos.color = Color.green;
        Vector3 minRay = Quaternion.AngleAxis(_minAngle, Vector3.up) * forward;
        Vector3 maxRay = Quaternion.AngleAxis(_maxAngle, Vector3.up) * forward;

        Gizmos.DrawRay(origin, minRay * 3f);
        Gizmos.DrawRay(origin, maxRay * 3f);

        // Дополнительно: рисуем дугу между лучами для наглядности
        int segments = 10;
        Vector3 lastPoint = minRay;
        for (int i = 1; i <= segments; i++)
        {
            float currentA = Mathf.Lerp(_minAngle, _maxAngle, i / (float)segments);
            Vector3 nextPoint = Quaternion.AngleAxis(currentA, Vector3.up) * forward;
            Gizmos.DrawLine(origin + lastPoint * 3f, origin + nextPoint * 3f);
            lastPoint = nextPoint;
        }

        //Тек. позиция обьекта
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_targetGm.transform.position, 0.1f);



        SplineUtility.GetNearestPoint(_setGmPositionSpliteAndRb.CurrentSpline, _targetGm.transform.position, out _,
            out float t);

        Vector3 nextPos = Vector3.zero;
        Vector3 moveDir = Vector3.zero;
        float signedAngle;
        
        for (float i = _searchStep; i < _maxOffsetT; i += _searchStep)
        {
            if (t - i > 0)
            {
                //Сначала ищем точку сзади
                nextPos = _setGmPositionSpliteAndRb.CurrentSpline.EvaluatePosition(t - i);
                moveDir = (nextPos - _targetGm.transform.position).normalized;

                // 3. Считаем УГОЛ между форвардом колайдера и направлением движения
                // Используем Vector3.up как ось вращения (Y), чтобы получить -180...180
                signedAngle = Vector3.SignedAngle(forward, moveDir, Vector3.up);

                // 4. Проверяем, входит ли угол в наш диапазон
                if (signedAngle >= _minAngle && signedAngle <= _maxAngle)
                {
                    break;
                }
            }

            //Потом ищем точку спереди
            nextPos = _setGmPositionSpliteAndRb.CurrentSpline.EvaluatePosition(t + i);
            moveDir = (nextPos - _targetGm.transform.position).normalized;

            // 3. Считаем УГОЛ между форвардом колайдера и направлением движения(куда хотим двинуться)
            // Используем Vector3.up как ось вращения (Y), чтобы получить -180...180
            signedAngle = Vector3.SignedAngle(forward, moveDir, Vector3.up);

            // 4. Проверяем, входит ли угол в наш диапазон
            if (signedAngle >= _minAngle && signedAngle <= _maxAngle)
            {
                break;
            }

            //на случай если не нашли точки, то просто все обнуляю(что бы не показывать чуш)
            nextPos = Vector3.zero;
            moveDir = Vector3.zero;
        }
        
        //Точка куда надо сместить тек. обьект
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(nextPos, 0.1f);
        Gizmos.DrawLine(origin, origin + moveDir * 4f);
        
        
        
        if (_SmoothingTargetPos == true) 
        {
            //Нужно для плавного движения в сторону точку(иначе может начать телепортироваться
            nextPos = Vector3.MoveTowards(_targetRb.position, nextPos, _followSpeed * Time.fixedDeltaTime);
            
            //Точка куда сместить тек. обьект(с учетом плавного изм)
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(nextPos, 0.2f);
            Gizmos.DrawLine(origin, origin + (nextPos - _targetGm.transform.position).normalized * 4f);
        }
    }
    
    public enum TypeSetPos
    {
        None,
        SetPosTransform,
        RB_MovePos
    }
}
