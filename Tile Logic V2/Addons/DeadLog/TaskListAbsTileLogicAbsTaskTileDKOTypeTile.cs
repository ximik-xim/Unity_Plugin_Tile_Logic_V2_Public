using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Обертка нужная для запуска(передачи данных)
/// </summary>
public class TaskListAbsTileLogicAbsTaskTileDKOTypeTile : AbsTileLogicAbsTaskDKO
{
    public override event Action OnInit
    {
        add
        {
            _deadFffddf.OnInit += value;
        }
        remove
        {
            _deadFffddf.OnInit -= value;
        }
    }

    public override bool IsInit => _deadFffddf.IsInit;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;

    [SerializeField] 
    private ListAbsTileLogicAbsTaskTileDKOTypeTile _deadFffddf;

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
        _isCompletedLogic = false;

        _deadFffddf.TriggerDeadTile(tileDKO);

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
