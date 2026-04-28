using System;
using UnityEngine;

/// <summary>
/// Обьект который движется вдоль Spline и 2 колайдерами задает разрешенный угол поворота
/// </summary>
public class LimitedRotationTargetGM : MonoBehaviour
{
    [SerializeField]
    private SetGMPositionSpliteAndRb _setGmPositionSpliteAndRb;

    [SerializeField]
    private Vector3 _offset;
    
    [SerializeField]
    private Vector3 _angleLeft;
    
    [SerializeField]
    private Vector3 _angleRight;

    [SerializeField]
    private BoxCollider _boxColliderLeft;
    
    [SerializeField]
    private BoxCollider _boxColliderRight;

    private void FixedUpdate()
    {
        Vector3 startPoint = transform.position + _offset;
        
        Vector3 dir = _setGmPositionSpliteAndRb.ForwardSpline;
        
        Vector3 left = Quaternion.Euler(_angleLeft) * dir;
        Vector3 right = Quaternion.Euler(_angleRight) * dir;

        left = Vector3.Normalize(left);
        right = Vector3.Normalize(right);
        
        _boxColliderLeft.transform.rotation = Quaternion.LookRotation(left);
        _boxColliderRight.transform.rotation = Quaternion.LookRotation(right);
        
        _boxColliderLeft.transform.position = startPoint + left * (_boxColliderLeft.size.z *  0.5f * _boxColliderLeft.transform.localScale.z);
        _boxColliderRight.transform.position = startPoint + right * (_boxColliderRight.size.z * 0.5f * _boxColliderRight.transform.localScale.z);
    }

    private void OnDrawGizmos()
    {
        Vector3 startPoint = transform.position + _offset;
        
        Vector3 dir = _setGmPositionSpliteAndRb.ForwardSpline;
        
        Vector3 left = Quaternion.Euler(_angleLeft) * dir;
        Vector3 right = Quaternion.Euler(_angleRight) * dir;

        // Вектор направления
        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPoint, startPoint + dir * 3f);

        //Векторы ограничения
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPoint, startPoint + left * 3f);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(startPoint, startPoint + right * 3f);
    }
}
