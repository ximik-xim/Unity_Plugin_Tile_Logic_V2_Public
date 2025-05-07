using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class TilePositionInfoGizmo : AbsTilePositionInfo
{
#if  UNITY_EDITOR
    
    [SerializeField] 
    private Color _colorGizmoTransformTile = Color.green;
    
    [SerializeField] 
    private Color _colorGizmoPointConnect = Color.yellow;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmoTransformTile;
        if (_transformTile != null)
        {
            Gizmos.DrawCube(_transformTile.transform.position, new Vector3(0.2f, 0.2f, 0.2f));    
        }
        
        Gizmos.color = _colorGizmoPointConnect;
        if (_pointConnect != null)
        {
            Gizmos.DrawCube(_pointConnect.transform.position, new Vector3(0.2f, 0.2f, 0.2f));    
        }
    }
    
#endif
    
    [SerializeField] 
    protected GameObject _pointConnect;

    public override Vector3 GetOffsetTile()
    {
        return _transformTile.transform.position - _pointConnect.transform.position;
    }
}
