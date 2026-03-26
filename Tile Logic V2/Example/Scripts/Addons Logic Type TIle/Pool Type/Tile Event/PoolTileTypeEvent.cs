
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Список событий пула
/// + Event который вернет таил назад в буффер
/// + (т.к по типу) Возращает тип ключа у текущего таила
/// </summary>
public class PoolTileTypeEvent : MonoBehaviour, CustomEventInPool, CustomEventInPoolReleaseThis<PoolTileTypeEvent>, IGetKey<string>
{
    [SerializeField] 
    private GetDataSODataDKODataKey _keyGetDataKey;
    
    [SerializeField] 
    private DKOKeyAndTargetAction _tileDKO;
    
    public event Action<PoolTileTypeEvent> OnReleaseThis;
    
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

    public string GetKey()
    {
        var data = (DKODataGetKeyTipeTile)_tileDKO.KeyRun(_keyGetDataKey.GetData());
        
        return data.GetKey().GetKey();
    }
}
