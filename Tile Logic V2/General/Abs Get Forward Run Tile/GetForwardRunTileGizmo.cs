using System;
using System.Collections;
using System.Collections.Generic;
#if  UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Служит для получения направления движения таилов
/// (в инспекторе отображает направление в виде стрелки, которое будет возвращать)
/// </summary>
public class GetForwardRunTileGizmo : AbsGetForwardRunTile
{
#if  UNITY_EDITOR
    
    [SerializeField] 
    private Color _colorGizmo = Color.blue;

    [SerializeField] 
    private float _multiplierSizeCone = 1f;

    [SerializeField] 
    private Vector3 _scaleTrail = new Vector3(0.1f, 0.1f, 1);

    private float _defaultSizeCone = 0.3f;
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
        
        Vector3 position = transform.position + transform.forward.normalized / 2 *  _scaleTrail.z;
        Matrix4x4 originalMatrix = Gizmos.matrix;
        
        Quaternion rotation = Quaternion.LookRotation(transform.forward);
        Gizmos.matrix = Matrix4x4.TRS(position, rotation, Vector3.one);

        Gizmos.DrawCube(Vector3.zero, new Vector3(_scaleTrail.x, _scaleTrail.y, _scaleTrail.z ));
        Gizmos.matrix = originalMatrix;
        
        Handles.color = _colorGizmo;
        Handles.ConeHandleCap(0, transform.position + transform.forward.normalized * _scaleTrail.z + transform.forward.normalized * _defaultSizeCone/2 * _multiplierSizeCone, rotation, _defaultSizeCone * _multiplierSizeCone, EventType.Repaint);
    }
    
#endif
    
    public override bool IsInit => true;
    public override event Action OnInit;

    private void Awake()
    {
        OnInit?.Invoke();
    }

    public override Vector3 GetForward()
    {
        return this.transform.forward.normalized;
    }
}
