using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// возвращает точку к которой будет стековаться следующий таил
/// (естественно находиться на самом таиле)
/// </summary>
public class PositionConnectNextTile : MonoBehaviour
{
#if  UNITY_EDITOR
    
    [SerializeField] 
    private Color _colorGizmoPointConnect = Color.yellow;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmoPointConnect;
        if (_pointConnect != null)
        {
            Gizmos.DrawCube(_pointConnect.transform.position, new Vector3(0.2f, 0.2f, 0.2f));    
        }
    }
    
#endif
    
    [SerializeField] 
    protected GameObject _pointConnect;

    public Vector3 GetPositionConnectNextTile()
    {
        return _pointConnect.transform.position;
    }
}
