using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Задача, на вычесления идеальной точки пересечения колайдеров талов(с учетом их векторов напровления), и нахождения итогового смещения
/// </summary>
public class TaskCalculatingOffsetIntersectionLines : TL_AbsTaskLogicDKO
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetDataPositionConnect;
    
    [SerializeField] 
    private Collider _colliderTrigger;
    [SerializeField] 
    private AbsGetForwardRunTile _forwardСollider;

    [SerializeField] 
    private AbsOffsetPositionTile _offsetSpawnTile;
    [SerializeField] 
    private AbsGetForwardRunTile _forwardRunTile;
    
    
    
#if  UNITY_EDITOR
    private Vector3 _pointIntersectionLine;
    private Vector3 _positionTriggerCollider;
    private Vector3 _positionColliderTile;
    
    [SerializeField] 
    private Color _colorGizmo = Color.blue;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
        
        Color col;

        col = Color.magenta;
        Gizmos.DrawLine(_positionTriggerCollider,_pointIntersectionLine);
        Gizmos.DrawLine(_positionColliderTile,_pointIntersectionLine);
        
        col = Color.red;
        col.a = 0.7f;
        Gizmos.color = col;
        Gizmos.DrawSphere(_pointIntersectionLine,0.1f);
    }
    
#endif
    
    private void Awake()
    {
        OnInit?.Invoke();
    }

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
            _isCompletedLogic = false;
            
            var dataPositionCollider = (DKODataGetInfoPositionCenterCollider)tileDKO.KeyRun(_keyGetDataPositionConnect.GetData());
            Vector3 positionCenterCollider = dataPositionCollider.InfoPositionCenterCollider.PositionCenterCollider();
        
            var forwardRunTile = _forwardRunTile.GetForward().normalized;
            var positionColliderTile = positionCenterCollider;
            
            var forwardTriggerCollider = _forwardСollider.GetForward().normalized;
            var positionTriggerCollider = _colliderTrigger.transform.position;
            
            Vector3 pointIntersectionLine1 = Vector3.zero;
            Vector3 pointIntersectionLine2 = Vector3.zero;
        
            //нужны т.к нахожу не относ. X, а относительно другой плоскости
            Vector3 pointIntersectionLine_1NormalXYZ;
            Vector3 pointIntersectionLine_2NormalXYZ;
        
            //работает нормально
            StaticCustomMethodVector.FindPointIntersectionV3(
                new Vector3(positionTriggerCollider.x, positionTriggerCollider.z, positionTriggerCollider.y),
                new Vector3(forwardTriggerCollider.x, forwardTriggerCollider.z, forwardTriggerCollider.y),
                new Vector3(positionColliderTile.x, positionColliderTile.z, positionColliderTile.y),
                new Vector3(forwardRunTile.x, forwardRunTile.z, forwardRunTile.y),
                out pointIntersectionLine1,out pointIntersectionLine2);

            //точка пересечения в напр. 1 линии
            pointIntersectionLine_1NormalXYZ = new Vector3(pointIntersectionLine1.x, pointIntersectionLine1.z, pointIntersectionLine1.y);
            //точка пересечения в напр. 2 линии
            pointIntersectionLine_2NormalXYZ = new Vector3(pointIntersectionLine2.x, pointIntersectionLine2.z, pointIntersectionLine2.y);

            var offset = pointIntersectionLine_2NormalXYZ - positionCenterCollider;
            _offsetSpawnTile.SetOffset(offset);
            
            Debug.Log("_pointIntersectionLine1 = " + pointIntersectionLine_1NormalXYZ);
            Debug.Log("_pointIntersectionLine2 = " + pointIntersectionLine_2NormalXYZ);
            
            Debug.Log("offset = " + offset);
            
#if  UNITY_EDITOR
            _pointIntersectionLine = pointIntersectionLine_2NormalXYZ;
            _positionTriggerCollider = _colliderTrigger.transform.position;
            _positionColliderTile = positionCenterCollider;
#endif

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
