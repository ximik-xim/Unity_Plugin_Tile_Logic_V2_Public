using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

/// <summary>
/// Список событий пула
/// + Event который вернет таил назад в буффер
/// </summary>
public class PoolTileEvent : MonoBehaviour, CustomEventInPool, CustomEventInPoolReleaseThis<PoolTileEvent>
{
    [SerializeField] 
    private DKOKeyAndTargetAction _tileDKO;
    
    public event Action<PoolTileEvent> OnReleaseThis;
    
    [SerializeField]
    private LogicListTaskDKO _beforeGetTask;
    
    [SerializeField]
    private LogicListTaskDKO _afterGetTask;
    
    [SerializeField]
    private LogicListTaskDKO _beforeReleaseTask;
    
    [SerializeField]
    private LogicListTaskDKO _aftereReleaseTask;

    public void OnBeforeGetObject()
    {
        _beforeGetTask.StartAction(_tileDKO);
    }

    public void OnAfterGetObject()
    {
        _afterGetTask.StartAction(_tileDKO);
    }

    public void OnBeforeReleaseObject()
    {
        _beforeReleaseTask.StartAction(_tileDKO);
    }

    public void OnAfterReleaseObject()
    {
        _aftereReleaseTask.StartAction(_tileDKO);
    }

    public void ReleaseThis()
    {
        OnReleaseThis?.Invoke(this);
    }

    public DKOKeyAndTargetAction GetTileDKO()
    {
        return _tileDKO;
    }
    
}
