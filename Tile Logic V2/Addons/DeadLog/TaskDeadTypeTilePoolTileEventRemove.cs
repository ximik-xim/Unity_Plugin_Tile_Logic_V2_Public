using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Вернет в пул таил
/// (черз вызов event у самого таила)
/// </summary>
public class TaskDeadTypeTilePoolTileEventRemove : TL_AbsTaskLogicDKO
{
    public override event Action OnInit;
    public override bool IsInit => true;

    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;

    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetData;

    
    private void Awake()
    {
        OnInit?.Invoke();
    }

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
        _isCompletedLogic = false;

        if (tileDKO.ActionIsAlready(_keyGetData.GetData()) == true)
        {
            var data = (DKODataInfoT<PoolTileTypeEvent>)tileDKO.KeyRun(_keyGetData.GetData());
            data.Data.ReleaseThis();
        }

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
