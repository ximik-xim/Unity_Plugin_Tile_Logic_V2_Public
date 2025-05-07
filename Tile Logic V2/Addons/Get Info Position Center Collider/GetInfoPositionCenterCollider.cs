
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Нужен, что бы получить центр позиции колайдера у таила
/// (нужен для реализации расчета пересечения 2 векторов)
/// </summary>
public class GetInfoPositionCenterCollider : MonoBehaviour
{
    [SerializeField] 
    private Collider _collider;

    public Vector3 PositionCenterCollider()
    {
        return _collider.gameObject.transform.position;
    }
}
