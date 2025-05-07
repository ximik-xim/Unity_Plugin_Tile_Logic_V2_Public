using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Вернет в пул таил
/// (на прямую, в сам пул передаст ссылку на класс пула у таила)
/// </summary>
public class TaskDeadTypeTilePoolRemove : AbsTileLogicAbsTaskDKO
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;

    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetData;

    [SerializeField] 
    private PoolTileType _poolData;
    
    
    private void Awake()
    {
        OnInit?.Invoke();
    }

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
        _isCompletedLogic = false;
        
        if (tileDKO.ActionIsAlready(_keyGetData.GetData()) == true)
        {
            var data = (DKODataInfoT<PoolTileTypeEvent>) tileDKO.KeyRun(_keyGetData.GetData());
            _poolData.ReleaseObject(data.Data); 
        }

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
