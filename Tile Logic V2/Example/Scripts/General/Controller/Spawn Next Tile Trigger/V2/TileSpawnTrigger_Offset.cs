using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


///Триггер сообщающий о том, что пора спалвнить следующий таил, и тут же вычесляет смещение которое нужно добавить новому таилу
/// (т.к у колайдера тригера есть размер и идеально по центру него колайдер таила ни когда не окажеться, отсюда и смещение по вектору напровления таила)
/// (расчет идет через точки координат(центры колайдеров) и векторы напровления
/// 1 - это точка колайдера на таиле и вектор напровление движение таила
/// 2 - это точка колайдера триггера спавна и вектор в доль которого будет просиходит обноружениие столкновения

public class TileSpawnTrigger_Offset : AbsTileSpawnTrigger
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action<DKOKeyAndTargetAction> OnTriggerSpawnTile;
    
    [SerializeField] 
    private bool _triggerEnter;

    [SerializeField] 
    private bool _triggerExit;
    
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

    private void OnTriggerEnter(Collider other)
    {
        if (_triggerEnter == true) 
        {
            GetTileDKO(other);    
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_triggerExit == true) 
        {
            GetTileDKO(other);    
        }
    }

    private void GetTileDKO(Collider other)
    {
        AbsTileSpawnMarker marker = other.GetComponent<AbsTileSpawnMarker>();
        
        if (marker != null)
        {
            var forwardRunTile = _forwardRunTile.GetForward().normalized;
            var positionColliderTile = other.transform.position;
            
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

            var offset = pointIntersectionLine_2NormalXYZ - other.transform.position;
            _offsetSpawnTile.SetOffset(offset);
            
            Debug.Log("_pointIntersectionLine1 = " + pointIntersectionLine_1NormalXYZ);
            Debug.Log("_pointIntersectionLine2 = " + pointIntersectionLine_2NormalXYZ);
            
            Debug.Log("offset = " + offset);
            
#if  UNITY_EDITOR
            _pointIntersectionLine = pointIntersectionLine_2NormalXYZ;
            _positionTriggerCollider = _colliderTrigger.transform.position;
            _positionColliderTile = other.transform.position;
#endif
            
            var tileDKO = marker.GetTileDKO();
            OnTriggerSpawnTile?.Invoke(tileDKO);

            //для тестов
            //Time.timeScale = 0;
        }
    }

}
