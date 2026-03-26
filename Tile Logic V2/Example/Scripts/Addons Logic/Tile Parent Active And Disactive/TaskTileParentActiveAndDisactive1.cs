using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public class TaskTileParentActiveAndDisactive1 : TL_AbsTaskLogicDKO
{
    public override event Action OnInit;
    public override bool IsInit => true;
    
    public override event Action OnCompletedLogic;
    public override bool IsCompletedLogic => _isCompletedLogic;
    private bool _isCompletedLogic = true;

    [SerializeField] 
    private bool _setActive;
    
    [SerializeField] 
    private AbsTileParentActiveAndDisactive _tileParentActiveAndDisactive;
    
    private void Awake()
    {
        OnInit?.Invoke();
    }

    public override void StartLogic(DKOKeyAndTargetAction tileDKO)
    {
        _isCompletedLogic = false;

        if (_setActive == true)
        {
            _tileParentActiveAndDisactive.ActiveTile();
        }
        else
        {
            _tileParentActiveAndDisactive.DisactiveTile();
        }

        _isCompletedLogic = true;
        OnCompletedLogic?.Invoke();
    }
}
