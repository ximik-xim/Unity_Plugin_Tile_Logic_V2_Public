using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Это лиш пример наброска. Можно много способов придумать как реализовать так, что бы таилы точно друг за другом шли
/// - можно через вычесление размеров таила и определение позиции начала таила
/// - можно что бы transform сразу возращал позицию начала таила
/// - можно узнавать координаты позиции края последнего таила(наверное самое лучшее) и к нему стековать новый таил
/// - и т.д
/// </summary>
public class SetPositionTileTargetPoint : AbsTileLogicAbsTaskTilePositionInfo
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
    

    [SerializeField] 
    private GameObject _targetPointSpawn;

    [SerializeField] 
    private GameObject _parent;

    private void Awake()
    {
        OnInit?.Invoke();
    }
    
    public override void StartLogic(SetPositionTileData tileInfo)
    {
        _isCompletedLogic = false;
        
        tileInfo.TilePositionInfo.GetTransformTile().position = _targetPointSpawn.transform.position + tileInfo.TilePositionInfo.GetOffsetTile();
        tileInfo.TilePositionInfo.GetTransformTile().parent = _parent.transform;

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
