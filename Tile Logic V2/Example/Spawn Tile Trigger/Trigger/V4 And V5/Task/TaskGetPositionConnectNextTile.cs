using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Задача на получения координат точки соединение следующего таила с текущим
/// </summary>
public class TaskGetPositionConnectNextTile : AbsTileLogicAbsTaskDKO
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;
    
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetDataPositionConnect;
  
    [SerializeField] 
    private AbsGetInfoPositionConnectNextTile _positionConnectInfo;
    
#if  UNITY_EDITOR
    [SerializeField] 
    private Color _colorGizmo = Color.blue;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;
        Gizmos.DrawCube(this.transform.position, new Vector3(0.2f, 0.2f, 0.2f));
    }
    
#endif
    
    private void Awake()
    {
        OnInit?.Invoke();
    }

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
        _isCompletedLogic = false;
        
        var dataPositionConnect = (DKODataGetInfoPositionConnectNextTile)tileDKO.KeyRun(_keyGetDataPositionConnect.GetData());
        var positionConnect = dataPositionConnect.PositionConnectNextTileInfo.GetPositionConnectNextTile();
            
        _positionConnectInfo.SetPositionConnectNextTile(positionConnect);

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
