using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Вызывает логику отключения у таила
/// </summary>
public class TaskDisactiveDeadTile : AbsTileLogicAbsTaskDKO
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
            tileDKO.KeyRun(_keyGetData.GetData());
        }

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
