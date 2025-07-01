using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Action который при обноружении тригера на смерть таила, везмет с таила нужный скрипт и сообщит пулу, о том, что этот обьект пора вернуть в пул
/// (а т.к в этой реализации на таиле есть реализованный интерфеис, то пул, сообит таилу, что тот возращаеться в пул, и логика самого таила и отключит этот таил)
/// </summary>
public class TaskDeadTilePoolRemove : TL_AbsTaskLogicDKO
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;

    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetData;

    [SerializeField] 
    private PoolTileDefault _poolData;
    
    
    private void Awake()
    {
        OnInit?.Invoke();
    }

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
        _isCompletedLogic = false;
        
        if (tileDKO.ActionIsAlready(_keyGetData.GetData()) == true)
        {
           var data = (DKODataInfoT<PoolTileEvent>) tileDKO.KeyRun(_keyGetData.GetData());
           _poolData.ReleaseObject(data.Data); 
        }

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
