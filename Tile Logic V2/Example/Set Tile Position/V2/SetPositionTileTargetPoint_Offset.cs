using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// логика установки позиции заспалвеного таила, с учетом найденного смещения между иделаьной точкой пересечения и текущей точкой
/// </summary>
public class SetPositionTileTargetPoint_Offset : AbsTileLogicAbsTaskTilePositionInfo
{
#if  UNITY_EDITOR
    
    [SerializeField] 
    private Color _colorGizmo = Color.green;
    
    private void OnDrawGizmos()
    {
        Gizmos.color = _colorGizmo;

        if (_targetPointSpawn != null)
        {
            Gizmos.DrawCube(this._targetPointSpawn.transform.position, new Vector3(0.2f, 0.2f, 0.2f));    
        }
    }
    
#endif
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;

    /// <summary>
    /// Учитывать ли найденное смещение
    /// </summary>
    [SerializeField] 
    private bool _checkOffset = true;
    

    [SerializeField] 
    private GameObject _targetPointSpawn;

    [SerializeField] 
    private GameObject _parent;

    [SerializeField] 
    private AbsOffsetPositionTile _offsetSpawnTile;
    
    private void Awake()
    {
        OnInit?.Invoke();
    }
    
    public override void StartLogic(SetPositionTileData tileInfo)
    {
        _isCompletedLogic = false;
        
        tileInfo.TilePositionInfo.GetTransformTile().position = _targetPointSpawn.transform.position + tileInfo.TilePositionInfo.GetOffsetTile();
        if (_checkOffset == true)
        {
            tileInfo.TilePositionInfo.GetTransformTile().position -= _offsetSpawnTile.GetOffset();    
        }
        tileInfo.TilePositionInfo.GetTransformTile().parent = _parent.transform;

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
