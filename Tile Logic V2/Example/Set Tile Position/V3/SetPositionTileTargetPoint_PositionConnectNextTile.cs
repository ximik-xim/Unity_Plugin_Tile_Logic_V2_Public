using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetPositionTileTargetPoint_PositionConnectNextTile : AbsTileLogicAbsTaskTilePositionInfo
{
#if  UNITY_EDITOR
    
    [SerializeField] 
    private Color _colorGizmo = Color.green;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;

        if (_positionConnectInfo != null)
        {
            Gizmos.DrawCube(_positionConnectInfo.GetPositionConnectNextTile(), new Vector3(0.2f, 0.2f, 0.2f));    
        }
    }
    
#endif
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;
    
    [SerializeField]
    private GameObject _parent;

    [SerializeField] 
    private AbsGetInfoPositionConnectNextTile _positionConnectInfo;
    
    private void Awake()
    {
        OnInit?.Invoke();
    }
    
    public override void StartLogic(SetPositionTileData tileInfo)
    {
        _isCompletedLogic = false;
        
        tileInfo.TilePositionInfo.GetTransformTile().position = _positionConnectInfo.GetPositionConnectNextTile() + tileInfo.TilePositionInfo.GetOffsetTile();
        
        tileInfo.TilePositionInfo.GetTransformTile().parent = _parent.transform;

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
