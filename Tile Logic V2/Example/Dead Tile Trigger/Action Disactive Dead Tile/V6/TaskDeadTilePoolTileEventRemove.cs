using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action который при обноружении тригера на смерть таила, везмет с таила нужный скрипт и вызовет у него event, который сообщит пулу, о том что таил нужно вернут в пул
/// (а значит выключить таил)
/// (а т.к в этой реализации на таиле есть реализованный интерфеис, то пул, сообит таилу, что тот возращаеться в пул, и логика самого таила и отключит этот таил)
/// </summary>
public class TaskDeadTilePoolTileEventRemove : AbsTileLogicAbsTaskDKO
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
            var data = (DKODataInfoT<PoolTileEvent>)tileDKO.KeyRun(_keyGetData.GetData());
            data.Data.ReleaseThis();
        }

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
